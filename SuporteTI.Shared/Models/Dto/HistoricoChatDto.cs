namespace SuporteTI.Shared.Models.Dto
{
    public class HistoricoChatDto
    {
        public string Remetente { get; set; }
        public string Conteudo { get; set; }
        public DateTime Timestamp { get; set; }
        public bool? Resolveu { get; set; }
    }
}