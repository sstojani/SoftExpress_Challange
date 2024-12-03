using System;
using System.Collections.Generic;

namespace SoftExpress_Challange.ViewModels
{
    public class ReportViewModel
    {
        // Filter properties
        public string Rajoni { get; set; }
        public string Aplikacioni { get; set; }
        public string LlojiIPajisjes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Data for the charts
        public List<ChartData> GroupedByRegion { get; set; }
        public List<ChartData> GroupedByApplication { get; set; }

        public ReportViewModel()
        {
            // Initialize the lists to avoid null reference errors
            GroupedByRegion = new List<ChartData>();
            GroupedByApplication = new List<ChartData>();
        }
    }

    public class ChartData
    {
        public string Label { get; set; }
        public int Count { get; set; }
    }
}
