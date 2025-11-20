using HelpDesk.Mobile.ViewModels;

namespace HelpDesk.Mobile.Views;

public partial class ManagementUsersPage : ContentPage
{
    // O construtor recebe o ViewModel via Injeção de Dependência (DI)
    public ManagementUsersPage(ManagementUsersViewModel viewModel)
    {
        InitializeComponent();
        // Atribui o ViewModel ao contexto de ligação da View
        BindingContext = viewModel;
    }
}
