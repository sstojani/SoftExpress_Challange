namespace SoftExpress_Challange.ViewModels
{
    public class StatistikaIndexViewModel
    {
        public List<StatistikaDto> Statistikas { get; set; } = new();

        // Filters
        public string? UserId { get; set; }
        public string? Rajoni { get; set; }
        public string? LlojiIPajisjes { get; set; }
        public string? Aplikacioni { get; set; }
        public DateTime? DataOra { get; set; }
    }

}
