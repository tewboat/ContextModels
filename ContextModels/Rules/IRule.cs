namespace ContextModels.Rules;

internal interface IRule
{
    bool Validate(string text, int position);
}