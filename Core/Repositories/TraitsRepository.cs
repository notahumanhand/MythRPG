using Microsoft.EntityFrameworkCore;
using MythRPG.Core.Interfaces;
using MythRPG.Data;

namespace MythRPG.Core.Repositories
{
    public class TraitsRepository : ITraitsRepository
    {
        private readonly MythRPGContext contextFactory;
        public TraitsRepository(MythRPGContext contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public void AddTrait(Trait trait, List<int>? eligibleClassIds)
        {
            var db = contextFactory;

            if (eligibleClassIds is not null && eligibleClassIds.Count > 0)
            {
                var selectedClasses = db.CharacterClasses.Where(c => eligibleClassIds.Contains(c.CharacterClassId)).ToList();

                foreach (var characterClass in selectedClasses)
                {
                    trait.EligibleClasses.Add(characterClass);
                }
            }

            db.Traits.Add(trait);
            db.SaveChanges();
        }
        public void AddBonus(Bonus bonus)
        {
            var db = contextFactory;
            db.Bonuses.Add(bonus);
            db.SaveChanges();
        }
        public void AddTraitToCharacter(int charId, Trait trait)
        {
            var db = contextFactory;
            var characterToUpdate = db.Characters.Find(charId);
            if (characterToUpdate is not null)
            {
                characterToUpdate.Traits.Add(trait);
                db.SaveChanges();
            }
        }
        public List<Trait> GetTraits()
        {
            var db = contextFactory;
            return db.Traits.Include(t => t.Bonuses).Include(t => t.EligibleClasses).ToList();
        }
        public List<Trait> ListTraits()
        {
            var db = contextFactory;
            return db.Traits.ToList();
        }
        public List<Bonus> GetBonuses()
        {
            var db = contextFactory;
            return db.Bonuses.ToList();
        }
        public Trait? GetTraitById(int id)
        {
            var db = contextFactory;
            return db.Traits.Include(t => t.EligibleClasses).FirstOrDefault(t => t.TraitId == id);
        }
        public Bonus? GetBonusById(int id)
        {
            var db = contextFactory;
            return db.Bonuses.Find(id);
        }
        public Trait? GetTraitByName(string name)
        {
            var db = contextFactory;
            return db.Traits.FirstOrDefault(t => t.Name == name);
        }
        public void UpdateTrait(int id, Trait trait)
        {
            if (trait == null)
            {
                throw new ArgumentNullException(nameof(trait));
            }
            if (id != trait.TraitId)
            {
                return;
            }

            var db = contextFactory;
            var traitToUpdate = db.Traits.Find(id);

            if (traitToUpdate is not null)
            {
                traitToUpdate.Name = trait.Name;
                traitToUpdate.Source = trait.Source;
                traitToUpdate.Description = trait.Description;
                traitToUpdate.ResourceCost = trait.ResourceCost;
                traitToUpdate.ActionCost = trait.ActionCost;

                db.SaveChanges();
            }
        }
        public void UpdateTrait(int id, Trait trait, List<int>? eligibleClassIds)
        {
            if (trait == null)
            {
                throw new ArgumentNullException(nameof(trait));
            }

            if (id != trait.TraitId)
            {
                return;
            }

            var db = contextFactory;

            var traitToUpdate = db.Traits.Include(t => t.EligibleClasses).FirstOrDefault(t => t.TraitId == id);

            if (traitToUpdate is null)
            {
                return;
            }

            traitToUpdate.Name = trait.Name;
            traitToUpdate.Source = trait.Source;
            traitToUpdate.Description = trait.Description;
            traitToUpdate.ResourceCost = trait.ResourceCost;
            traitToUpdate.ActionCost = trait.ActionCost;
            traitToUpdate.EligibleClasses.Clear();

            if (eligibleClassIds is not null && eligibleClassIds.Count > 0)
            {
                var selectedClasses = db.CharacterClasses.Where(c => eligibleClassIds.Contains(c.CharacterClassId)).ToList();

                foreach (var characterClass in selectedClasses)
                {
                    traitToUpdate.EligibleClasses.Add(characterClass);
                }
            }

            db.SaveChanges();
        }
        public void DeleteTrait(int id)
        {
            var db = contextFactory;
            var trait = db.Traits.Find(id);
            if (trait is null) return;
            db.Traits.Remove(trait);
            db.SaveChanges();
        }
        public List<Trait> GetAvailableTraitsForClass(int classId)
        {
            var db = contextFactory;
            return db.Traits.Where(t => !t.EligibleClasses.Any() || t.EligibleClasses.Any(c => c.CharacterClassId == classId)).ToList();
        }
    }
}
