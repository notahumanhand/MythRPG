
namespace MythRPG.Core
{
    public interface ICharactersRepository
    {
        void AddCharacter(Character character);
        void DeleteCharacter(int id);
        List<Character> GetCharacters();
        List<Character> ListCharacters();
        Character GetCharacterById(int id);
        List<Character> GetCharactersByClass(string charclass);
        List<Character> SearchCharacters(string name);
        void UpdateCharacter(int id, Character character);
        void RemoveTrait(int CharId, int TraitId);
        void RemoveSpell(int CharId, int SpellId);
        void LevelUpCharacter(int id);
    }
}