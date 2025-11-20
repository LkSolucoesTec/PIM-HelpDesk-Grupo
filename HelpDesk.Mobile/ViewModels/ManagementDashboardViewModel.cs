using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDesk.Mobile.Services;
using SuporteTI.Shared.Models.Dto;
using System.Collections.ObjectModel;
using System.Diagnostics;
using HelpDesk.Mobile.Views;

namespace HelpDesk.Mobile.ViewModels
{
    public partial class ManagementDashboardViewModel : ObservableObject
    {
        private readonly ManagementService _managementService;
        private readonly AuthService _authService;

        [ObservableProperty]
        private DashboardMetricsDto dashboardStats;

        [ObservableProperty]
        private ObservableCollection<TecnicoStatusDto> technicianStats;

        public ManagementDashboardViewModel(ManagementService managementService, AuthService authService)
        {
            _managementService = managementService;
            _authService = authService;

            TechnicianStats = new ObservableCollection<TecnicoStatusDto>();
            DashboardStats = new DashboardMetricsDto();

            Task.Run(async () => await LoadDashboardData());
        }

        [RelayCommand]
        private async Task LoadDashboardData()
        {
            try
            {
                var apiMetrics = await _managementService.GetDashboardMetricsAsync();

                if (apiMetrics != null)
                {
                    DashboardStats = apiMetrics;

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        TechnicianStats.Clear();
                        foreach (var tec in apiMetrics.StatusTecnicos)
                            TechnicianStats.Add(tec);
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro Dashboard Gerencia: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task IrParaUsuarios()
        {
            await Shell.Current.GoToAsync(nameof(ManagementUsersPage));
        }

        [RelayCommand]
        private async Task Logout()
        {
            bool answer = await Shell.Current.DisplayAlert("Sair", "Deseja sair da conta?", "Sim", "Não");
            if (answer)
            {
                _authService.Logout();
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}