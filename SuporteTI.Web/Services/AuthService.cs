using Newtonsoft.Json;
using SuporteTI.Shared.Models;
using System.Text;

namespace SuporteTI.Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        // ApiClient foi removido, não precisamos mais dele aqui

        // (Use a porta HTTP correta da sua API)
        private const string ApiBaseUrl = "http://localhost:5219";

        // Armazenamento estático (Não guarda mais Token)
        public static string UserRole { get; private set; }
        public static string UserName { get; private set; }
        public static int UserId { get; private set; }

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ApiBaseUrl);
        }

        public async Task<LoginResponse> LoginAsync(string email, string senha)
        {
            var request = new LoginRequest { Email = email, Senha = senha };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Login ou Senha inválidos.");
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            // Armazena os dados do usuário (sem token)
            UserRole = loginResponse.Papel;
            UserName = loginResponse.Nome;
            UserId = loginResponse.Id;

            return loginResponse;
        }

        public void Logout()
        {
            UserRole = null;
            UserName = null;
            UserId = 0;
        }

        public bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(UserRole);
        }
    }
}