using RIMS.Models.Entities;
using RIMS.Models.ViewModels;

namespace RIMS.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task RemoveByPatternAsync(string pattern);
        Task ClearAllAsync();

        // Specific cache keys
        Task<IEnumerable<RIMSResident>> GetResidentsCacheAsync();
        Task SetResidentsCacheAsync(IEnumerable<RIMSResident> residents);
        Task<IEnumerable<RIMSDocument>> GetDocumentsCacheAsync();
        Task SetDocumentsCacheAsync(IEnumerable<RIMSDocument> documents);

        Task<DashboardAnalytics> GetDashboardCacheAsync(); // Now uses RIMS.Models.ViewModels.DashboardAnalytics
        Task SetDashboardCacheAsync(DashboardAnalytics analytics);

        // Cache statistics
        Task<CacheStatistics> GetCacheStatisticsAsync();
    }

    public class CacheStatistics
    {
        public int TotalItems { get; set; }
        public long MemoryUsage { get; set; }
        public int HitCount { get; set; }
        public int MissCount { get; set; }
        public double HitRatio { get; set; }
    }

    // REMOVE this duplicate DashboardAnalytics class
    // public class DashboardAnalytics
    // {
    //     public int TotalResidents { get; set; }
    //     public int TotalDocuments { get; set; }
    //     public int PendingApplications { get; set; }
    //     public int CompletedApplications { get; set; }
    //     public Dictionary<string, int> ApplicationsByStatus { get; set; } = new Dictionary<string, int>();
    //     public Dictionary<string, int> ResidentsByCategory { get; set; } = new Dictionary<string, int>();
    //     public List<MonthlyStat> MonthlyStats { get; set; } = new List<MonthlyStat>();
    // }

    public class MonthlyStat
    {
        public string Month { get; set; } = string.Empty;
        public int Applications { get; set; }
        public int Completions { get; set; }
    }
}