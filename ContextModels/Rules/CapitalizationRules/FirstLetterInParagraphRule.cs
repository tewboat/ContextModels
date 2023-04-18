namespace ContextModels.Rules.CapitalizationRules;

internal sealed class FirstLetterInParagraphRule : IRule
{
    public bool Validate(string text, int position)
    {
        return (position > 0 && text[position - 1] == '\n')
            || (position >= 2 && !char.IsLetter(text[position - 1]) && text[position - 2] == '\n');
    }
}