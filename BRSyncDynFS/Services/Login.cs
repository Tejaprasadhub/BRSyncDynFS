using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BRSyncDynFS.Interfaces;

namespace BRSyncDynFS.Services
{
    public class Login : ILogin
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public Login(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> PerformLogin() // Renamed method to avoid conflict with class name
        {
            string? BaseUrl = _configuration["AppSettings:BaseUrl"];
            // Implement your login logic here
            var response = await _httpClient.GetAsync(BaseUrl+"/?a=login&userid=1path&password=fire1-truck2-building3");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content; // Return the content as the method signature requires a string
        }
    }
}
