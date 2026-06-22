using Microsoft.EntityFrameworkCore;
using MythRPG.Core.Interfaces;
using MythRPG.Data;

namespace MythRPG.Core.Repositories
{
    public class CharactersRepository : ICharactersRepository
    {
        private readonly MythRPGContext contextFactory;
        public CharactersRepository(MythRPGContext contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void AddCharacter(Character character)
        {
            var db = contextFactory;
            db.Characters.Add(character);
            db.SaveChanges();
        }
        public List<Character> GetCharacters()
        {
            var db = contextFactory;
            return db.Characters.Include(c => c.CharacterClass).Include(e => e.Traits).Include(c => c.Spells).ToList();
        }
        public List<Character> GetPublicCharacters()
        {
            var db = contextFactory;

            return db.Characters
                .Where(c => c.IsPublic)
                .Include(c => c.CharacterClass)
                .Include(c => c.Traits)
                .Include(c => c.Spells)
                .ToList();
        }
        public List<Character> ListCharacters()
        {
            var db = contextFactory;
            return db.Characters.ToList();
        }
        public Character? GetCharacterById(int id)
        {
            List<Character> characters = GetCharacters();
            foreach (var character in characters)
            {
                if (character.CharacterId == id) return character;
            }
            return null;
        }
        public void DeleteCharacter(int id)
        {
            var db = contextFactory;
            var character = db.Characters.Find(id);
            if (character is null) return;
            db.Characters.Remove(character);
            db.SaveChanges();
        }
        public List<Character> GetCharactersByUserId(string userId)
        {
            var db = contextFactory;
            return db.Characters
                .Where(c => c.UserId == userId)
                .Include(c => c.CharacterClass)
                .Include(c => c.Traits)
                .Include(c => c.Spells)
                .ToList();
        }
        public List<Character> SearchCharacters(string name)
        {
            var db = contextFactory;
            return db.Characters.ToList();
        }
        public List<Character> GetCharactersByClassId(int classId)
        {
            var db = contextFactory;

            return db.Characters
                .Include(c => c.CharacterClass)
                .Include(c => c.Traits)
                .Include(c => c.Spells)
                .Where(c =>
                    c.CharacterClass.CharacterClassId == classId)
                .ToList();
        }
        public void UpdateCharacter(int id, Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));
            if (id != character.CharacterId) return;
            var db = contextFactory;
            var characterToUpdate = db.Characters.Find(id);
            if (characterToUpdate is not null)
            {
                characterToUpdate.IsPublic = character.IsPublic;
                characterToUpdate.CharacterClass = character.CharacterClass;
                characterToUpdate.CharacterLevel = character.CharacterLevel;
                characterToUpdate.CharacterName = character.CharacterName;
                characterToUpdate.MythicPath = character.MythicPath;
                characterToUpdate.Might = character.Might;
                characterToUpdate.Agility = character.Agility;
                characterToUpdate.Knowledge = character.Knowledge;
                characterToUpdate.Charisma = character.Charisma;
                characterToUpdate.Species = character.Species;
                characterToUpdate.PrimaryTrait = character.PrimaryTrait;
                characterToUpdate.SecondaryTrait = character.SecondaryTrait;
                characterToUpdate.TertiaryTrait = character.TertiaryTrait;
                characterToUpdate.Size = character.Size;
                characterToUpdate.Armour = character.Armour;
                characterToUpdate.Shield = character.Shield;
                characterToUpdate.PortraitUrl = character.PortraitUrl;
                db.SaveChanges();
            }
        }
        public void RemoveTrait(int CharId, int TraitId)
        {
            var db = contextFactory;
            var characterToUpdate = db.Characters.Find(CharId);
            var traitToRemove = db.Traits.Find(TraitId);
            if (characterToUpdate is not null && traitToRemove is not null)
            {
                characterToUpdate.Traits.Remove(traitToRemove);
                db.SaveChanges();
            }
        }
        public void RemoveSpell(int CharId, int SpellId)
        {
            var db = contextFactory;
            var characterToUpdate = db.Characters.Find(CharId);
            var spellToRemove = db.Spells.Find(SpellId);
            if (characterToUpdate is not null && spellToRemove is not null)
            {
                characterToUpdate.Spells.Remove(spellToRemove);
                db.SaveChanges();
            }
        }
        public void LevelUpCharacter(int id)
        {
            var db = contextFactory;
            var character = db.Characters.Find(id);
            if (character is not null && character.CharacterLevel < 20) character.CharacterLevel += 1;
        }
    }
}
