using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDesk.Mobile.Services;
using SuporteTI.Shared.Models.Dto; // <-- CORREÇÃO: Usando o Shared da nossa API
using System;
using System.Collections.Generic; // Para a Lista
using System.Collections.ObjectModel; // <-- CORREÇÃO: Mudado de List para ObservableCollection
using System.Threading.Tasks;

namespace HelpDesk.Mobile.ViewModels
{
    public partial class ManagementUsersViewModel : ObservableObject
    {
        private readonly ManagementService _managementService;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string selectedRole; // Usuario, Tecnico ou Gerencia

        [ObservableProperty]
        private bool isBusy;

        // CORREÇÃO: Nomes de papéis alinhados com a API
        public List<string> Roles { get; } = new List<string> { "Usuario", "Tecnico", "Gerencia" };

        public ManagementUsersViewModel(ManagementService managementService)
        {
            _managementService = managementService;
            SelectedRole = "Usuario"; // Define um padrão
        }

        [RelayCommand]
        private async Task RegisterUser()
        {
            if (IsBusy) return;
            IsBusy = true;

            // --- VALIDAÇÕES BÁSICAS ---
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(SelectedRole))
            {
                await Shell.Current.DisplayAlert("Erro", "Preencha todos os campos.", "OK");
                IsBusy = false;
                return;
            }
            // --------------------------

            try
            {
                // CORREÇÃO: Usando o DTO da nossa API (UserCreateDto)
                var userDto = new UserCreateDto
                {
                    Nome = Name,
                    Email = Email,
                    Senha = Password,
                    Papel = SelectedRole // API usa "Papel"
                    // Os campos Setor/Especialidade não estão no XAML
                };

                // CORREÇÃO: Chamada REAL à API (do Batch 127)
                await _managementService.CreateUsuarioAsync(userDto);

                await Shell.Current.DisplayAlert("Sucesso", $"Usuário {Name} ({SelectedRole}) cadastrado com sucesso!", "OK");

                // Limpar campos
                Name = string.Empty;
                Email = string.Empty;
                Password = string.Empty;
                SelectedRole = "Usuario";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Falha ao cadastrar: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
