namespace SuporteTI.Shared.Models.Dto
{
    public class DashboardMetricsDto
    {
        public int TotalChamados { get; set; }
        public int TotalAbertos { get; set; }
        public int TotalEmAndamento { get; set; }
        public int TotalResolvidosIA { get; set; }
        public int TotalResolvidosTecnico { get; set; }
        public double TempoMedioResolucaoHoras { get; set; } // Em horas
        public List<TecnicoStatusDto> StatusTecnicos { get; set; } = new List<TecnicoStatusDto>();
    }
}