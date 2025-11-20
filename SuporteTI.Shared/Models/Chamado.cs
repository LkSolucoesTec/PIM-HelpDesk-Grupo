using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuporteTI.Shared.Models
{
    public class Chamado
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string DescricaoInicial { get; set; }
        public string Status { get; set; } // "BotAtendimento", "Aberto", "EmAndamento", "ResolvidoIA", "ResolvidoTecnico"
        public string Prioridade { get; set; } = "Baixa"; // "Baixa", "Media", "Alta"
        public DateTime DataAbertura { get; set; } = DateTime.Now;
        public DateTime? DataFechamento { get; set; }
        public string? ObservacaoTecnica { get; set; } // Relatório do técnico

        // --- Chaves Estrangeiras ---
        [ForeignKey("UsuarioSolicitante")]
        public int UsuarioSolicitanteId { get; set; }
        public virtual Usuario UsuarioSolicitante { get; set; }

        [ForeignKey("TecnicoResponsavel")]
        public int? TecnicoResponsavelId { get; set; } // Nulo até um técnico pegar
        public virtual Usuario? TecnicoResponsavel { get; set; }

        public int? CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }

        // --- Relacionamentos ---
        public virtual ICollection<HistoricoChat> Historico { get; set; } = new List<HistoricoChat>();
    }
}