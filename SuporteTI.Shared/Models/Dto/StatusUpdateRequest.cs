namespace SuporteTI.Shared.Models.Dto
{
    public class StatusUpdateRequest
    {
        public int TecnicoId { get; set; } // <--- CAMPO NOVO ADICIONADO
        public string NovoStatus { get; set; }
    }
}