using Microsoft.EntityFrameworkCore;
using MythRPG.Data;

namespace MythRPG.Core
{
    public class CharactersRepository : ICharactersRepository
    {
        private readonly IDbContextFactory<MythRPGContext> contextFactory;
        public CharactersRepository(IDbContextFactory<MythRPGContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void AddCharacter(Character character)
        {
            using var db = this.contextFactory.CreateDbContext();
            db.Characters.Add(character);
            db.SaveChanges();
        }
        public List<Character> GetCharacters()
        {
            using var db = this.contextFactory.CreateDbContext();
            return db.Characters.Include(e => e.Traits).ToList();
        }
        public List<Character> ListCharacters()
        {
            using var db = this.contextFactory.CreateDbContext();
            return db.Characters.ToList();
        }
        public Character GetCharacterById(int id)
        {
            List<Character> characters = GetCharacters();
            foreach (var character in characters)
            {
                if (character.CharacterId == id) return character;
            }
            return new Character();
        }
        public void DeleteCharacter(int id)
        {
            using var db = this.contextFactory.CreateDbContext();
            var character = db.Characters.Find(id);
            if (character is null) return;
            db.Characters.Remove(character);
            db.SaveChanges();
        }
        public List<Character> SearchCharacters(string name)
        {
            using var db = this.contextFactory.CreateDbContext();
            return db.Characters.ToList();
        }
        public List<Character> GetCharactersByClass(string charclass)
        {
            using var db = this.contextFactory.CreateDbContext();
            List<Character> characters = GetCharacters();
            List<Character> classcharacters = new List<Character>();
            foreach (var character in characters)
            {
                if (character.CharacterClass == charclass) classcharacters.Add(character);
            }
            return classcharacters;
        }
        public void UpdateCharacter(int id, Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));
            if (id != character.CharacterId) return;
            using var db = this.contextFactory.CreateDbContext();
            var characterToUpdate = db.Characters.Find(id);
            if (characterToUpdate is not null)
            {
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
                //characterToUpdate.Traits = character.Traits;
                db.SaveChanges();
            }
        }
        public void RemoveTrait(int CharId, int TraitId)
        {
            using var db = this.contextFactory.CreateDbContext();
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
            using var db = this.contextFactory.CreateDbContext();
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
            using var db = this.contextFactory.CreateDbContext();
            var character = db.Characters.Find(id);
            if (character is not null && character.CharacterLevel < 20) character.CharacterLevel += 1;
        }
    }
}
