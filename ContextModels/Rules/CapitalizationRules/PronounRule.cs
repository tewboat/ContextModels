namespace ContextModels.Rules.CapitalizationRules;

internal sealed class PronounRule : IRule
{
    public bool Validate(string text, int position)
    {
        return position != 0 && position < text.Length - 1 && !char.IsLetter(text[position - 1]) &&
               !char.IsLetter(text[position + 1]);
    }
}