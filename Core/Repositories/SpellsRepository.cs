using Microsoft.EntityFrameworkCore;
using MythRPG.Core.Interfaces;
using MythRPG.Data;

namespace MythRPG.Core.Repositories
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
            var db = contextFactory;
            if (spell.Colour is not null)
            {
                spell.Colour =
                    db.SpellColours.Find(
                        spell.Colour.SpellColourId);
            }

            db.Spells.Add(spell);
            db.SaveChanges();
        }
        public void AddSpellToCharacter(int charId, Spell spell)
        {
            var db = contextFactory;
            var characterToUpdate = db.Characters.Find(charId);
            if (characterToUpdate is not null)
            {
                characterToUpdate.Spells.Add(spell);
                db.SaveChanges();
            }
        }
        public List<Spell> GetSpells()
        {
            var db = contextFactory;

            return db.Spells.OrderBy(s => s.Name).Include(s => s.Colour).ToList();
        }
        public Spell? GetSpellById(int id)
        {
            var db = contextFactory;

            return db.Spells.Include(s => s.Colour).FirstOrDefault(s => s.SpellId == id);
        }
        public Spell? GetSpellByName(string name)
        {
            var db = contextFactory;

            return db.Spells.Include(s => s.Colour).FirstOrDefault(s => s.Name == name);
        }
        public void UpdateSpell(int id, Spell spell)
        {
            if (spell == null) throw new ArgumentNullException(nameof(spell));
            if (id != spell.SpellId) return;
            var db = contextFactory;
            var spellToUpdate = db.Spells.Find(id);
            if (spellToUpdate is not null)
            {
                spellToUpdate.Name = spell.Name;
                spellToUpdate.Cost = spell.Cost;
                spellToUpdate.Colour = spell.Colour is null ? null : db.SpellColours.Find(spell.Colour.SpellColourId);
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
            var db = contextFactory;
            var spell = db.Spells.Find(id);
            if (spell is null) return;
            db.Spells.Remove(spell);
            db.SaveChanges();
        }
    }
}
