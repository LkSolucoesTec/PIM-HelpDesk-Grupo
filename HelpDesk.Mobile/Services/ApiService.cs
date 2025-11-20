using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HelpDesk.Mobile.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private static string _token;

    // A URL da NOSSA API (SuporteTI.Api)
    private static string GetApiBaseUrl()
    {
        // Use a porta HTTP da sua API (ex: 5219)
        string portaApi = "5219";

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            return $"http://10.0.2.2:{portaApi}";
        }

        // Windows
        return $"http://localhost:{portaApi}";
    }

    public ApiService()
    {
        HttpMessageHandler handler;

#if ANDROID
        handler = new Platforms.Android.CustomHttpClientHandler();
#else
        handler = new HttpClientHandler();
#endif

        _httpClient = new HttpClient(handler)
        {
            // Aponta para a raiz da nossa API (ex: http://localhost:5219/api/)
            BaseAddress = new Uri($"{GetApiBaseUrl()}/api/")
        };
    }

    // Método para o AuthService definir o token
    public void SetAuthToken(string token)
    {
        _token = token;
        // Nossa API não usa mais token, mas o método está aqui
    }

    // --- Métodos Helper (GET, POST, PUT) ---

    public async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task<TResponse> PostAsync<TResponse>(string endpoint, object payload)
    {
        var jsonPayload = JsonConvert.SerializeObject(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(jsonResponse);
    }

    public async Task PutAsync(string endpoint, object payload)
    {
        var jsonPayload = JsonConvert.SerializeObject(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
    }
}