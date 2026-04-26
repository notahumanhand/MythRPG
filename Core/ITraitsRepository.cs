
namespace MythRPG.Core
{
    public interface ITraitsRepository
    {
        void AddTrait(Trait trait);
        void AddTraitToCharacter(int charId, Trait trait);
        void AddBonus(Bonus bonus);
        void DeleteTrait(int id);
        List<Trait> GetTraits();
        List<Trait> ListTraits();
        List<Bonus> GetBonuses();
        Trait GetTraitById(int id);
        Trait GetTraitBonusById(int id);
        Bonus? GetBonusById(int id);
        Trait GetTraitByName(string name);
        Trait GetTraitBonusByName(string name);
        void UpdateTrait(int id, Trait trait);
    }
}