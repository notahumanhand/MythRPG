
namespace MythRPG.Core
{
    public interface ISpellsRepository
    {
        void AddSpell(Spell spell);
        void AddSpellToCharacter(int charId, Spell spell);
        void DeleteSpell(int id);
        Spell GetSpellById(int id);
        Spell? GetSpellByName(string name);
        List<Spell> GetSpells();
        void UpdateSpell(int id, Spell spell);
    }
}