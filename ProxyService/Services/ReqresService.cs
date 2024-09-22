using ProxyServer.Models;
using ProxyService.Interfaces;
using ProxyService.Models;
using System.Text.Json;

namespace ProxyService.Services
{
    public class ReqresService : IReqresService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<User> LoadUser(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://reqres.in/api/users/{id}");
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Conect error!");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var userResponse = JsonSerializer.Deserialize<ReqresDataResponse>(responseContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });



            return userResponse.Data;
        }
    }
}
