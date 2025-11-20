using HelpDesk.Mobile.ViewModels;

namespace HelpDesk.Mobile.Views;

public partial class LoginPage : ContentPage
{
    // O construtor recebe o ViewModel via Injeção de Dependência
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        // Define o ViewModel como o contexto de ligação (BindingContext) da View
        BindingContext = viewModel;
    }
}