namespace SuporteTI.Shared.Models.Dto
{
    public class ConfirmacaoRequest
    {
        public int ChamadoId { get; set; }
        public bool Resolveu { get; set; } // true = Sim, false = Não
    }
}