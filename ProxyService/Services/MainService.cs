using Google.Protobuf.WellKnownTypes;
using ProxyServer.Models;
using ProxyService.Interfaces;

namespace ProxyService.Services
{
    public class MainService : IMainService
    {
        DataBaseService _dataBaseService;
        ReqresService _reqresService;
        public MainService()
        {
            _dataBaseService = new DataBaseService();
            _reqresService = new ReqresService();
        }

        public User AddInTable(User user)
        {
            _dataBaseService.CreateUser(user);
            return user;
        }

        public void Delete(int id)
        {
            _dataBaseService.DeleteUser(id);
        }

        public bool Exists(int id)
        { 
            var root = _dataBaseService.GetUserById(id);
            return root != null;
        }

        public List<User> GetAll()
        {
            return _dataBaseService.GetAllUsers();
        }

        public User GetById(int id)
        {
            return _dataBaseService.GetUserById(id);
        }

        public async Task<User> RecordFromSite(int id)
        {
            User user = await _reqresService.LoadUser(id);

            var @bool = _dataBaseService.UniqueCheck(user);
            if (!@bool) throw new Exception("that Email already in use");

            if (Exists(id))
            {
                int newId = _dataBaseService.MaxId();
                user.Id = newId + 1;
            }
            AddInTable(user);

            return user;
        }
    }
}
