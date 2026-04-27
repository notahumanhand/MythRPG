using Microsoft.AspNetCore.Identity;

namespace MythRPG.Core
{
    public class User : IdentityUser
    {
        public List<Character> Characters { get; set; } = new List<Character>();
    }
}
