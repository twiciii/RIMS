using RIMS.Models.Entities;

namespace RIMS.Services.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResults> SearchAsync(string searchTerm);
        Task<SearchResults> AdvancedSearchAsync(AdvancedSearchModel searchModel);
        Task<QuickSearchResults> QuickSearchAsync(string term);

        Task<IEnumerable<RIMSResident>> SearchResidentsAsync(string searchTerm);
        Task<IEnumerable<RIMSDocumentApplication>> SearchDocumentsAsync(string searchTerm);
        Task<IEnumerable<RIMSDocumentApplication>> SearchApplicationsAsync(ApplicationSearchModel searchModel);

        Task<IEnumerable<string>> GetSearchSuggestionsAsync(string prefix);
        Task<SearchAnalytics> GetSearchAnalyticsAsync();
    }

    public class SearchResults
    {
        public IEnumerable<RIMSResident> Residents { get; set; } = new List<RIMSResident>();
        public IEnumerable<RIMSDocumentApplication> Documents { get; set; } = new List<RIMSDocumentApplication>();
        public IEnumerable<RIMSDocumentApplication> Applications { get; set; } = new List<RIMSDocumentApplication>();
        public int TotalResults { get; set; }
        public TimeSpan SearchDuration { get; set; }
    }

    public class QuickSearchResults
    {
        public IEnumerable<RIMSResident> Residents { get; set; } = new List<RIMSResident>();
        public IEnumerable<RIMSDocumentApplication> Documents { get; set; } = new List<RIMSDocumentApplication>();
    }

    public class ApplicationSearchModel
    {
        public string? ResidentName { get; set; }
        public string? DocumentType { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class AdvancedSearchModel
    {
        public string? Name { get; set; }
        public string? DocumentType { get; set; }
        public string? Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class SearchAnalytics
    {
        public int TotalSearches { get; set; }
        public int SuccessfulSearches { get; set; }
        public Dictionary<string, int> PopularSearchTerms { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> SearchByType { get; set; } = new Dictionary<string, int>();
        public double AverageSearchTime { get; set; }
    }
}