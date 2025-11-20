using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDesk.Mobile.Services;
using HelpDesk.Mobile.Views;
using Microsoft.Maui.Controls;

namespace HelpDesk.Mobile.ViewModels
{
    public partial class ClientDashboardViewModel : ObservableObject
    {
        // Serviço de autenticação para fazer logout
        private readonly AuthService _authService;

        [ObservableProperty]
        private string userName;

        // Injeção de Dependência (DI)
        public ClientDashboardViewModel(AuthService authService)
        {
            _authService = authService;
            // Pega o nome do usuário logado (ou um padrão se for nulo)
            UserName = AuthService.LoggedUserName ?? "Cliente";
        }

        // === NAVEGAÇÃO (Botões do Dashboard) ===

        [RelayCommand]
        private async Task IrParaChatIA()
        {
            // Vai para a tela do Chatbot
            await Shell.Current.GoToAsync(nameof(IAChatPage));
        }

        [RelayCommand]
        private async Task IrParaMeusChamados()
        {
            // Vai para a lista de chamados (MyCallsPage)
            await Shell.Current.GoToAsync(nameof(MyCallsPage));
        }

        [RelayCommand]
        private async Task IrParaNovoChamadoManual()
        {
            // Vai para a tela de abertura manual (NewCallPage)
            await Shell.Current.GoToAsync(nameof(NewCallPage));
        }

        // === LOGOUT (Sair da Conta) ===

        [RelayCommand]
        private async Task Logout()
        {
            // Pergunta se quer sair mesmo
            bool confirm = await Shell.Current.DisplayAlert("Sair", "Deseja realmente sair da conta?", "Sim", "Não");

            if (confirm)
            {
                // Limpa os dados do usuário no serviço
                _authService.Logout();

                // Volta para a tela de login
                // O "///" reseta a pilha de navegação, impedindo o botão "Voltar" do Android de retornar ao Dashboard
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}