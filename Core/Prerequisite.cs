namespace MythRPG.Core
{
    public class Prerequisite
    {
        public int PrerequisiteId { get; set; }
        public int TraitId { get; set; }
        public PrerequisiteType Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}