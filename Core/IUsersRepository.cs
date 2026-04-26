
namespace MythRPG.Core
{
    public interface IUsersRepository
    {
        List<User> GetUsers();
        User? GetUserById(int UserId);
        User? GetUserByUsername(string username);
        User? LogIn(string username, string password);
        void AddUser(User user);
        void UpdateUser(int UserId, User user);
        void UpdateUserCharacters(int UserId, List<Character> characters);
        void DeleteUser(int UserId);
    }
}