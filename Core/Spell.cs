using System.ComponentModel.DataAnnotations;

namespace MythRPG.Core
{
    public class Spell
    {
        public int SpellId { get; set; }
        public string Name { get; set; } = String.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Cost must be greater than 0.")]
        public int Cost { get; set; }
        public string? Colour { get; set; }
        public string? Type { get; set; }
        public string? Casting { get; set; }
        public string? Duration { get; set; }
        public string? Range { get; set; }
        public bool Concentration { get; set; }
        public string? Effect { get; set; }
    }
}
