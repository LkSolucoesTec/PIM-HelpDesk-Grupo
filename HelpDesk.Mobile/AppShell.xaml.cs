using HelpDesk.Mobile.Views;

namespace HelpDesk.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // === REGISTRO DE ROTAS (PUSH NAVIGATION) ===
        // Registrar as páginas aqui permite usar GoToAsync("NomeDaPagina")
        // e ter o botão de voltar (back arrow) automaticamente.

        // Cliente
        Routing.RegisterRoute(nameof(IAChatPage), typeof(IAChatPage));
        Routing.RegisterRoute(nameof(NewCallPage), typeof(NewCallPage));
        Routing.RegisterRoute(nameof(MyCallsPage), typeof(MyCallsPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));

        // Técnico
        Routing.RegisterRoute(nameof(TechnicianCallsPage), typeof(TechnicianCallsPage));
        // Routing.RegisterRoute(nameof(TicketDetailPage), typeof(TicketDetailPage)); // Se houver detalhes

        // Gerência
        Routing.RegisterRoute(nameof(ManagementUsersPage), typeof(ManagementUsersPage));
    }
}