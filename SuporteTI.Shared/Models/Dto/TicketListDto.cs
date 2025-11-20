namespace SuporteTI.Shared.Models.Dto
{
    public class TicketListDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Status { get; set; }
        public string Prioridade { get; set; }
        public DateTime DataAbertura { get; set; }
        public string NomeSolicitante { get; set; }
    }
}