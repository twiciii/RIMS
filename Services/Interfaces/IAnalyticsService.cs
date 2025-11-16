using RIMS.Models;
using RIMS.Models.ViewModels;

namespace RIMS.Services
{
    public interface IAnalyticsService
    {
        // Dashboard analytics
        Task<DashboardAnalytics> GetDashboardAnalyticsAsync();
        Task<SystemPerformance> GetSystemPerformanceAsync();

        // Resident analytics
        Task<ResidentAnalytics> GetResidentAnalyticsAsync();
        Task<DemographicData> GetDemographicDataAsync();
        Task<CategoryDistribution> GetCategoryDistributionAsync();

        // Document analytics
        Task<DocumentAnalytics> GetDocumentAnalyticsAsync();
        Task<ApplicationTrends> GetApplicationTrendsAsync(DateTime fromDate, DateTime toDate);
        Task<ProcessingTimes> GetProcessingTimesAsync();

        // User analytics
        Task<UserActivity> GetUserActivityAsync();
        Task<SystemUsage> GetSystemUsageAsync();

        // Predictive analytics
        Task<ForecastData> GetResidentGrowthForecastAsync(int months);
        Task<ForecastData> GetDocumentDemandForecastAsync(int months);
    }

    public class DashboardAnalytics
    {
        public int TotalResidents { get; set; }
        public int NewResidentsThisMonth { get; set; }
        public int PendingApplications { get; set; }
        public int CompletedApplicationsToday { get; set; }
        public decimal CompletionRate { get; set; }
        public Dictionary<string, int> ApplicationsByStatus { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ResidentsByCategory { get; set; } = new Dictionary<string, int>();
    }

    public class SystemPerformance
    {
        public TimeSpan Uptime { get; set; }
        public int AverageResponseTime { get; set; }
        public double ErrorRate { get; set; }
        public int ActiveUsers { get; set; }
        public string DatabasePerformance { get; set; } = string.Empty;
        public DateTime LastMaintenance { get; set; }
    }

    public class ResidentAnalytics
    {
        public AgeDistribution AgeDistribution { get; set; } = new AgeDistribution();
        public GenderDistribution GenderDistribution { get; set; } = new GenderDistribution();
        public Dictionary<string, int> ResidentsByBarangay { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> CategoryCounts { get; set; } = new Dictionary<string, int>();
        public TrendData MonthlyGrowth { get; set; } = new TrendData();
    }

    public class DocumentAnalytics
    {
        public int TotalApplications { get; set; }
        public int ApplicationsThisMonth { get; set; }
        public Dictionary<string, int> ApplicationsByType { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ApplicationsByStatus { get; set; } = new Dictionary<string, int>();
        public AverageProcessingTime ProcessingTimes { get; set; } = new AverageProcessingTime();
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

    public class ForecastData
    {
        public List<string> Periods { get; set; } = new List<string>();
        public List<int> ExpectedValues { get; set; } = new List<int>();
        public List<int> LowerBounds { get; set; } = new List<int>();
        public List<int> UpperBounds { get; set; } = new List<int>();
    }

    // Additional model classes that were missing from the interface
    public class DemographicData
    {
        public int TotalPopulation { get; set; }
        public double AverageAge { get; set; }
        public Dictionary<string, decimal> GenderRatio { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, int> AgeGroups { get; set; } = new Dictionary<string, int>();
    }

    public class CategoryDistribution
    {
        public Dictionary<string, int> Categories { get; set; } = new Dictionary<string, int>();
        public int TotalCategorized { get; set; }
    }

    public class ApplicationTrends
    {
        public List<string> Dates { get; set; } = new List<string>();
        public List<int> Counts { get; set; } = new List<int>();
        public int TotalApplications { get; set; }
        public double AverageDaily { get; set; }
    }

    public class AverageProcessingTime
    {
        public TimeSpan AverageTime { get; set; }
        public TimeSpan FastestTime { get; set; }
        public TimeSpan SlowestTime { get; set; }
    }

    public class ProcessingTimes
    {
        public TimeSpan AverageProcessingTime { get; set; }
        public TimeSpan MedianProcessingTime { get; set; }
        public Dictionary<string, int> ProcessingTimeDistribution { get; set; } = new Dictionary<string, int>();
    }

    public class UserActivity
    {
        public int ActiveUsers { get; set; }
        public string MostActiveUser { get; set; } = string.Empty;
        public double AverageActivitiesPerUser { get; set; }
        public Dictionary<int, int> ActivityByHour { get; set; } = new Dictionary<int, int>();
    }

    public class SystemUsage
    {
        public Dictionary<int, int> PeakUsageHours { get; set; } = new Dictionary<int, int>();
        public TimeSpan AverageSessionDuration { get; set; }
        public Dictionary<string, int> MostUsedFeatures { get; set; } = new Dictionary<string, int>();
        public Dictionary<DateTime, int> ConcurrentUsers { get; set; } = new Dictionary<DateTime, int>();
    }
}