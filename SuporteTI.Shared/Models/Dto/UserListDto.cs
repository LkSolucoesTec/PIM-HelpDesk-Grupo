namespace SuporteTI.Shared.Models.Dto
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; } // Login
        public string Papel { get; set; } // "Usuario", "Tecnico", "Gerencia"
        public string? Setor { get; set; }
        public string? Especialidade { get; set; }
        public string StatusDisponibilidade { get; set; }
    }
}