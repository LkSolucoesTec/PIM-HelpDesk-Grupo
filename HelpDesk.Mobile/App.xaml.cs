namespace HelpDesk.Mobile
{
    public partial class App : Application
    {
        // CORREÇÃO: O AppShell é injetado (vem do MauiProgram)
        // em vez de ser criado com 'new AppShell()'
        public App(AppShell shell)
        {
            InitializeComponent();

            // Define o Shell injetado como a página principal
            MainPage = shell;
        }

    }
}