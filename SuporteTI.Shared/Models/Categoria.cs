using System.ComponentModel.DataAnnotations;

namespace SuporteTI.Shared.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } // "Hardware", "Software", "Rede"
    }
}
