namespace MythRPG.Core.Interfaces
{
    public interface IUsersRepository
    {
        List<User> GetUsers();
        User? GetUserById(string id);
    }
}