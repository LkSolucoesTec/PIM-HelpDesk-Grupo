using HelpDesk.Mobile.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace HelpDesk.Mobile.Views;

public partial class MyCallsPage : ContentPage
{
    public MyCallsPage(MyCallsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

