using SuporteTI.Shared.Models;
using SuporteTI.Web.Services;

namespace SuporteTI.Web.ViewModels
{
    // Classe simples para representar um balão de chat
    public class ChatMessage
    {
        public string Remetente { get; set; } // "Bot" ou "Usuario"
        public string Mensagem { get; set; }
        public bool IsUsuario => Remetente == "Usuario";
    }

    // O cérebro que gerencia a tela de chat
    public class ChatPageViewModel
    {
        private readonly ApiClient _apiClient;

        public List<ChatMessage> Mensagens { get; private set; } = new List<ChatMessage>();
        public string MensagemAtual { get; set; } = "";
        public bool IsEnviando { get; private set; } = false;
        public bool MostrarBotoesConfirmacao { get; private set; } = false;
        private int _chamadoId = 0;

        // Evento para avisar o Blazor que a tela precisa ser redesenhada
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public ChatPageViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
            AdicionarMensagemBot("Olá! Eu sou o assistente virtual. Por favor, descreva seu problema.");
        }

        public async Task EnviarMensagem()
        {
            if (string.IsNullOrWhiteSpace(MensagemAtual) || IsEnviando)
                return;

            var msgParaEnviar = MensagemAtual;
            MensagemAtual = "";
            IsEnviando = true;
            MostrarBotoesConfirmacao = false;

            AdicionarMensagemUsuario(msgParaEnviar);
            NotifyStateChanged();

            try
            {
                var resposta = await _apiClient.EnviarMensagemAsync(msgParaEnviar, _chamadoId);
                _chamadoId = resposta.ChamadoId; // Salva o ID do chamado

                AdicionarMensagemBot(resposta.RespostaBot);
                MostrarBotoesConfirmacao = resposta.PedirConfirmacao;
            }
            catch (Exception ex)
            {
                AdicionarMensagemBot($"Erro ao enviar: {ex.Message}");
            }
            finally
            {
                IsEnviando = false;
                NotifyStateChanged();
            }
        }

        public async Task ConfirmarResolucao(bool resolveu)
        {
            IsEnviando = true;
            MostrarBotoesConfirmacao = false;
            NotifyStateChanged();

            try
            {
                var resposta = await _apiClient.ConfirmarResolucaoAsync(_chamadoId, resolveu);
                AdicionarMensagemBot(resposta.RespostaBot);
            }
            catch (Exception ex)
            {
                AdicionarMensagemBot($"Erro ao confirmar: {ex.Message}");
            }
            finally
            {
                IsEnviando = false;
                NotifyStateChanged();
            }
        }

        private void AdicionarMensagemBot(string msg)
        {
            Mensagens.Add(new ChatMessage { Remetente = "Bot", Mensagem = msg });
        }

        private void AdicionarMensagemUsuario(string msg)
        {
            Mensagens.Add(new ChatMessage { Remetente = "Usuario", Mensagem = msg });
        }
    }
}
