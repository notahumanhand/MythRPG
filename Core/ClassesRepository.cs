using Microsoft.EntityFrameworkCore;
using MythRPG.Data;

namespace MythRPG.Core
{
    public class ClassesRepository : IClassesRepository
    {
        private readonly MythRPGContext contextFactory;

        public ClassesRepository(MythRPGContext contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void AddClass(CharacterClass characterClass)
        {
            var db = this.contextFactory;

            db.CharacterClasses.Add(characterClass);

            db.SaveChanges();
        }

        public List<CharacterClass> GetClasses()
        {
            var db = this.contextFactory;

            return db.CharacterClasses
                .Include(c => c.GrantedTraits)
                .Include(c => c.SpellColours)
                .ToList();
        }

        public List<CharacterClass> ListClasses()
        {
            var db = this.contextFactory;

            return db.CharacterClasses.ToList();
        }

        public CharacterClass GetClassById(int id)
        {
            List<CharacterClass> classes = GetClasses();

            foreach (var characterClass in classes)
            {
                if (characterClass.CharacterClassId == id)
                {
                    return characterClass;
                }
            }

            return new CharacterClass();
        }

        public void UpdateClass(int id, CharacterClass characterClass)
        {
            if (characterClass == null)
            {
                throw new ArgumentNullException(nameof(characterClass));
            }

            if (id != characterClass.CharacterClassId)
            {
                return;
            }

            var db = this.contextFactory;

            var classToUpdate =
                db.CharacterClasses.Find(id);

            if (classToUpdate is not null)
            {
                classToUpdate.Name = characterClass.Name;
                classToUpdate.Description = characterClass.Description;
                classToUpdate.PrimaryBonus = characterClass.PrimaryBonus;
                classToUpdate.StartingSpells = characterClass.StartingSpells;
                classToUpdate.SpellsPerLevel = characterClass.SpellsPerLevel;

                db.SaveChanges();
            }
        }

        public void DeleteClass(int id)
        {
            var db = this.contextFactory;

            var characterClass =
                db.CharacterClasses.Find(id);

            if (characterClass is null)
            {
                return;
            }

            db.CharacterClasses.Remove(characterClass);

            db.SaveChanges();
        }

        public void AddSpellColourToClass(int classId, int spellColourId)
        {
            var db = this.contextFactory;

            var characterClass =
                db.CharacterClasses
                    .Include(c => c.SpellColours)
                    .FirstOrDefault(
                        c => c.CharacterClassId == classId);

            var spellColour =
                db.SpellColours.Find(spellColourId);

            if (characterClass is null ||
                spellColour is null)
            {
                return;
            }

            if (!characterClass.SpellColours
                .Any(c => c.SpellColourId == spellColourId))
            {
                characterClass.SpellColours.Add(spellColour);

                db.SaveChanges();
            }
        }

        public void RemoveSpellColourFromClass(int classId, int spellColourId)
        {
            var db = this.contextFactory;

            var characterClass =
                db.CharacterClasses
                    .Include(c => c.SpellColours)
                    .FirstOrDefault(
                        c => c.CharacterClassId == classId);

            if (characterClass is null)
            {
                return;
            }

            var colour =
                characterClass.SpellColours
                    .FirstOrDefault(
                        c => c.SpellColourId == spellColourId);

            if (colour is not null)
            {
                characterClass.SpellColours.Remove(colour);

                db.SaveChanges();
            }
        }

        public void AddTraitToClass(int classId, int traitId)
        {
            var db = this.contextFactory;

            var characterClass =
                db.CharacterClasses
                    .Include(c => c.GrantedTraits)
                    .FirstOrDefault(
                        c => c.CharacterClassId == classId);

            var trait =
                db.Traits.Find(traitId);

            if (characterClass is null ||
                trait is null)
            {
                return;
            }

            if (!characterClass.GrantedTraits
                .Any(t => t.TraitId == traitId))
            {
                characterClass.GrantedTraits.Add(trait);

                db.SaveChanges();
            }
        }

        public void RemoveTraitFromClass(int classId, int traitId)
        {
            var db = this.contextFactory;

            var characterClass =
                db.CharacterClasses
                    .Include(c => c.GrantedTraits)
                    .FirstOrDefault(
                        c => c.CharacterClassId == classId);

            if (characterClass is null)
            {
                return;
            }

            var trait =
                characterClass.GrantedTraits
                    .FirstOrDefault(
                        t => t.TraitId == traitId);

            if (trait is not null)
            {
                characterClass.GrantedTraits.Remove(trait);

                db.SaveChanges();
            }
        }
    }
}