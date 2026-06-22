namespace MythRPG.Core.Interfaces
{
    public interface ITraitsRepository
    {
        void AddTrait(Trait trait, List<int>? eligibleClassIds);
        void AddTraitToCharacter(int charId, Trait trait);
        void AddBonus(Bonus bonus);
        void DeleteTrait(int id);
        List<Trait> GetTraits();
        List<Trait> ListTraits();
        List<Bonus> GetBonuses();
        Trait? GetTraitById(int id);
        Bonus? GetBonusById(int id);
        Trait? GetTraitByName(string name);
        void UpdateTrait(int id, Trait trait, List<int>? eligibleClassIds);
        List<Trait> GetAvailableTraitsForClass(int classId);
    }
}