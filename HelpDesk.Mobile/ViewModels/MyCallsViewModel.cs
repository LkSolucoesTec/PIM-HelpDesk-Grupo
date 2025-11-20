using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDesk.Mobile.Services;
using SuporteTI.Shared.Models.Dto;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HelpDesk.Mobile.ViewModels
{
    public partial class MyCallsViewModel : ObservableObject
    {
        // Serviço usado para buscar os tickets (reutilizando o TechnicianService que já tem os métodos)
        private readonly TechnicianService _ticketService;

        [ObservableProperty]
        private ObservableCollection<TicketListDto> meusChamados;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isRefreshing;

        public MyCallsViewModel(TechnicianService ticketService)
        {
            _ticketService = ticketService;
            MeusChamados = new ObservableCollection<TicketListDto>();

            // Carrega automaticamente ao abrir a tela (em background para não travar)
            Task.Run(async () => await CarregarChamados());
        }

        [RelayCommand]
        private async Task CarregarChamados()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Limpa a lista na thread principal para evitar erros de concorrência
                MainThread.BeginInvokeOnMainThread(() => MeusChamados.Clear());

                // Busca os chamados abertos na API
                // (Nota: Como paliativo, estamos pegando TODOS os abertos. 
                // O ideal seria a API filtrar pelo usuário logado.)
                var tickets = await _ticketService.GetTicketsAbertosAsync();

                if (tickets != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var ticket in tickets)
                        {
                            // Opcional: Filtrar aqui se quiser ver apenas os seus
                            // if (ticket.NomeSolicitante == "Seu Nome")
                            MeusChamados.Add(ticket);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MyCallsViewModel] Erro: {ex.Message}");
                // Erro silencioso para não atrapalhar a experiência se a lista estiver vazia
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false; // Para o spinner do "puxar para atualizar"
            }
        }
    }
}