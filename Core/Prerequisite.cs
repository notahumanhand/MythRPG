namespace MythRPG.Core
{
    public class Prerequisite
    {
        public int PrerequisiteId { get; set; }
        public int TraitId { get; set; }
        public PrerequisiteType Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public override string ToString()
        {
            return Type switch
            {
                PrerequisiteType.Trait => $"{Value} (trait)",

                PrerequisiteType.TraitTag => $"At least one {Value} trait",

                PrerequisiteType.Species => $"At least one species trait from {Value}",

                PrerequisiteType.MythicPath => $"{Value} mythic path taken",

                PrerequisiteType.Spell => $"{Value} known",

                PrerequisiteType.Level => $"Level {Value}",

                PrerequisiteType.MixedLineage => "Mixed lineage",

                PrerequisiteType.Narrative => $"{Value} (narrative: requires manual validation)",

                _ => Value
            };
        }
    }
}