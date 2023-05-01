namespace ContextModels;

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
        using var streamReader =
            new StreamReader(@"E:\University\СSharp\ContextModels\ContextModels\doyle_the_adventures.txt");
        var text = await streamReader.ReadToEndAsync();

        var contextModels = GetContextModels(text, k);

        var positions = new byte[(int)Math.Ceiling(text.Length / (double)ByteSize)];
        for (var i = 0; i < text.Length; i++)
        {
            var index = i / ByteSize;
            if (char.IsUpper(text[i]) && !CompliesWithRules(text, i))
                positions[index] += (byte)(1 << (ByteSize - 1 - i % ByteSize));
        }

        var encoded = new ArithmeticEncoder().Encode(positions);

        var enthropy = GetEnthropy(positions);

        var length = 0d;
        var prob = new int[256];
        foreach (var b in positions)
            prob[b]++;
        for (var i = 0; i < 256; i++)
        {
            length += prob[i] * (Math.Floor(Math.Log2(i + 1)) +
                                 2 * Math.Floor(Math.Log2(Math.Floor(Math.Log2(i + 1)) + 1)) + 1);
        }
    }

    private static double GetEnthropy(byte[] bytes)
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