using HelpDesk.Mobile.ViewModels;

namespace HelpDesk.Mobile.Views;

public partial class ManagementDashboardPage : ContentPage
{
    public ManagementDashboardPage(ManagementDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
