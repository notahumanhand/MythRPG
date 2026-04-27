using Microsoft.EntityFrameworkCore;
using MythRPG.Data;

namespace MythRPG.Core
{
    public class SpellsRepository : ISpellsRepository
    {
        private readonly MythRPGContext contextFactory;
        public SpellsRepository(MythRPGContext contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public void AddSpell(Spell spell)
        {
            var db = this.contextFactory;
            db.Spells.Add(spell);
            db.SaveChanges();
        }
        public void AddSpellToCharacter(int charId, Spell spell)
        {
            var db = this.contextFactory;
            var characterToUpdate = db.Characters.Find(charId);
            if (characterToUpdate is not null)
            {
                characterToUpdate.Spells.Add(spell);
                db.SaveChanges();
            }
        }
        public List<Spell> GetSpells()
        {
            var db = this.contextFactory;
            return db.Spells.ToList();
        }
        public Spell GetSpellById(int id)
        {
            var db = this.contextFactory;
            var spell = db.Spells.Find(id);
            if (spell is not null) return spell;
            return new Spell();
        }
        public Spell? GetSpellByName(string name)
        {
            var db = this.contextFactory;
            List<Spell> spells = GetSpells();
            foreach (var spell in spells)
            {
                if (spell.Name.Equals(name)) return spell;
            }
            return new Spell();
        }
        public void UpdateSpell(int id, Spell spell)
        {
            if (spell == null) throw new ArgumentNullException(nameof(spell));
            if (id != spell.SpellId) return;
            var db = this.contextFactory;
            var spellToUpdate = db.Spells.Find(id);
            if (spellToUpdate is not null)
            {
                spellToUpdate.Name = spell.Name;
                spellToUpdate.Cost = spell.Cost;
                spellToUpdate.Colour = spell.Colour;
                spellToUpdate.Type = spell.Type;
                spellToUpdate.Casting = spell.Casting;
                spellToUpdate.Duration = spell.Duration;
                spellToUpdate.Range = spell.Range;
                spellToUpdate.Concentration = spell.Concentration;
                spellToUpdate.Effect = spell.Effect;
                db.SaveChanges();
            }
        }
        public void DeleteSpell(int id)
        {
            var db = this.contextFactory;
            var spell = db.Spells.Find(id);
            if (spell is null) return;
            db.Spells.Remove(spell);
            db.SaveChanges();
        }
    }
}
