using MythRPG.Core;

namespace MythRPG.Core
{
    public interface IUsersRepository
    {
        List<User> GetUsers();
        User? GetUserById(string id);
    }
}