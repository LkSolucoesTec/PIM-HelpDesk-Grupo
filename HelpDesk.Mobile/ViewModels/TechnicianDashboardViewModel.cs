using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDesk.Mobile.Services;
using HelpDesk.Mobile.Views;
using SuporteTI.Shared.Models.Dto;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HelpDesk.Mobile.ViewModels
{
    public partial class TechnicianDashboardViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        private readonly TechnicianService _technicianService;
        private readonly ManagementService _managementService;

        // Status atual do técnico na tela
        [ObservableProperty]
        private string technicianStatus = "Online";

        [ObservableProperty]
        private ObservableCollection<TicketListDto> availableCalls;

        [ObservableProperty]
        private ObservableCollection<TecnicoStatusDto> onlineTechnicians;

        public TechnicianDashboardViewModel(AuthService authService, TechnicianService technicianService, ManagementService managementService)
        {
            _authService = authService;
            _technicianService = technicianService;
            _managementService = managementService;

            AvailableCalls = new ObservableCollection<TicketListDto>();
            OnlineTechnicians = new ObservableCollection<TecnicoStatusDto>();

            // Carrega os dados iniciais
            Task.Run(async () => await LoadData());
        }

        [RelayCommand]
        private async Task LoadData()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    AvailableCalls.Clear();
                    OnlineTechnicians.Clear();
                });

                // 1. Busca chamados abertos
                var chamados = await _technicianService.GetTicketsAbertosAsync();

                // 2. Busca status dos colegas (e aproveita para tentar pegar o MEU status atual do servidor se possível)
                // Nota: A API atual não tem um endpoint "GetMyStatus", então assumimos o último conhecido ou Online.
                var metrics = await _managementService.GetDashboardMetricsAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (chamados != null)
                    {
                        foreach (var chamado in chamados) AvailableCalls.Add(chamado);
                    }

                    if (metrics != null && metrics.StatusTecnicos != null)
                    {
                        foreach (var tec in metrics.StatusTecnicos) OnlineTechnicians.Add(tec);

                        // Tenta atualizar meu status na tela com o que veio do servidor
                        var meuStatus = metrics.StatusTecnicos.FirstOrDefault(t => t.NomeTecnico == AuthService.LoggedUserName);
                        if (meuStatus != null)
                        {
                            TechnicianStatus = meuStatus.StatusDisponibilidade;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao carregar: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task PegarChamado(TicketListDto ticket)
        {
            if (ticket == null) return;

            try
            {
                await _technicianService.AceitarTicketAsync(ticket.Id);
                await Shell.Current.DisplayAlert("Sucesso", $"Chamado #{ticket.Id} assumido!", "OK");
                await LoadData();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Falha ao assumir: {ex.Message}", "OK");
            }
        }

        // === LÓGICA DE MUDANÇA DE STATUS ===
        // Este método aceita qualquer string: "Online", "Almoco", "Cafe", "Offline"
        [RelayCommand]
        private async Task MudarStatus(string novoStatus)
        {
            try
            {
                // 1. Chama a API para salvar no banco
                await _technicianService.UpdateStatusAsync(novoStatus);

                // 2. Atualiza a tela localmente
                TechnicianStatus = novoStatus;

                // Feedback visual (opcional)
                // await Shell.Current.DisplayAlert("Status", $"Alterado para {novoStatus}", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Erro", "Não foi possível atualizar seu status. Verifique a conexão.", "OK");
            }
        }

        [RelayCommand]
        private async Task IrParaFila()
        {
            await Shell.Current.GoToAsync(nameof(TechnicianCallsPage));
        }

        [RelayCommand]
        private async Task IrParaMeusAtendimentos()
        {
            await Shell.Current.GoToAsync(nameof(TechnicianCallsPage));
        }

        [RelayCommand]
        private async Task Logout()
        {
            bool answer = await Shell.Current.DisplayAlert("Sair", "Deseja sair da conta?", "Sim", "Não");
            if (answer)
            {
                // Antes de sair, define status como Offline (boa prática)
                try { await _technicianService.UpdateStatusAsync("Offline"); } catch { }

                _authService.Logout();
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}