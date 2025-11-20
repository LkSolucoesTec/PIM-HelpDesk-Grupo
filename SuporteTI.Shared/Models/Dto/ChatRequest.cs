namespace SuporteTI.Shared.Models.Dto
{
    public class ChatRequest
    {
        public int ChamadoId { get; set; } // 0 se for a primeira mensagem
        public int UsuarioId { get; set; }
        public string Mensagem { get; set; }
    }
}