using HelpDesk.Mobile.ViewModels;
using System.Threading.Tasks;

namespace HelpDesk.Mobile.Views;

public partial class TechnicianCallsPage : ContentPage
{
    public TechnicianCallsPage(TechnicianCallsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}