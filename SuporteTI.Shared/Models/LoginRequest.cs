namespace SuporteTI.Shared.Models
{
    // Objeto que o Front-end envia para a API
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}