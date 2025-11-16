using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;
using RIMS.Services.Interfaces;

namespace RIMS.Services.Implementations
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SearchResults> SearchAsync(string searchTerm)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var residentsTask = SearchResidentsAsync(searchTerm);
            var documentsTask = SearchDocumentsAsync(searchTerm);

            // Use await to run tasks asynchronously
            await Task.WhenAll(residentsTask, documentsTask);

            var residents = residentsTask.Result;
            var documents = documentsTask.Result;

            stopwatch.Stop();

            return new SearchResults
            {
                Residents = residents,
                Documents = documents,
                Applications = documents, // Use documents for applications since they're the same type
                TotalResults = residents.Count() + documents.Count(),
                SearchDuration = stopwatch.Elapsed
            };
        }

        public async Task<SearchResults> AdvancedSearchAsync(AdvancedSearchModel searchModel)
        {
            var query = _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                query = query.Where(da =>
                    (da.Resident != null && da.Resident.FirstName != null && da.Resident.FirstName.Contains(searchModel.Name)) ||
                    (da.Resident != null && da.Resident.LastName != null && da.Resident.LastName.Contains(searchModel.Name)));
            }

            if (!string.IsNullOrEmpty(searchModel.DocumentType))
            {
                query = query.Where(da => da.Document != null && da.Document.DocumentName != null && da.Document.DocumentName.Contains(searchModel.DocumentType));
            }

            if (searchModel.DateFrom.HasValue)
            {
                query = query.Where(da => da.DateInsurance >= searchModel.DateFrom.Value);
            }

            if (searchModel.DateTo.HasValue)
            {
                query = query.Where(da => da.DateInsurance <= searchModel.DateTo.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.Status))
            {
                query = query.Where(da => da.Status == searchModel.Status);
            }

            var applications = await query.ToListAsync();
            var residents = await SearchResidentsAsync(searchModel.Name ?? "");

            return new SearchResults
            {
                Residents = residents,
                Documents = applications,
                Applications = applications,
                TotalResults = residents.Count() + applications.Count()
            };
        }

        public async Task<QuickSearchResults> QuickSearchAsync(string term)
        {
            var residentsTask = SearchResidentsAsync(term);
            var documentsTask = SearchDocumentsAsync(term);

            // Use await to run tasks asynchronously
            await Task.WhenAll(residentsTask, documentsTask);

            var residents = residentsTask.Result;
            var documents = documentsTask.Result;

            return new QuickSearchResults
            {
                Residents = residents.Take(5),
                Documents = documents.Take(5)
            };
        }

        public async Task<IEnumerable<RIMSResident>> SearchResidentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<RIMSResident>();

            return await _context.rimsResidents
                .Where(r =>
                    (r.FirstName != null && r.FirstName.Contains(searchTerm)) ||
                    (r.LastName != null && r.LastName.Contains(searchTerm)) ||
                    (r.MiddleName != null && r.MiddleName.Contains(searchTerm)) ||
                    (r.Address != null && r.Address.Contains(searchTerm)) ||
                    (r.Email != null && r.Email.Contains(searchTerm)))
                .Take(50)
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> SearchDocumentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<RIMSDocumentApplication>();

            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da =>
                    (da.Resident != null && da.Resident.FirstName != null && da.Resident.FirstName.Contains(searchTerm)) ||
                    (da.Resident != null && da.Resident.LastName != null && da.Resident.LastName.Contains(searchTerm)) ||
                    (da.Document != null && da.Document.DocumentName != null && da.Document.DocumentName.Contains(searchTerm)) ||
                    (da.Purpose != null && da.Purpose.Contains(searchTerm)))
                .Take(50)
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> SearchApplicationsAsync(ApplicationSearchModel searchModel)
        {
            var query = _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.ResidentName))
            {
                query = query.Where(da =>
                    (da.Resident != null && da.Resident.FirstName != null && da.Resident.FirstName.Contains(searchModel.ResidentName)) ||
                    (da.Resident != null && da.Resident.LastName != null && da.Resident.LastName.Contains(searchModel.ResidentName)));
            }

            if (!string.IsNullOrEmpty(searchModel.DocumentType))
            {
                query = query.Where(da => da.Document != null && da.Document.DocumentName != null && da.Document.DocumentName.Contains(searchModel.DocumentType));
            }

            if (!string.IsNullOrEmpty(searchModel.Status))
            {
                query = query.Where(da => da.Status == searchModel.Status);
            }

            if (searchModel.FromDate.HasValue)
            {
                query = query.Where(da => da.DateInsurance >= searchModel.FromDate.Value);
            }

            if (searchModel.ToDate.HasValue)
            {
                query = query.Where(da => da.DateInsurance <= searchModel.ToDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetSearchSuggestionsAsync(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix) || prefix.Length < 2)
                return new List<string>();

            var residentNamesTask = _context.rimsResidents
                .Where(r => (r.FirstName != null && r.FirstName.StartsWith(prefix)) || (r.LastName != null && r.LastName.StartsWith(prefix)))
                .Select(r => (r.FirstName ?? "") + " " + (r.LastName ?? ""))
                .Distinct()
                .Take(10)
                .ToListAsync();

            var documentTypesTask = _context.rimsDocuments
                .Where(d => d.DocumentName != null && d.DocumentName.StartsWith(prefix))
                .Select(d => d.DocumentName!)
                .Distinct()
                .Take(10)
                .ToListAsync();

            // Use await to run tasks asynchronously
            await Task.WhenAll(residentNamesTask, documentTypesTask);

            var residentNames = residentNamesTask.Result;
            var documentTypes = documentTypesTask.Result;

            return residentNames.Concat(documentTypes).Take(15);
        }

        public async Task<SearchAnalytics> GetSearchAnalyticsAsync()
        {
            // For now, return mock data asynchronously
            await Task.CompletedTask; // This makes the method truly async

            return new SearchAnalytics
            {
                TotalSearches = 1000,
                SuccessfulSearches = 950,
                PopularSearchTerms = new Dictionary<string, int>
                {
                    {"birth certificate", 150},
                    {"business permit", 120},
                    {"id card", 100}
                },
                SearchByType = new Dictionary<string, int>
                {
                    {"Resident", 400},
                    {"Document", 350},
                    {"Application", 250}
                },
                AverageSearchTime = 0.15
            };
        }
    }
}