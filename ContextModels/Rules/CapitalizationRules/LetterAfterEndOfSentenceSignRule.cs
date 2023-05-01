namespace ContextModels.Rules.CapitalizationRules;

internal sealed class LetterAfterEndOfSentenceSignRule : IRule
{
    public bool Validate(string text, int position)
    {
        return position >= 2 && char.IsWhiteSpace(text[position - 1]) && text[position - 2] is '!' or '.' or '?';
    }
}