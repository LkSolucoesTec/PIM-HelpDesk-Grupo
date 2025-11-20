using HelpDesk.Mobile.ViewModels;

namespace HelpDesk.Mobile.Views;

public partial class IAChatPage : ContentPage
{
    public IAChatPage(IAChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // <--- ESSENCIAL
    }
}