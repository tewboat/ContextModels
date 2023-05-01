namespace ContextModels;

using System.Text;
using Encoders;
using Models;
using Rules;
using Rules.CapitalizationRules;

internal static class Program
{
    private static readonly IReadOnlyCollection<IRule> Rules = new IRule[]
        {new FirstLetterInParagraphRule(), new PronounRule()};

    private static readonly DeltaEncoder DeltaEncoder = new(new GammaEncoder());
    private static readonly BinaryStringToIntegersEncoder BinaryStringToIntegersEncoder = new();
    private static readonly ArithmeticEncoder arithmeticEncoder = new ArithmeticEncoder();

    public static async Task Main(string[] args)
    {
        const int k = 3;
        using var streamReader =
            new StreamReader(@"E:\University\СSharp\ContextModels\ContextModels\doyle_the_adventures.txt");
        var text = await streamReader.ReadToEndAsync();

        var encodedPositions = new StringBuilder();

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
                if (char.IsUpper(text[j]) && !CompliesWithRules(text, j))
                {
                    var encodedPosition = DeltaEncoder.Encode(j + 1);
                    encodedPositions.Append(encodedPosition);
                }

                substring += char.ToLower(text[j]);
                if (j < text.Length - 1)
                    UpdateContextModel(substring, char.ToLower(text[j + 1]));
            }
        }

        var encodedUpperLettersPositions = BinaryStringToIntegersEncoder.Encode(encodedPositions.ToString());
    }


    private static bool CompliesWithRules(string text, int position)
    {
        return Rules.Any(rule => rule.Validate(text, position));
    }
}