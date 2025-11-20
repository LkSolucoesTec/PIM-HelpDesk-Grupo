using System.ComponentModel.DataAnnotations;

namespace SuporteTI.Shared.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; } // Será o login
        public string Senha { get; set; }
        public string Papel { get; set; } // "Usuario", "Tecnico", "Gerencia"

        // --- Campos específicos de Usuário ---
        public string? Setor { get; set; }
        public string? Telefone { get; set; }

        // --- Campos específicos de Técnico ---
        public string? Especialidade { get; set; }
        public string StatusDisponibilidade { get; set; } = "Offline"; // "Online", "Almoco", "Cafe", "Offline"

        // --- Relacionamentos ---
        public virtual ICollection<Chamado> ChamadosAbertos { get; set; } = new List<Chamado>();
        public virtual ICollection<Chamado> ChamadosAtendidos { get; set; } = new List<Chamado>();
    }
}