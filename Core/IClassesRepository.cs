namespace MythRPG.Core
{
    public interface IClassesRepository
    {
        void AddClass(CharacterClass characterClass);
        List<CharacterClass> GetClasses();
        List<CharacterClass> ListClasses();
        CharacterClass GetClassById(int id);
        void UpdateClass(int id, CharacterClass characterClass);
        void DeleteClass(int id);
        void AddSpellColourToClass(int classId, int spellColourId);
        void RemoveSpellColourFromClass(int classId, int spellColourId);
        void AddTraitToClass(int classId, int traitId);
        void RemoveTraitFromClass(int classId, int traitId);
    }
}