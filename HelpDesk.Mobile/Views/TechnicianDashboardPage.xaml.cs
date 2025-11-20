using HelpDesk.Mobile.ViewModels;

namespace HelpDesk.Mobile.Views;

public partial class TechnicianDashboardPage : ContentPage
{
    public TechnicianDashboardPage(TechnicianDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
