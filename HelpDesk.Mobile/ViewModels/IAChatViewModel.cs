using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDesk.Mobile.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HelpDesk.Mobile.ViewModels
{
    public class ChatHistoryItem
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public bool IsUser => Sender == "Usuario" || Sender == "User";
        public string AvatarImage => IsUser ? "user_avatar.png" : "iasistente.png";
    }

    public partial class IAChatViewModel : ObservableObject
    {
        private readonly CallService _callService;
        private int _chamadoId = 0;

        [ObservableProperty]
        private ObservableCollection<ChatHistoryItem> chatHistory;

        [ObservableProperty]
        private string currentMessage;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool mostrarBotoesConfirmacao;

        public IAChatViewModel(CallService callService)
        {
            _callService = callService;
            ChatHistory = new ObservableCollection<ChatHistoryItem>();

            // Tenta carregar um chamado existente (se houver lógica para isso)
            // Por enquanto, inicia um novo fluxo
            AdicionarMensagemBot("Olá! Sou o assistente virtual. Em que posso ajudar?");
        }

        [RelayCommand]
        private async Task SendMessage()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(CurrentMessage)) return;

            var texto = CurrentMessage;
            CurrentMessage = "";
            IsBusy = true;
            MostrarBotoesConfirmacao = false;

            AdicionarMensagemUsuario(texto);

            try
            {
                // AQUI ESTÁ O SEGREDO:
                // O _chamadoId começa como 0.
                // Na primeira resposta da API, ela devolve o ID do chamado (se criado ou encontrado).
                // Nós salvamos esse ID em _chamadoId.
                // Nas próximas mensagens, enviamos esse _chamadoId, garantindo que tudo fique no mesmo histórico.

                var resposta = await _callService.EnviarMensagemAsync(texto, _chamadoId);

                // ATUALIZA O ID DO CHAMADO COM O QUE A API DEVOLVEU
                if (resposta.ChamadoId != 0)
                {
                    _chamadoId = resposta.ChamadoId;
                }

                AdicionarMensagemBot(resposta.RespostaBot);
                MostrarBotoesConfirmacao = resposta.PedirConfirmacao;

                if (resposta.ChamadoAberto)
                {
                    await Shell.Current.DisplayAlert("Aviso", "Chamado técnico aberto. Número: " + _chamadoId, "OK");
                    // Opcional: Navegar para a tela de detalhes do chamado
                }
            }
            catch (Exception ex)
            {
                AdicionarMensagemBot($"Erro: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ConfirmarResolucao(bool resolveu)
        {
            IsBusy = true;
            MostrarBotoesConfirmacao = false;
            try
            {
                var resposta = await _callService.ConfirmarResolucaoAsync(_chamadoId, resolveu);
                AdicionarMensagemBot(resposta.RespostaBot);
            }
            catch (Exception ex)
            {
                AdicionarMensagemBot($"Erro: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AdicionarMensagemBot(string msg)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                ChatHistory.Add(new ChatHistoryItem { Sender = "Bot", Message = msg });
            });
        }

        private void AdicionarMensagemUsuario(string msg)
        {
            MainThread.BeginInvokeOnMainThread(() => {
                ChatHistory.Add(new ChatHistoryItem { Sender = "User", Message = msg });
            });
        }
    }
}