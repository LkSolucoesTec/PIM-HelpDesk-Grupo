using System.ComponentModel.DataAnnotations;

namespace SuporteTI.Shared.Models
{
    public class SolucaoIA
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; } // A solução passo a passo com "|STEP|"
        public string PalavrasChave { get; set; } // "internet, rede, wifi, lento"
        public int CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }
    }
}