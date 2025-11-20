using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDesk.Mobile.Services;
using HelpDesk.Mobile.Views;   // (Para os nomes das páginas)
using System;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Mobile.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isBusy;

        private readonly AuthService _authService;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            // Credenciais de teste
            Email = "user";
            Password = "usuario123";
        }

        [RelayCommand]
        private async Task Login()
        {
            if (IsBusy) return;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Erro", "Preencha todos os campos.", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var response = await _authService.LoginAsync(Email, Password);

                if (response != null)
                {
                    // --- CORREÇÃO (Batch 128) ---
                    // Adaptado para a NOSSA API (SuporteTI.Api):
                    // .Role -> .Papel
                    // "Cliente" -> "Usuario"

                    if (response.Papel == "Usuario")
                    {
                        // Vai para o Dashboard do Cliente/Usuário
                        await Shell.Current.GoToAsync($"//{nameof(ClientDashboardPage)}");
                    }
                    else if (response.Papel == "Tecnico")
                    {
                        await Shell.Current.GoToAsync($"//{nameof(TechnicianDashboardPage)}");
                    }
                    else if (response.Papel == "Gerencia")
                    {
                        await Shell.Current.GoToAsync($"//{nameof(ManagementDashboardPage)}");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Erro", $"Papel '{response.Papel}' não reconhecido.", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Falha", "Login ou senha incorretos.", "OK");
                }
            }
            catch (Exception ex)
            {
                // CORREÇÃO: Bloco catch/finally completo
                await Shell.Current.DisplayAlert("Erro", $"Falha na conexão: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
