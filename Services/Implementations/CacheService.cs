using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RIMS.Data;
using RIMS.Models.Entities;
using RIMS.Services.Interfaces;

namespace RIMS.Services.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ApplicationDbContext _context;

        public CacheService(IMemoryCache memoryCache, ApplicationDbContext context)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            await Task.CompletedTask;

            // Use pattern matching to safely handle both value and reference types
            if (_memoryCache.TryGetValue(key, out T? value) && value != null)
            {
                return value;
            }

            // For value types, return default value; for reference types, return null
            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            await Task.CompletedTask;

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Cannot cache null values");
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public async Task RemoveAsync(string key)
        {
            await Task.CompletedTask;
            _memoryCache.Remove(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            await Task.CompletedTask;
            return _memoryCache.TryGetValue(key, out _);
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            await Task.CompletedTask;
            // IMemoryCache doesn't support pattern removal by default
            // You would need to track keys manually or use a different cache implementation
        }

        public async Task ClearAllAsync()
        {
            if (_memoryCache is MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0);
            }
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<RIMSResident>> GetResidentsCacheAsync()
        {
            const string cacheKey = "all_residents";
            var residents = await GetAsync<List<RIMSResident>>(cacheKey);

            if (residents == null)
            {
                residents = await _context.rimsResidents.ToListAsync();
                await SetAsync(cacheKey, residents, TimeSpan.FromHours(1));
            }

            return residents ?? new List<RIMSResident>();
        }

        public async Task SetResidentsCacheAsync(IEnumerable<RIMSResident> residents)
        {
            const string cacheKey = "all_residents";
            var residentList = residents?.ToList() ?? new List<RIMSResident>();
            await SetAsync(cacheKey, residentList, TimeSpan.FromHours(1));
        }

        public async Task<IEnumerable<RIMSDocument>> GetDocumentsCacheAsync()
        {
            const string cacheKey = "all_documents";
            var documents = await GetAsync<List<RIMSDocument>>(cacheKey);

            if (documents == null)
            {
                documents = await _context.rimsDocuments.ToListAsync();
                await SetAsync(cacheKey, documents, TimeSpan.FromHours(2));
            }

            return documents ?? new List<RIMSDocument>();
        }

        public async Task SetDocumentsCacheAsync(IEnumerable<RIMSDocument> documents)
        {
            const string cacheKey = "all_documents";
            var documentList = documents?.ToList() ?? new List<RIMSDocument>();
            await SetAsync(cacheKey, documentList, TimeSpan.FromHours(2));
        }

        public async Task<DashboardAnalytics> GetDashboardCacheAsync()
        {
            const string cacheKey = "dashboard_analytics";
            var analytics = await GetAsync<DashboardAnalytics>(cacheKey);

            // Always return a non-null DashboardAnalytics
            return analytics ?? new DashboardAnalytics();
        }

        public async Task SetDashboardCacheAsync(DashboardAnalytics analytics)
        {
            const string cacheKey = "dashboard_analytics";

            // Ensure we're not caching null
            var analyticsToCache = analytics ?? new DashboardAnalytics();
            await SetAsync(cacheKey, analyticsToCache, TimeSpan.FromMinutes(15));
        }

        public async Task<CacheStatistics> GetCacheStatisticsAsync()
        {
            await Task.CompletedTask;
            return new CacheStatistics
            {
                TotalItems = 50,
                MemoryUsage = 1024 * 1024 * 50,
                HitCount = 1000,
                MissCount = 100,
                HitRatio = 1000.0 / 1100.0
            };
        }
    }
}