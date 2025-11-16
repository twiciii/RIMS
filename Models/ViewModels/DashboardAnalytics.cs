namespace RIMS.Models.ViewModels
{
    public class DashboardAnalytics
    {
        public int TotalResidents { get; set; }
        public int NewResidentsThisMonth { get; set; }
        public int PendingApplications { get; set; }
        public int CompletedApplicationsToday { get; set; }
        public int CompletedApplications { get; set; }
        public int TotalDocuments { get; set; }
        public decimal CompletionRate { get; set; }
        public Dictionary<string, int> ApplicationsByStatus { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ResidentsByCategory { get; set; } = new Dictionary<string, int>();
        public List<MonthlyStat> MonthlyStats { get; set; } = new List<MonthlyStat>();
    }

    public class MonthlyStat
    {
        public string Month { get; set; } = string.Empty;
        public int Applications { get; set; }
        public int Completions { get; set; }
    }

    public class ResidentAnalytics
    {
        public AgeDistribution AgeDistribution { get; set; } = new AgeDistribution();
        public GenderDistribution GenderDistribution { get; set; } = new GenderDistribution();
        public Dictionary<string, int> ResidentsByBarangay { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> CategoryCounts { get; set; } = new Dictionary<string, int>();
        public TrendData MonthlyGrowth { get; set; } = new TrendData();
    }

    public class AgeDistribution
    {
        public int Children { get; set; }
        public int Youth { get; set; }
        public int Adults { get; set; }
        public int Seniors { get; set; }
    }

    public class GenderDistribution
    {
        public int Male { get; set; }
        public int Female { get; set; }
        public int Other { get; set; }
    }

    public class TrendData
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<int> Values { get; set; } = new List<int>();
    }

    public class DocumentAnalytics
    {
        public int TotalApplications { get; set; }
        public int ApplicationsThisMonth { get; set; }
        public Dictionary<string, int> ApplicationsByType { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ApplicationsByStatus { get; set; } = new Dictionary<string, int>();
        public AverageProcessingTime ProcessingTimes { get; set; } = new AverageProcessingTime();
    }

    public class AverageProcessingTime
    {
        public double Certificate { get; set; }
        public double Clearance { get; set; }
        public double BusinessPermit { get; set; }
        public double Other { get; set; }
        public double Overall { get; set; }
    }

    public class ForecastData
    {
        public List<string> Periods { get; set; } = new List<string>();
        public List<int> ExpectedValues { get; set; } = new List<int>();
        public List<int> LowerBounds { get; set; } = new List<int>();
        public List<int> UpperBounds { get; set; } = new List<int>();
    }
}