using SuporteTI.Shared.Models;
using SuporteTI.Shared.Models.Dto;
using System.Net.Http.Headers;

namespace SuporteTI.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        // CORREÇÃO (Batch 92): A porta está correta
        private const string ApiBaseUrl = "http://localhost:5219";

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ApiBaseUrl);
        }

        public void SetAuthHeader(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // --- Métodos de Gerência ---
        public async Task<DashboardMetricsDto> GetDashboardMetricsAsync()
        {
            return await _httpClient.GetFromJsonAsync<DashboardMetricsDto>("/api/gerencia/dashboard-metrics");
        }

        public async Task<List<UserListDto>> GetUsuariosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserListDto>>("/api/gerencia/usuarios");
        }

        public async Task CreateUsuarioAsync(UserCreateDto novoUsuario)
        {
            await _httpClient.PostAsJsonAsync("/api/gerencia/usuarios", novoUsuario);
        }

        public async Task DeleteUsuarioAsync(int id)
        {
            await _httpClient.DeleteAsync($"/api/gerencia/usuarios/{id}");
        }

        // --- Métodos de Chat ---
        public async Task<ChatResponse> EnviarMensagemAsync(string mensagem, int chamadoId)
        {
            var request = new ChatRequest
            {
                ChamadoId = chamadoId,
                UsuarioId = AuthService.UserId,
                Mensagem = mensagem
            };

            var response = await _httpClient.PostAsJsonAsync("/api/chatbot/mensagem", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ChatResponse>();
        }

        public async Task<ChatResponse> ConfirmarResolucaoAsync(int chamadoId, bool resolveu)
        {
            var request = new ConfirmacaoRequest
            {
                ChamadoId = chamadoId,
                Resolveu = resolveu
            };

            var response = await _httpClient.PostAsJsonAsync("/api/chatbot/confirmar", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ChatResponse>();
        }

        // --- Métodos de Técnico ---
        public async Task<List<TicketListDto>> GetTicketsAbertosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TicketListDto>>("/api/tecnico/tickets-abertos");
        }

        public async Task<List<TicketListDto>> GetTicketsEmAndamentoAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TicketListDto>>($"/api/tecnico/tickets-em-andamento/{AuthService.UserId}");
        }

        public async Task<List<TicketListDto>> GetTicketsResolvidosIAAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TicketListDto>>("/api/tecnico/tickets-resolvidos-ia");
        }

        public async Task<TicketDetailDto> GetTicketDetailAsync(int ticketId)
        {
            return await _httpClient.GetFromJsonAsync<TicketDetailDto>($"/api/tecnico/ticket/{ticketId}");
        }

        public async Task AceitarTicketAsync(int ticketId)
        {
            var response = await _httpClient.PutAsync($"/api/tecnico/aceitar-ticket/{ticketId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task FecharTicketAsync(CloseTicketRequest request)
        {
            await _httpClient.PutAsJsonAsync("/api/tecnico/fechar-ticket", request);
        }

        public async Task UpdateStatusAsync(string novoStatus)
        {
            var request = new StatusUpdateRequest { NovoStatus = novoStatus };
            await _httpClient.PutAsJsonAsync("/api/tecnico/status", request);
        }
    }
}