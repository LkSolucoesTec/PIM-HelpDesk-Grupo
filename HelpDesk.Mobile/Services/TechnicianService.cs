using SuporteTI.Shared.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDesk.Mobile.Services
{
    public class TechnicianService
    {
        private readonly ApiService _api;

        public TechnicianService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<TicketListDto>> GetTicketsAbertosAsync()
        {
            return await _api.GetAsync<List<TicketListDto>>("tecnico/tickets-abertos");
        }

        public async Task AceitarTicketAsync(int ticketId)
        {
            // CORREÇÃO: Envia o ID do técnico na URL (Query String)
            int tecnicoId = AuthService.LoggedUserId;
            await _api.PutAsync($"tecnico/aceitar-ticket/{ticketId}?tecnicoId={tecnicoId}", null);
        }

        public async Task UpdateStatusAsync(string novoStatus)
        {
            // CORREÇÃO: Envia o ID do técnico no corpo do JSON
            var request = new StatusUpdateRequest
            {
                NovoStatus = novoStatus,
                TecnicoId = AuthService.LoggedUserId
            };

            await _api.PutAsync("tecnico/status", request);
        }

        // Outros métodos mantidos...
        public async Task<List<TicketListDto>> GetTicketsEmAndamentoAsync()
        {
            var tecnicoId = AuthService.LoggedUserId;
            return await _api.GetAsync<List<TicketListDto>>($"tecnico/tickets-em-andamento/{tecnicoId}");
        }
    }
}