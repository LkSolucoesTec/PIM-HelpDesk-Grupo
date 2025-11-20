namespace SuporteTI.Shared.Models.Dto
{
    public class TicketDetailDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string DescricaoInicial { get; set; }
        public string Status { get; set; }
        public string Prioridade { get; set; }
        public DateTime DataAbertura { get; set; }

        // Dados do Solicitante
        public string NomeSolicitante { get; set; }
        public string SetorSolicitante { get; set; }
        public string TelefoneSolicitante { get; set; }

        // Histórico do Chat
        public List<HistoricoChatDto> Historico { get; set; } = new List<HistoricoChatDto>();
    }
}