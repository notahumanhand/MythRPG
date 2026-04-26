using Microsoft.EntityFrameworkCore;
using MythRPG.Data;

namespace MythRPG.Core
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbContextFactory<MythRPGContext> contextFactory;
        public UsersRepository(IDbContextFactory<MythRPGContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public List<User> GetUsers()
        {
            using var db = this.contextFactory.CreateDbContext();
            return db.Users.Include(e => e.Characters).ToList();
        }
        public User? GetUserById(int UserId)
        {
            List<User> users = GetUsers();
            foreach (var user in users)
            {
                if (user.UserId == UserId) return user;
            }
            return null;
        }
        public User? GetUserByUsername(string username)
        {
            List<User> users = GetUsers();
            foreach (var user in users)
            {
                if (user.Username.Equals(username)) return user;
            }
            return null;
        }
        public User? LogIn(string username, string password)
        {
            User? user = GetUserByUsername(username);
            if (user is null) return null;
            if (user.Password.Equals(password)) return user;
            return null;
        }
        public void AddUser(User user)
        {
            using var db = this.contextFactory.CreateDbContext();
            db.Users.Add(user);
            db.SaveChanges();
        }
        public void UpdateUser(int UserId, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            using var db = this.contextFactory.CreateDbContext();
            var userToUopdate = db.Users.Find(UserId);
            if (userToUopdate is not null)
            {
                userToUopdate.Username = user.Username;
                userToUopdate.Password = user.Password;
                db.SaveChanges();
            }
        }
        public void UpdateUserCharacters(int UserId, List<Character> characters)
        {
            if (characters == null) throw new ArgumentNullException(nameof(characters));
            using var db = this.contextFactory.CreateDbContext();
            var userToUopdate = db.Users.Find(UserId);
            if (userToUopdate is not null)
            {
                userToUopdate.Characters = characters;
                db.SaveChanges();
            }
        }
        public void DeleteUser(int UserId)
        {
            using var db = this.contextFactory.CreateDbContext();
            var user = db.Users.Find(UserId);
            if (user is null) return;
            db.Users.Remove(user);
            db.SaveChanges();
        }
    }
}
