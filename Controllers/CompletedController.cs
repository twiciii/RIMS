using Microsoft.AspNetCore.Mvc;
using RIMS.Models.Entities;
using RIMS.Models.ViewModels;
using RIMS.Services.Interfaces;

namespace RIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompletedController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IAuditService _auditService;

        public CompletedController(IDocumentService documentService, IAuditService auditService)
        {
            _documentService = documentService;
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>> GetCompletedApplications()
        {
            try
            {
                var completedApplications = await _documentService.GetCompletedApplicationsAsync();
                await _auditService.LogActionAsync("CompletedController.GetCompletedApplications", "Retrieved completed applications", User.Identity?.Name ?? "System");
                return Ok(completedApplications);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CompletedController.GetCompletedApplications", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>>GetCompletedApplication()
        {
            try
            {
                var todayCompleted = await _documentService.GetTodayCompletedApplicationsAsync();
                await _auditService.LogActionAsync("CompletedController.GetTodayCompletedApplications", "Retrieved today's completed applications", User.Identity?.Name ?? "System");
                return Ok(todayCompleted);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CompletedController.GetTodayCompletedApplications", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>> GetCompletedApplicationsByDate(DateTime date)
        {
            try
            {
                var completedByDate = await _documentService.GetCompletedApplicationsByDateAsync(date);
                await _auditService.LogActionAsync("CompletedController.GetCompletedApplicationsByDate", $"Retrieved completed applications for date: {date.ToShortDateString()}", User.Identity?.Name ?? "System");
                return Ok(completedByDate);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CompletedController.GetCompletedApplicationsByDate", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<CompletionStats>> GetCompletionStatistics()
        {
            try
            {
                var statistics = await _documentService.GetCompletionStatisticsAsync();
                await _auditService.LogActionAsync("CompletedController.GetCompletionStatistics", "Retrieved completion statistics", User.Identity?.Name ?? "System");
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CompletedController.GetCompletionStatistics", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("archive/{applicationId}")]
        public async Task<IActionResult> ArchiveCompletedApplication(int applicationId)
        {
            try
            {
                await _documentService.ArchiveApplicationAsync(applicationId);
                await _auditService.LogActionAsync("CompletedController.ArchiveCompletedApplication", $"Archived completed application ID: {applicationId}", User.Identity?.Name ?? "System");

                return Ok(new { message = "Application archived successfully" });
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CompletedController.ArchiveCompletedApplication", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    // Renamed to avoid conflict with interface namespace
    public class CompletionStats
    {
        public int TotalCompleted { get; set; }
        public int CompletedToday { get; set; }
        public int CompletedThisWeek { get; set; }
        public int CompletedThisMonth { get; set; }
        public Dictionary<string, int> CompletedByDocumentType { get; set; } = new Dictionary<string, int>();
    }
}