using Blazorise;
using MythRPG.Core.Repositories;

namespace MythRPG.Core
{
    public class Bonus
    {
        public int BonusId { get; set; }
        public string? Type { get; set; }
        public string? Modifies { get; set; }
        public int Amount { get; set; }
        public override string ToString()
        {
            return "+" + Amount + " " + Type + " bonus to " + Modifies;
        }
    }
}
