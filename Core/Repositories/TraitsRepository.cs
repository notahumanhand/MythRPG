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
        public void AddBonusToTrait(int traitId, int bonusId)
        {
            var db = contextFactory;

            var trait = db.Traits
                .Include(t => t.Bonuses)
                .FirstOrDefault(t => t.TraitId == traitId);

            if (trait is null)
            {
                return;
            }

            var bonus = db.Bonuses.Find(bonusId);

            if (bonus is null)
            {
                return;
            }

            if (trait.Bonuses.Any(b => b.BonusId == bonusId))
            {
                return;
            }

            trait.Bonuses.Add(bonus);

            db.SaveChanges();
        }
        public void UpdateBonus(int id, Bonus bonus)
        {
            if (bonus == null)
            {
                throw new ArgumentNullException(nameof(bonus));
            }

            if (id != bonus.BonusId)
            {
                return;
            }

            var db = contextFactory;

            var bonusToUpdate = db.Bonuses.Find(id);

            if (bonusToUpdate is null)
            {
                return;
            }

            bonusToUpdate.Type = bonus.Type;
            bonusToUpdate.Modifies = bonus.Modifies;
            bonusToUpdate.Amount = bonus.Amount;

            db.SaveChanges();
        }
        public void DeleteBonus(int id)
        {
            var db = contextFactory;

            var bonus = db.Bonuses.Find(id);

            if (bonus is null)
            {
                return;
            }

            var traits = db.Traits
                .Include(t => t.Bonuses)
                .Where(t => t.Bonuses.Any(b => b.BonusId == id))
                .ToList();

            foreach (var trait in traits)
            {
                trait.Bonuses.Remove(bonus);
            }

            db.Bonuses.Remove(bonus);

            db.SaveChanges();
        }
        public void RemoveBonus(int traitId, int bonusId)
        {
            var db = contextFactory;

            var trait = db.Traits
                .Include(t => t.Bonuses)
                .FirstOrDefault(t => t.TraitId == traitId);

            if (trait is null)
            {
                return;
            }

            var bonus = trait.Bonuses
                .FirstOrDefault(b => b.BonusId == bonusId);

            if (bonus is null)
            {
                return;
            }

            trait.Bonuses.Remove(bonus);

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
            return db.Traits.OrderBy(t => t.Name).Include(t => t.Bonuses).Include(t => t.Prerequisites).Include(t => t.EligibleClasses).ToList();
        }
        public List<Trait> ListTraits()
        {
            var db = contextFactory;
            return db.Traits.OrderBy(t => t.Name).ToList();
        }
        public List<Bonus> GetBonuses()
        {
            var db = contextFactory;
            return db.Bonuses.ToList();
        }
        public Trait? GetTraitById(int id)
        {
            var db = contextFactory;
            return db.Traits.Include(t => t.Bonuses).Include(t => t.Prerequisites).Include(t => t.EligibleClasses).FirstOrDefault(t => t.TraitId == id);
        }
        public Bonus? GetBonusById(int id)
        {
            var db = contextFactory;
            return db.Bonuses.Find(id);
        }
        public Trait? GetTraitByName(string name)
        {
            var db = contextFactory;
            return db.Traits.Include(t => t.Bonuses).Include(t => t.Prerequisites).Include(t => t.EligibleClasses).FirstOrDefault(t => t.Name == name);
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

            var traitToUpdate = db.Traits
                .Include(t => t.Bonuses)
                .Include(t => t.Prerequisites)
                .Include(t => t.EligibleClasses)
                .FirstOrDefault(t => t.TraitId == id);

            if (traitToUpdate is null)
            {
                return;
            }

            traitToUpdate.Name = trait.Name;
            traitToUpdate.Source = trait.Source;
            traitToUpdate.Rank = trait.Rank;
            traitToUpdate.Description = trait.Description;

            // Copy prerequisite data BEFORE removing anything.
            var prerequisitesToCopy = trait.Prerequisites
                .Select(p => new Prerequisite
                {
                    Type = p.Type,
                    Value = p.Value,
                    GroupId = p.GroupId
                })
                .ToList();

            var existingPrerequisites = db.Prerequisites
                .Where(p => p.TraitId == id)
                .ToList();

            db.Prerequisites.RemoveRange(existingPrerequisites);

            traitToUpdate.Prerequisites.Clear();

            foreach (var prerequisite in prerequisitesToCopy)
            {
                traitToUpdate.Prerequisites.Add(prerequisite);
            }

            traitToUpdate.EligibleClasses.Clear();

            if (eligibleClassIds is not null && eligibleClassIds.Count > 0)
            {
                var selectedClasses = db.CharacterClasses
                    .Where(c => eligibleClassIds.Contains(c.CharacterClassId))
                    .ToList();

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
            return db.Traits.Where(t => !t.EligibleClasses.Any() || t.EligibleClasses.Any(c => c.CharacterClassId == classId)).OrderBy(t => t.Name).ToList();
        }
        private static void AddIncompatibilityGroup(Trait trait, int groupId)
        {
            var groups = trait.GetIncompatibilityGroups();

            if (!groups.Contains(groupId))
            {
                groups.Add(groupId);
                trait.SetIncompatibilityGroups(groups);
            }
        }
        private static void RemoveIncompatibilityGroup(Trait trait, int groupId)
        {
            var groups = trait.GetIncompatibilityGroups();

            if (groups.Remove(groupId))
            {
                trait.SetIncompatibilityGroups(groups);
            }
        }
        public void AddTraitToIncompatibilityGroup(int traitId, int incompatibilityGroupId)
        {
            var db = contextFactory;

            var trait = db.Traits
                .FirstOrDefault(t => t.TraitId == traitId);

            if (trait is null)
            {
                return;
            }

            if (trait.HasIncompatibilityGroup(incompatibilityGroupId))
            {
                return;
            }

            bool groupExists = db.Traits
                .ToList()
                .Any(t => t.HasIncompatibilityGroup(incompatibilityGroupId));

            if (!groupExists)
            {
                throw new InvalidOperationException(
                    $"Incompatibility group {incompatibilityGroupId} does not exist.");
            }

            AddIncompatibilityGroup(
                trait,
                incompatibilityGroupId);

            db.SaveChanges();
        }
        public void RemoveTraitFromIncompatibilityGroup(int traitId, int incompatibilityGroupId)
        {
            var db = contextFactory;

            var trait = db.Traits.FirstOrDefault(t => t.TraitId == traitId);

            if (trait is null)
            {
                return;
            }

            RemoveIncompatibilityGroup(
                trait,
                incompatibilityGroupId);

            var remainingTraits = db.Traits
                .ToList()
                .Where(t => t.HasIncompatibilityGroup(incompatibilityGroupId))
                .ToList();

            if (remainingTraits.Count < 2)
            {
                foreach (var remainingTrait in remainingTraits)
                {
                    RemoveIncompatibilityGroup(
                        remainingTrait,
                        incompatibilityGroupId);
                }
            }

            db.SaveChanges();
        }
        public void ReplaceTraitIncompatibility(int incompatibilityGroupId, List<int> traitIds)
        {
            traitIds = traitIds.Distinct().ToList();

            if (traitIds.Count < 2)
            {
                throw new InvalidOperationException(
                    "An incompatibility group must contain at least two traits.");
            }

            var db = contextFactory;

            bool groupExists = db.Traits
                .ToList()
                .Any(t => t.HasIncompatibilityGroup(incompatibilityGroupId));

            if (!groupExists)
            {
                throw new InvalidOperationException(
                    $"Incompatibility group {incompatibilityGroupId} does not exist.");
            }

            var traitsInGroup = db.Traits
                .ToList()
                .Where(t => t.HasIncompatibilityGroup(incompatibilityGroupId))
                .ToList();

            foreach (var trait in traitsInGroup)
            {
                RemoveIncompatibilityGroup(
                    trait,
                    incompatibilityGroupId);
            }

            var selectedTraits = db.Traits
                .Where(t => traitIds.Contains(t.TraitId))
                .ToList();

            if (selectedTraits.Count != traitIds.Count)
            {
                throw new InvalidOperationException(
                    "One or more selected traits could not be found.");
            }

            foreach (var trait in selectedTraits)
            {
                AddIncompatibilityGroup(
                    trait,
                    incompatibilityGroupId);
            }

            db.SaveChanges();
        }
        public List<int> GetUsedIncompatibilityGroups()
        {
            var db = contextFactory;

            return db.Traits
                .ToList()
                .SelectMany(t => t.GetIncompatibilityGroups())
                .Distinct()
                .OrderBy(g => g)
                .ToList();
        }
        private int GetNextIncompatibilityGroupId()
        {
            var usedGroups = GetUsedIncompatibilityGroups();

            int candidate = 1;

            while (usedGroups.Contains(candidate))
            {
                candidate++;
            }

            return candidate;
        }
        public void AddTraitIncompatibility(List<int> traitIds)
        {
            traitIds = traitIds.Distinct().ToList();
            if (traitIds.Count < 2)
            {
                throw new InvalidOperationException(
                    "An incompatibility group must contain at least two traits.");
            }

            var db = contextFactory;

            var traits = db.Traits
                .Where(t => traitIds.Contains(t.TraitId))
                .ToList();

            if (traits.Count < 2)
            {
                throw new InvalidOperationException(
                    "At least two valid traits are required.");
            }

            int groupId = GetNextIncompatibilityGroupId();

            foreach (var trait in traits)
            {
                AddIncompatibilityGroup(trait, groupId);
            }

            db.SaveChanges();
        }
        public void RemoveTraitIncompatibility(int incompatibilityGroupId)
        {
            var db = contextFactory;

            var traits = db.Traits.ToList();

            foreach (var trait in traits)
            {
                RemoveIncompatibilityGroup(trait, incompatibilityGroupId);
            }

            db.SaveChanges();
        }
        public List<Trait> GetTraitsInIncompatibilityGroup(int incompatibilityGroupId)
        {
            var db = contextFactory;

            return db.Traits
                .ToList()
                .Where(t => t.HasIncompatibilityGroup(incompatibilityGroupId))
                .OrderBy(t => t.Name)
                .ToList();
        }
    }
}
