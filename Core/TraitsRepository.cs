using Microsoft.EntityFrameworkCore;
using MythRPG.Data;

namespace MythRPG.Core
{
    public class TraitsRepository : ITraitsRepository
    {
        private readonly IDbContextFactory<MythRPGContext> contextFactory;
        public TraitsRepository(IDbContextFactory<MythRPGContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public void AddTrait(Trait trait)
        {
            using var db = this.contextFactory.CreateDbContext();
            db.Traits.Add(trait);
            db.SaveChanges();
        }
        public void AddBonus (Bonus bonus)
        {
            using var db = this.contextFactory.CreateDbContext();
            db.Bonuses.Add(bonus);
            db.SaveChanges();
        }
        public void AddTraitToCharacter(int charId, Trait trait)
        {
            using var db = this.contextFactory.CreateDbContext();
            var characterToUpdate = db.Characters.Find(charId);
            if (characterToUpdate is not null)
            {
                characterToUpdate.Traits.Add(trait);
                db.SaveChanges();
            }
        }
        public List<Trait> GetTraits()
        {
            using var db = this.contextFactory.CreateDbContext();
            return db.Traits.Include(e => e.Bonuses).ToList();
        }
        public List<Trait> ListTraits()
        {
            using var db = this.contextFactory.CreateDbContext();
            return db.Traits.ToList();
        }
        public List<Bonus> GetBonuses()
        {
            using var db = this.contextFactory.CreateDbContext();
            return db.Bonuses.ToList();
        }
        public Trait GetTraitById(int id)
        {
            using var db = this.contextFactory.CreateDbContext();
            var trait = db.Traits.Find(id);
            if (trait is not null)  return trait;
            return new Trait();
        }
        public Trait GetTraitBonusById(int id)
        {
            using var db = this.contextFactory.CreateDbContext();
            List<Trait> traits = GetTraits();
            foreach (var trait in traits)
            {
                if (trait.TraitId == id) return trait;
            }
            return new Trait();
        }
        public Bonus? GetBonusById(int id)
        {
            using var db = this.contextFactory.CreateDbContext();
            var bonus = db.Bonuses.Find(id);
            if (bonus is not null) return bonus;
            return null;
        }
        public Trait GetTraitByName(string name)
        {
            using var db = this.contextFactory.CreateDbContext();
            List<Trait> traits = ListTraits();
            foreach (var trait in traits)
            {
                if (trait.Name is not null && trait.Name.Equals(name)) return trait;
            }
            return new Trait();
        }
        public Trait GetTraitBonusByName(string name)
        {
            using var db = this.contextFactory.CreateDbContext();
            List<Trait> traits = GetTraits();
            foreach (var trait in traits)
            {
                if (trait.Name is not null && trait.Name.Equals(name)) return trait;
            }
            return new Trait();
        }
        public void UpdateTrait(int id, Trait trait)
        {
            if (trait == null) throw new ArgumentNullException(nameof(trait));
            if (id != trait.TraitId) return;
            using var db = this.contextFactory.CreateDbContext();
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
        public void DeleteTrait(int id)
        {
            using var db = this.contextFactory.CreateDbContext();
            var trait = db.Traits.Find(id);
            if (trait is null) return;
            db.Traits.Remove(trait);
            db.SaveChanges();
        }
    }
}
