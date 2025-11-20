using HelpDesk.Mobile.ViewModels;
namespace HelpDesk.Mobile.Views;

public partial class ClientDashboardPage : ContentPage
{
    public ClientDashboardPage(ClientDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}