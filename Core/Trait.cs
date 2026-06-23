namespace MythRPG.Core
{
    public class Trait
    {
        public int TraitId { get; set; }
        public required string Name { get; set; }
        public string? Source { get; set; }
        public string? Description { get; set; }
        public required int Rank { get; set; } = 0;
        public List<Bonus> Bonuses { get; set; } = new();
        public List<CharacterClass> EligibleClasses { get; set; } = new();
        public List<Prerequisite> Prerequisites { get; set; } = new();
    }
}