using HelpDesk.Mobile.ViewModels;

namespace HelpDesk.Mobile.Views;

public partial class NewCallPage : ContentPage
{
    public NewCallPage(NewCallViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}