#if ANDROID
using global::Android.Net.Http;
using global::System.Net.Http; // ADICIONADO: Necessário para a classe AndroidClientHandler
using Java.Net;
using Javax.Net.Ssl;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Android.Net;

namespace HelpDesk.Mobile.Platforms.Android
{
    // CORREÇÃO: AndroidMessageHandler foi substituído por AndroidClientHandler no .NET MAUI
    internal class CustomHttpClientHandler : AndroidClientHandler
    {
        public CustomHttpClientHandler()
        {
            // O ServerCertificateCustomValidationCallback é uma propriedade da classe AndroidClientHandler (e da HttpClientHandler)
            this.ServerCertificateCustomValidationCallback = (request, certificate, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None || sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
                {
                    // Essa lógica é importante para ambientes de desenvolvimento (localhost/self-signed certs)
                    if (certificate?.Issuer == "CN=localhost")
                    {
                        return true; // Confia no localhost
                    }
                }
                return sslPolicyErrors == SslPolicyErrors.None;
            };
        }

        // Este método protegido não é mais exposto em AndroidClientHandler. 
        // Para customizar o HostnameVerifier, o método correto é o Get=>ConfigureNativeHandler()
        // No entanto, para fins de teste rápido, se essa função for o problema, podemos adaptá-la ou comentá-la.
        // Se a sua customização de HostnameVerifier é *realmente* necessária, vamos reescrever essa parte.
        // Por hora, COMENTAREI a customização avançada para focar na compilação básica:
        /*
        protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection)
        {
             return new CustomHostnameVerifier();
        }
        */
    }

    // Deixo o CustomHostnameVerifier aqui, caso o código que usa essa classe precise dela
    // para ser compilado, mas ela não será mais usada pelo CustomHttpClientHandler acima,
    // a menos que você a injete de forma diferente no MAUI.
    public class CustomHostnameVerifier : Java.Lang.Object, IHostnameVerifier
    {
        public bool Verify(string hostname, ISSLSession session)
        {
            return HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session) ||
                       hostname == "10.0.2.2" || // Padrão de IP do emulador para localhost
                       hostname == "localhost";
        }
    }
}
#endif