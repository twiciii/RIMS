namespace RIMS.Configurations
{
    public class CacheConfig
    {
        public int ResidentCacheMinutes { get; set; } = 30;
        public int SearchCacheMinutes { get; set; } = 15;
        public int AnalyticsCacheMinutes { get; set; } = 60;
        public int MaxCacheSizeMB { get; set; } = 100;
        public bool EnableCache { get; set; } = true;
    }
}