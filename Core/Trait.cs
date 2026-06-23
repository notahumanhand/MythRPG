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
        public string IncompatibilityGroups { get; set; } = string.Empty;
        public List<int> GetIncompatibilityGroups()
        {
            if (string.IsNullOrWhiteSpace(IncompatibilityGroups))
            {
                return new();
            }

            return IncompatibilityGroups
                .Split(':', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }
        public void SetIncompatibilityGroups(List<int> groups)
        {
            IncompatibilityGroups = string.Join(
                ':',
                groups
                    .Distinct()
                    .Where(g => g > 0)
                    .OrderBy(g => g));
        }
        public bool HasIncompatibilityGroup(int groupId)
        {
            return GetIncompatibilityGroups().Contains(groupId);
        }
        public bool IsIncompatibleWith(Trait other)
        {
            return GetIncompatibilityGroups()
                .Intersect(other.GetIncompatibilityGroups())
                .Any();
        }
        public List<string> GetFormattedPrerequisiteGroups()
        {
            return Prerequisites
                .GroupBy(p => p.GroupId)
                .OrderBy(g => g.Key)
                .Select(g => string.Join(" OR ", g.Select(p => p.ToString())))
                .ToList();
        }
        public string GetFormattedPrerequisites()
        {
            return string.Join(
                Environment.NewLine,
                GetFormattedPrerequisiteGroups());
        }
    }
}