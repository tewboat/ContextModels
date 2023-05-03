namespace ContextModels;

using System.Text;
using Encoders;
using Models;
using Rules;
using Rules.CapitalizationRules;

internal static class Program
{
    private static readonly IReadOnlyCollection<IRule> Rules = new IRule[]
        { new FirstLetterInParagraphRule(), new PronounRule(), new LetterAfterEndOfSentenceSignRule() };

    private const byte ByteSize = sizeof(byte) * 8;

    public static async Task Main()
    {
        const int k = 3;
        using var streamReader = new StreamReader(@"doyle_the_adventures.txt");
        var text = await streamReader.ReadToEndAsync();

        var positions = new byte[(int)Math.Ceiling(text.Length / (double)ByteSize)];
        for (var i = 0; i < text.Length; i++)
        {
            var index = i / ByteSize;
            if (char.IsUpper(text[i]) && !CompliesWithRules(text, i))
                positions[index] += (byte)(1 << (ByteSize - 1 - i % ByteSize));
        }
        
        var entropy = GetEntropy(positions);
        Console.WriteLine($"Capital letters byte array entropy: {entropy}");

        new ArithmeticEncoder(12).Encode(positions);
        new DeltaEncoder().Encode(positions);

        var contextModels = GetContextModels(text, k);
        Console.WriteLine($"ContextModels size {GetSize(contextModels.Values)} bytes");
    }

    private static double GetEntropy(byte[] bytes)
    {
        var prob = new int[256];
        foreach (var b in bytes)
            prob[b]++;

        return prob
            .Where(p => p != 0)
            .Select(p => p / (double)bytes.Length)
            .Select(probability => probability * -Math.Log2(probability))
            .Sum();
    }

    private static long GetSize(IEnumerable<ContextModel> models)
    {
        var size = 0L;
        foreach (var model in models)
        {
            size += model.String.Length * 2; // Считаем, что каждый символ куодируется двумя байтами
            size += 4; // размер int
            size += model.CharactersEncounteredWithLeftTextCount * (2 + 4); // количество пар (char, int) умноженное на размер пары в байтах 
        }

        return size;
    }


    private static Dictionary<string, ContextModel> GetContextModels(string text, int k)
    {
        var contextModels = new Dictionary<string, ContextModel>();

        void UpdateContextModel(string substring, char nextCharacter)
        {
            if (!contextModels!.TryGetValue(substring, out var model))
            {
                model = new ContextModel(substring);
                contextModels[substring] = model;
            }

            model.AddOccurence(nextCharacter);
        }

        for (var i = 0; i < text.Length; i++)
        {
            var substring = string.Empty;
            UpdateContextModel(substring, char.ToLower(text[i]));

            for (var j = i; j < Math.Min(i + k, text.Length); j++)
            {
                substring += char.ToLower(text[j]);
                if (j < text.Length - 1)
                    UpdateContextModel(substring, char.ToLower(text[j + 1]));
            }
        }

        return contextModels;
    }

    private static bool CompliesWithRules(string text, int position)
    {
        return Rules.Any(rule => rule.Validate(text, position));
    }
}