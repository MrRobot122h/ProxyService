using ProxyServer.Models;

namespace ProxyService.Interfaces
{
    public interface IReqresService
    {
        public Task<User> LoadUser(int id);
    }
}
