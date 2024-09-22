using Microsoft.AspNetCore.Mvc;
using ProxyServer.Models;
using ProxyService.Interfaces;
using Serilog;

namespace ProxyService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private IMainService _service;
        public AuthenticationController(IMainService service)
        {
            _service = service;
        }

        [HttpGet("from reqres.in")]
        public async Task<IActionResult> GetUserFromSite(int reqresId)
        {
            var obj = _service.RecordFromSite(reqresId);
            return Ok(obj);
        }

        [HttpGet("Get user by id")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var obj = _service.GetById(id);
            return Ok(obj);
        }

        [HttpGet("Get all users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var obj = _service.GetAll();
            return Ok(obj);
        }

        [HttpPost("Set user")]
        public async Task<IActionResult> SetUser(int NewId, string email, string first_name,
            string last_name, string avatar)
        {
            User user = new User
            {
                Id = NewId,
                Email = email,
                FirstName = first_name,
                LastName = last_name,
                Avatar = avatar
            };

            var obj = _service.AddInTable(user);

            return Ok();
        }

        [HttpDelete("Delete user by id")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var obj = _service.Delete(id);
            return Ok();
        }

    }
}
