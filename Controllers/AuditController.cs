using Microsoft.AspNetCore.Mvc;
using RIMS.Models.Entities;
using RIMS.Services.Interfaces;

namespace RIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RIMSAuditTrail>>> GetAuditLogs([FromQuery] AuditFilter filter)
        {
            try
            {
                var auditLogs = await _auditService.GetAuditLogsAsync(filter);
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("GetAuditLogs", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<RIMSAuditTrail>>> GetUserAuditLogs(string userId)
        {
            try
            {
                var userLogs = await _auditService.GetUserAuditLogsAsync(userId);
                return Ok(userLogs);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("GetUserAuditLogs", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<RIMSAuditTrail>>> GetAuditLogsByDate(DateTime date)
        {
            try
            {
                var dateLogs = await _auditService.GetAuditLogsByDateAsync(date);
                return Ok(dateLogs);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("GetAuditLogsByDate", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("action/{actionType}")]
        public async Task<ActionResult<IEnumerable<RIMSAuditTrail>>> GetAuditLogsByAction(string actionType)
        {
            try
            {
                var actionLogs = await _auditService.GetAuditLogsByActionAsync(actionType);
                return Ok(actionLogs);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("GetAuditLogsByAction", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<AuditStatistics>> GetAuditStatistics(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            try
            {
                var statistics = await _auditService.GetAuditStatisticsAsync(fromDate, toDate);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("GetAuditStatistics", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportAuditLogs([FromBody] ExportRequest request)
        {
            try
            {
                if (!request.StartDate.HasValue || !request.EndDate.HasValue)
                {
                    return BadRequest("Start date and end date are required");
                }

                var exportData = await _auditService.ExportAuditLogsAsync(
                    request.StartDate.Value,
                    request.EndDate.Value,
                    request.Format);

                await _auditService.LogActionAsync("ExportAuditLogs", "Exported audit logs", User.Identity?.Name ?? "System");

                return File(exportData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "audit_logs.xlsx");
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ExportAuditLogs", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        // Additional action methods with proper logging
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<RIMSAuditTrail>>> GetRecentAuditLogs()
        {
            try
            {
                var recentLogs = await _auditService.GetRecentAuditLogsAsync();
                await _auditService.LogActionAsync("GetRecentAuditLogs", "Retrieved recent audit logs", User.Identity?.Name ?? "System");
                return Ok(recentLogs);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("GetRecentAuditLogs", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("cleanup")]
        public async Task<IActionResult> CleanupOldAuditLogs([FromQuery] int daysToKeep = 365)
        {
            try
            {
                var deletedCount = await _auditService.CleanupOldAuditLogsAsync(daysToKeep);
                await _auditService.LogActionAsync("CleanupOldAuditLogs", $"Cleaned up {deletedCount} old audit logs", User.Identity?.Name ?? "System");

                return Ok(new { message = $"Successfully deleted {deletedCount} old audit logs" });
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CleanupOldAuditLogs", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    // If ExportRequest is not defined elsewhere, add it here
    public class ExportRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Format { get; set; } = "excel";
    }
}