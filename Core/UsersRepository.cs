using MythRPG.Data;

namespace MythRPG.Core
{
    public class UsersRepository : IUsersRepository
    {
        private readonly MythRPGContext context;

        public UsersRepository(MythRPGContext context)
        {
            this.context = context;
        }

        public List<User> GetUsers()
        {
            var users = context.Users.ToList();
            var characters = context.Characters.ToList();

            foreach (var user in users)
            {
                user.Characters = characters
                    .Where(c => c.UserId == user.Id)
                    .ToList();
            }

            return users;
        }

        public User? GetUserById(string id)
        {
            var user =
                context.Users.FirstOrDefault(
                    u => u.Id == id);

            if (user != null)
            {
                user.Characters = context.Characters
                    .Where(c => c.UserId == id)
                    .ToList();
            }

            return user;
        }
    }
}