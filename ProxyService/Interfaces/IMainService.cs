using ProxyServer.Models;

namespace ProxyService.Interfaces
{
    public interface IMainService
    {
        public User AddInTable(User root);
        public bool Exists(int id);
        public Task<User> RecordFromSite(int id);
        public User GetById(int id);
        public List<User> GetAll();
        public void Delete(int id);
    }
}
