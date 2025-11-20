namespace SuporteTI.Shared.Models.Dto
{
    public class UserCreateDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Papel { get; set; } // "Usuario", "Tecnico", "Gerencia"
        public string? Setor { get; set; }
        public string? Telefone { get; set; }
        public string? Especialidade { get; set; }
    }
}