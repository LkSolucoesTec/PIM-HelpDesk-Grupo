using SuporteTI.Shared.Models; // <-- MUDANÇA: Usa o .Shared da nossa API
using Newtonsoft.Json;
using System.Text;

namespace HelpDesk.Mobile.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ApiService _apiService;

    public AuthService(ApiService apiService)
    {
        _apiService = apiService;

#if ANDROID
        var handler = new Platforms.Android.CustomHttpClientHandler();
#else
        var handler = new HttpClientHandler();
#endif

        // URL da NOSSA API (SuporteTI.Api), porta 5219
        string portaApi = "5219";
        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri($"http://{(DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost")}:{portaApi}/api/auth/")
        };
    }

    // Propriedades estáticas para o app saber quem está logado
    public static int LoggedUserId { get; private set; }
    public static string LoggedUserRole { get; private set; }
    public static string LoggedUserName { get; private set; }

    // MUDANÇA: Retorna "LoginResponse" (do SuporteTI.Shared)
    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var request = new LoginRequest { Email = email, Senha = password };
        var jsonPayload = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("login", content);
        response.EnsureSuccessStatusCode(); // Vai falhar aqui se a API estiver offline

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);

        if (loginResponse != null)
        {
            _apiService.SetAuthToken(loginResponse.Token);
            LoggedUserId = loginResponse.Id;
            LoggedUserRole = loginResponse.Papel;
            LoggedUserName = loginResponse.Nome;
        }

        return loginResponse;
    }

    public void Logout()
    {
        _apiService.SetAuthToken(null);
        LoggedUserId = 0;
        LoggedUserRole = null;
        LoggedUserName = null;
    }
}