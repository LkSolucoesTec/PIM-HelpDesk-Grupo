using SuporteTI.Shared.Models.Dto; // Nossos DTOs do SuporteTI.Shared
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDesk.Mobile.Services
{
    // CORREÇÃO 1: "internal" -> "public"
    public class ManagementService
    {
        // CORREÇÃO 2: Injeta o ApiService
        private readonly ApiService _api;

        public ManagementService(ApiService api)
        {
            _api = api;
        }

        // CORREÇÃO 3: Adiciona os métodos que chamam a API

        // Endpoint da NOSSA API: "api/gerencia/dashboard-metrics"
        public async Task<DashboardMetricsDto> GetDashboardMetricsAsync()
        {
            return await _api.GetAsync<DashboardMetricsDto>("gerencia/dashboard-metrics");
        }

        // Endpoint da NOSSA API: "api/gerencia/usuarios"
        public async Task<List<UserListDto>> GetUsuariosAsync()
        {
            return await _api.GetAsync<List<UserListDto>>("gerencia/usuarios");
        }

        // Endpoint da NOSSA API: "api/gerencia/usuarios" (POST)
        public async Task CreateUsuarioAsync(UserCreateDto novoUsuario)
        {
            // O XAML da ManagementUsersPage espera um cadastro
            await _api.PostAsync<UserCreateDto>("gerencia/usuarios", novoUsuario);
        }
    }
}
