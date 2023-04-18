namespace ContextModels.Models;

internal sealed class ContextModel
{
    public string String { get; }
    public int OccurrencesCounter { get; private set; }
    public IReadOnlyDictionary<char, int> CharactersEncounteredWithLeftText => charactersEncounteredWithLeftText;
    public int CharactersEncounteredWithLeftTextCount => charactersEncounteredWithLeftText.Count;

    private readonly Dictionary<char, int> charactersEncounteredWithLeftText;

    public ContextModel(string @string)
    {
        String = @string;
        charactersEncounteredWithLeftText = new Dictionary<char, int>();
    }

    public void AddOccurence(char nextCharacter)
    {
        OccurrencesCounter++;
        charactersEncounteredWithLeftText.TryAdd(nextCharacter, 0);
        charactersEncounteredWithLeftText[nextCharacter]++;
    }
}