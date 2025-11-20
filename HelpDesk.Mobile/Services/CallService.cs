using SuporteTI.Shared.Models.Dto;
using System.Diagnostics;

namespace HelpDesk.Mobile.Services
{
    public class CallService
    {
        private readonly ApiService _api;

        public CallService(ApiService api)
        {
            _api = api;
        }

        public async Task<ChatResponse> EnviarMensagemAsync(string mensagem, int chamadoId)
        {
            try
            {
                // CRÍTICO: Garante que o ID do usuário logado seja enviado.
                // Se isso for 0 ou nulo, o chamado fica "órfão" e não aparece na lista "Meus Chamados".
                int usuarioId = AuthService.LoggedUserId;

                if (usuarioId == 0)
                {
                    Debug.WriteLine("[CallService] ALERTA: UsuarioId é 0. O usuário não parece estar logado corretamente.");
                }

                var request = new ChatRequest
                {
                    ChamadoId = chamadoId,
                    UsuarioId = usuarioId,
                    Mensagem = mensagem
                };

                return await _api.PostAsync<ChatResponse>("chatbot/mensagem", request);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CallService] Erro ao enviar: {ex.Message}");
                throw;
            }
        }

        public async Task<ChatResponse> ConfirmarResolucaoAsync(int chamadoId, bool resolveu)
        {
            try
            {
                var request = new ConfirmacaoRequest
                {
                    ChamadoId = chamadoId,
                    Resolveu = resolveu
                };

                return await _api.PostAsync<ChatResponse>("chatbot/confirmar", request);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CallService] Erro ao confirmar: {ex.Message}");
                throw;
            }
        }
    }
}