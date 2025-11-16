// CompletionStats.cs
namespace RIMS.Models.ViewModels
{
    public class CompletionStatistics
    {
        public int TotalCompleted { get; set; }
        public int CompletedToday { get; set; }
        public int CompletedThisWeek { get; set; }
        public int CompletedThisMonth { get; set; }
        public Dictionary<string, int> CompletedByDocumentType { get; set; } = new Dictionary<string, int>();
    }
}