using System.ComponentModel.DataAnnotations;

namespace SuporteTI.Shared.Models
{
    public class HistoricoChat
    {
        [Key]
        public int Id { get; set; }
        public int ChamadoId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Remetente { get; set; } // "Bot", "Usuario", "Tecnico"
        public string Conteudo { get; set; }
        public bool? Resolveu { get; set; } // Para os botões Sim/Não

        // --- Relacionamento ---
        public virtual Chamado Chamado { get; set; }
    }
}