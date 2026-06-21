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
        public void AddTrait(Trait trait)
        {
            var db = contextFactory;
            db.Traits.Add(trait);
            db.SaveChanges();
        }
        public void AddTrait(Trait trait, List<int> eligibleClassIds)
        {
            var db = contextFactory;

            if (eligibleClassIds.Count > 0)
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
        public Trait GetTraitById(int id)
        {
            List<Trait> traits = GetTraits();

            foreach (var trait in traits)
            {
                if (trait.TraitId == id)
                {
                    return trait;
                }
            }
            return new Trait
            {
                Name = string.Empty
            };
        }
        public Trait GetTraitBonusById(int id)
        {
            var db = contextFactory;
            List<Trait> traits = GetTraits();
            foreach (var trait in traits)
            {
                if (trait.TraitId == id) return trait;
            }
            return new Trait
            {
                Name = string.Empty
            };
        }
        public Bonus? GetBonusById(int id)
        {
            var db = contextFactory;
            var bonus = db.Bonuses.Find(id);
            if (bonus is not null) return bonus;
            return null;
        }
        public Trait GetTraitByName(string name)
        {
            var db = contextFactory;
            List<Trait> traits = ListTraits();
            foreach (var trait in traits)
            {
                if (trait.Name is not null && trait.Name.Equals(name)) return trait;
            }
            return new Trait
            {
                Name = string.Empty
            };
        }
        public Trait GetTraitBonusByName(string name)
        {
            var db = contextFactory;
            List<Trait> traits = GetTraits();
            foreach (var trait in traits)
            {
                if (trait.Name is not null && trait.Name.Equals(name)) return trait;
            }
            return new Trait
            {
                Name = string.Empty
            };
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
        public void UpdateTrait(int id, Trait trait, List<int> eligibleClassIds)
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

            var traitToUpdate =
                db.Traits
                    .Include(t => t.EligibleClasses)
                    .FirstOrDefault(
                        t => t.TraitId == id);

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

            if (eligibleClassIds.Count > 0)
            {
                var selectedClasses =
                    db.CharacterClasses
                        .Where(c =>
                            eligibleClassIds.Contains(
                                c.CharacterClassId))
                        .ToList();

                foreach (var characterClass in selectedClasses)
                {
                    traitToUpdate.EligibleClasses.Add(
                        characterClass);
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
            List<Trait> traits = GetTraits();

            return traits
                .Where(t =>
                    t.EligibleClasses.Count == 0 ||
                    t.EligibleClasses.Any(
                        c => c.CharacterClassId == classId))
                .ToList();
        }
    }
}
