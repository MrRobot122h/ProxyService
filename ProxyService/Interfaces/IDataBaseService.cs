using ProxyServer.Models;

namespace ProxyService.Interfaces
{
    public interface IDataBaseService
    {
        public void CreateUser(User user);
        public List<User> GetAllUsers();
        public User GetUserById(int id);
        public void UpdateUser(User user);
        public void DeleteUser(int id);
        public int MaxId();
        public bool UniqueCheck(User user);
    }
}
