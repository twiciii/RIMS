using Microsoft.AspNetCore.Mvc;
using RIMS.Models.Entities;
using RIMS.Services;
using RIMS.Services.Interfaces;

namespace RIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IResidentService _residentService;
        private readonly IDocumentService _documentService;
        private readonly IAuditService _auditService;

        public SearchController(IResidentService residentService, IDocumentService documentService, IAuditService auditService)
        {
            _residentService = residentService;
            _documentService = documentService;
            _auditService = auditService;
        }

        [HttpGet("residents/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<RIMSResident>>> SearchResidents(string searchTerm)
        {
            try
            {
                var residents = await _residentService.SearchResidentsAsync(searchTerm);
                await _auditService.LogActionAsync("SearchController.SearchResidents", $"Searched residents with term: {searchTerm}", User.Identity?.Name ?? "System");
                return Ok(residents);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("SearchController.SearchResidents", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("documents/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>> SearchDocuments(string searchTerm)
        {
            try
            {
                var documents = await _documentService.SearchDocumentApplicationsAsync(searchTerm);
                await _auditService.LogActionAsync("SearchController.SearchDocuments", $"Searched documents with term: {searchTerm}", User.Identity?.Name ?? "System");
                return Ok(documents);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("SearchController.SearchDocuments", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("advanced")]
        public async Task<ActionResult<SearchResults>> AdvancedSearch([FromQuery] AdvancedSearchModel searchModel)
        {
            try
            {
                var results = new SearchResults();

                if (!string.IsNullOrEmpty(searchModel.Name))
                {
                    results.Residents = await _residentService.SearchResidentsAsync(searchModel.Name);
                }

                if (!string.IsNullOrEmpty(searchModel.DocumentType))
                {
                    results.Documents = await _documentService.SearchByDocumentTypeAsync(searchModel.DocumentType);
                }

                if (searchModel.DateFrom.HasValue && searchModel.DateTo.HasValue)
                {
                    results.Applications = await _documentService.SearchByDateRangeAsync(searchModel.DateFrom.Value, searchModel.DateTo.Value);
                }

                await _auditService.LogActionAsync("SearchController.AdvancedSearch", "Performed advanced search", User.Identity?.Name ?? "System");
                return Ok(results);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("SearchController.AdvancedSearch", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("quick/{term}")]
        public async Task<ActionResult<QuickSearchResults>> QuickSearch(string term)
        {
            try
            {
                var results = new QuickSearchResults
                {
                    Residents = await _residentService.SearchResidentsAsync(term),
                    Documents = await _documentService.SearchDocumentApplicationsAsync(term)
                };

                await _auditService.LogActionAsync("SearchController.QuickSearch", $"Quick search with term: {term}", User.Identity?.Name ?? "System");
                return Ok(results);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("SearchController.QuickSearch", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class AdvancedSearchModel
    {
        public string Name { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class SearchResults
    {
        public IEnumerable<RIMSResident> Residents { get; set; } = new List<RIMSResident>();
        public IEnumerable<RIMSDocumentApplication> Documents { get; set; } = new List<RIMSDocumentApplication>();
        public IEnumerable<RIMSDocumentApplication> Applications { get; set; } = new List<RIMSDocumentApplication>();
    }

    public class QuickSearchResults
    {
        public IEnumerable<RIMSResident> Residents { get; set; } = new List<RIMSResident>();
        public IEnumerable<RIMSDocumentApplication> Documents { get; set; } = new List<RIMSDocumentApplication>();
    }
}