using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDesk.Mobile.Services;
using SuporteTI.Shared.Models.Dto;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HelpDesk.Mobile.ViewModels
{
    public partial class TechnicianCallsViewModel : ObservableObject
    {
        private readonly TechnicianService _technicianService;

        // Lista de chamados que aparece na tela
        [ObservableProperty]
        private ObservableCollection<TicketListDto> chamadosAbertos;

        // Controla o loading (spinner)
        [ObservableProperty]
        private bool isBusy;

        // Controla o refresh (puxar para atualizar)
        [ObservableProperty]
        private bool isRefreshing;

        public TechnicianCallsViewModel(TechnicianService technicianService)
        {
            _technicianService = technicianService;
            ChamadosAbertos = new ObservableCollection<TicketListDto>();

            // Carrega os dados automaticamente ao abrir a tela
            Task.Run(async () => await CarregarChamados());
        }

        [RelayCommand]
        private async Task CarregarChamados()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Limpa a lista na thread principal
                MainThread.BeginInvokeOnMainThread(() => ChamadosAbertos.Clear());

                // Busca os tickets abertos na API
                var tickets = await _technicianService.GetTicketsAbertosAsync();

                if (tickets != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var t in tickets)
                        {
                            ChamadosAbertos.Add(t);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao carregar fila: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task PegarChamado(TicketListDto ticket)
        {
            if (ticket == null) return;

            // Pergunta se tem certeza
            bool confirm = await Shell.Current.DisplayAlert("Confirmar", $"Deseja assumir o chamado '{ticket.Titulo}'?", "Sim", "Não");
            if (!confirm) return;

            try
            {
                // Chama a API para aceitar o ticket
                await _technicianService.AceitarTicketAsync(ticket.Id);

                await Shell.Current.DisplayAlert("Sucesso", "Chamado assumido! Ele foi movido para seus atendimentos.", "OK");

                // Atualiza a lista para remover o chamado que acabou de ser pego
                await CarregarChamados();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Falha ao assumir: {ex.Message}", "OK");
            }
        }
    }
}