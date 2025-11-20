using Newtonsoft.Json;
using SuporteTI.Shared.Models;
using SuporteTI.Shared.Models.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuporteTI.AdminDesktop.Services
{
    public class ApiClient
    {
        private const string ApiBaseUrl = "http://localhost:5219";

        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new System.Uri(ApiBaseUrl)
        };

        public static LoginResponse CurrentUser { get; private set; }

        public async Task<LoginResponse> LoginAsync(string email, string senha)
        {
            var request = new LoginRequest { Email = email, Senha = senha };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/login", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);

            if (loginResponse.Papel != "Gerencia")
            {
                throw new System.Exception("Acesso negado. Este app é apenas para Gerência.");
            }

            CurrentUser = loginResponse;

            

            return loginResponse;
        }

        // --- MÉTODOS DE ADMIN (CRUD) ---

        public async Task<List<UserListDto>> GetUsuariosAsync()
        {
            var response = await _httpClient.GetAsync("/api/gerencia/usuarios");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<UserListDto>>(json);
        }

        public async Task CreateUsuarioAsync(UserCreateDto novoUsuario)
        {
            var content = new StringContent(JsonConvert.SerializeObject(novoUsuario), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/gerencia/usuarios", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUsuarioAsync(UserListDto usuario)
        {
            // Simula o Update (Deleta e Cria)
            await DeleteUsuarioAsync(usuario.Id);

            var novoUsuario = new UserCreateDto
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = "123456",
                Papel = usuario.Papel,
                Setor = usuario.Setor,
                Especialidade = usuario.Especialidade,
                Telefone = ""
            };
            await CreateUsuarioAsync(novoUsuario);
        }

        // Método Delete (APENAS UMA VEZ)
        public async Task DeleteUsuarioAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/gerencia/usuarios/{id}");
            response.EnsureSuccessStatusCode();
        }
    
        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            var response = await _httpClient.GetAsync("/api/gerencia/categorias");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Categoria>>(json);
        }

        public async Task CreateCategoriaAsync(string nome)
        {
            var categoria = new Categoria { Nome = nome };
            var content = new StringContent(JsonConvert.SerializeObject(categoria), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/gerencia/categorias", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoriaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/gerencia/categorias/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

}