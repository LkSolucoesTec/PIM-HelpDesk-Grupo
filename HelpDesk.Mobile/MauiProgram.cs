using CommunityToolkit.Maui;
using HelpDesk.Mobile.Services;
using HelpDesk.Mobile.ViewModels;
using HelpDesk.Mobile.Views;
using Microsoft.Extensions.Logging;

namespace HelpDesk.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ==================================================================
        // 1. SERVIÇOS (Singletons)
        // ==================================================================
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<CallService>();       // Essencial para o Chat
        builder.Services.AddSingleton<TechnicianService>(); // Essencial para Meus Chamados
        builder.Services.AddSingleton<ManagementService>(); // Essencial para Gerência

        // ==================================================================
        // 2. VIEWMODELS (Transient)
        // ==================================================================
        // Telas de Login e Dashboards
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ClientDashboardViewModel>();
        builder.Services.AddTransient<TechnicianDashboardViewModel>();
        builder.Services.AddTransient<ManagementDashboardViewModel>();

        // Funcionalidades do Cliente
        builder.Services.AddTransient<IAChatViewModel>();    // <--- O Chat precisa disso
        builder.Services.AddTransient<NewCallViewModel>();   // <--- Novo Chamado precisa disso
        builder.Services.AddTransient<MyCallsViewModel>();   // <--- Meus Chamados precisa disso (CRASH ATUAL)
        builder.Services.AddTransient<ProfileViewModel>();   // <--- Perfil precisa disso

        // Funcionalidades do Técnico e Gerente
        builder.Services.AddTransient<TechnicianCallsViewModel>();
        builder.Services.AddTransient<ManagementUsersViewModel>();


        // ==================================================================
        // 3. VIEWS / PÁGINAS (Transient)
        // ==================================================================
        builder.Services.AddSingleton<LoginPage>();

        // Dashboards
        builder.Services.AddTransient<ClientDashboardPage>();
        builder.Services.AddTransient<TechnicianDashboardPage>();
        builder.Services.AddTransient<ManagementDashboardPage>();

        // Páginas Funcionais
        builder.Services.AddTransient<IAChatPage>();          // <--- Página do Chat
        builder.Services.AddTransient<NewCallPage>();         // <--- Página Novo Chamado
        builder.Services.AddTransient<MyCallsPage>();         // <--- Página Meus Chamados (CORREÇÃO DO CRASH)
        builder.Services.AddTransient<ProfilePage>();         // <--- Página de Perfil

        builder.Services.AddTransient<TechnicianCallsPage>();
        builder.Services.AddTransient<ManagementUsersPage>();

        // ==================================================================
        // 4. SHELL (Navegação)
        // ==================================================================
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}