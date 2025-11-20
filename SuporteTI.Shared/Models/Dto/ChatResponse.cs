namespace SuporteTI.Shared.Models.Dto
{
    public class ChatResponse
    {
        public int ChamadoId { get; set; }
        public string RespostaBot { get; set; }
        public bool PedirConfirmacao { get; set; } // Se for true, o front-end mostra os botões [Sim] e [Não]
        public bool ChamadoAberto { get; set; } // Se for true, o bot parou e um técnico foi chamado
    }
}