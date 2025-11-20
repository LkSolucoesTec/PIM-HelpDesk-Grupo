using SuporteTI.Web.Components;
using SuporteTI.Web.Services;
using SuporteTI.Web.ViewModels;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Registra os serviços do Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Registra os serviços do App
builder.Services.AddMudServices();
builder.Services.AddHttpClient<ApiClient>();
builder.Services.AddHttpClient<AuthService>();
builder.Services.AddScoped<ChatPageViewModel>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// app.UseHttpsRedirection(); // <-- Desabilitado
app.UseStaticFiles();
app.UseAntiforgery();

// 3. Mapeia os componentes
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
