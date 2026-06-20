using System.ComponentModel.DataAnnotations;

namespace MythRPG.Core
{
    public class CharacterClass
    {
        public int CharacterClassId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PrimaryBonus { get; set; }
        [Range(0, 100, ErrorMessage = "Must be between 0 and 100.")]
        public int StartingSpells { get; set; }
        [Range(0, 100, ErrorMessage = "Must be between 0 and 100.")]
        public int SpellsPerLevel { get; set; }
        public List<Trait> GrantedTraits { get; set; } = new();
        public List<SpellColour> SpellColours { get; set; } = new();
    }
}