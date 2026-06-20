namespace MythRPG.Core
{
    public interface ISpellColoursRepository
    {
        List<SpellColour> GetSpellColours();

        SpellColour? GetSpellColourById(int id);

        SpellColour? GetSpellColourByName(string name);
    }
}