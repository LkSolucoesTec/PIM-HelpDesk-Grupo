namespace SuporteTI.Shared.Models
{
    // Objeto que a API retorna para o Front-end
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Nome { get; set; }
        public string Papel { get; set; } // "Usuario", "Tecnico", "Gerencia"
    }
}