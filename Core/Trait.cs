namespace MythRPG.Core
{
    public class Trait
    {
        public int TraitId { get; set; }
        public string? Name { get; set; }
        public string? Source { get; set; }
        public string? Description { get; set; }
        public string? ActionCost { get; set; }
        public string? ResourceCost { get; set; }
        public List<Bonus> Bonuses { get; set; } = new List<Bonus>();

    }
}
