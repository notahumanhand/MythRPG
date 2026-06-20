using MythRPG.Data;

namespace MythRPG.Core
{
    public class SpellColoursRepository : ISpellColoursRepository
    {
        private readonly MythRPGContext contextFactory;

        public SpellColoursRepository(
            MythRPGContext contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public List<SpellColour> GetSpellColours()
        {
            var db = this.contextFactory;

            return db.SpellColours.ToList();
        }

        public SpellColour? GetSpellColourById(int id)
        {
            var db = this.contextFactory;

            return db.SpellColours.Find(id);
        }

        public SpellColour? GetSpellColourByName(string name)
        {
            var db = this.contextFactory;

            return db.SpellColours
                .FirstOrDefault(
                    c => c.Name != null &&
                         c.Name.Equals(name));
        }
    }
}