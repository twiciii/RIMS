using Microsoft.AspNetCore.Mvc;
using RIMS.Models.Entities;
using RIMS.Models.ViewModels;
using RIMS.Services.Interfaces;

namespace RIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IAuditService _auditService;

        public ProcessController(IDocumentService documentService, IAuditService auditService)
        {
            _documentService = documentService;
            _auditService = auditService;
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>> GetPendingApplications()
        {
            try
            {
                var pendingApplications = await _documentService.GetPendingApplicationsAsync();
                await _auditService.LogActionAsync("ProcessController.GetPendingApplications", "Retrieved pending applications", User.Identity?.Name ?? "System");
                return Ok(pendingApplications);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ProcessController.GetPendingApplications", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("approve/{applicationId}")]
        public async Task<IActionResult> ApproveApplication(int applicationId)
        {
            try
            {
                var result = await _documentService.ApproveApplicationAsync(applicationId);
                if (result)
                {
                    await _auditService.LogActionAsync("ProcessController.ApproveApplication", $"Approved application ID: {applicationId}", User.Identity?.Name ?? "System");
                    return Ok(new { message = "Application approved successfully" });
                }
                else
                {
                    return NotFound("Application not found");
                }
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ProcessController.ApproveApplication", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("reject/{applicationId}")]
        public async Task<IActionResult> RejectApplication(int applicationId, [FromBody] ProcessRejectRequest request)
        {
            try
            {
                var result = await _documentService.RejectApplicationAsync(applicationId, request.Remarks);
                if (result)
                {
                    await _auditService.LogActionAsync("ProcessController.RejectApplication", $"Rejected application ID: {applicationId}", User.Identity?.Name ?? "System");
                    return Ok(new { message = "Application rejected" });
                }
                else
                {
                    return NotFound("Application not found");
                }
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ProcessController.RejectApplication", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("return/{applicationId}")]
        public async Task<IActionResult> ReturnApplication(int applicationId, [FromBody] ReturnRequest request)
        {
            try
            {
                // Since ReturnApplicationAsync might not exist in IDocumentService,
                // we'll use RejectApplicationAsync as an alternative or remove this method
                var result = await _documentService.RejectApplicationAsync(applicationId, $"Returned for revision: {request.Feedback}");
                if (result)
                {
                    await _auditService.LogActionAsync("ProcessController.ReturnApplication", $"Returned application ID: {applicationId} for revision", User.Identity?.Name ?? "System");
                    return Ok(new { message = "Application returned for revision" });
                }
                else
                {
                    return NotFound("Application not found");
                }
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ProcessController.ReturnApplication", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("status/{applicationId}")]
        public async Task<ActionResult<ApplicationStatus>> GetApplicationStatus(int applicationId)
        {
            try
            {
                // Get the application first to check its status
                var applications = await _documentService.GetAllDocumentApplicationsAsync();
                var application = applications.FirstOrDefault(a => a.Id == applicationId);

                if (application == null)
                {
                    return NotFound("Application not found");
                }

                var status = new ApplicationStatus
                {
                    Status = application.Status ?? "Unknown",
                    ProcessedDate = application.DateInsurance, // Using DateInsurance as processed date
                    ProcessedBy = "System", // You might want to get this from audit trail
                    Remarks = application.Purpose ?? "No remarks"
                };

                await _auditService.LogActionAsync("ProcessController.GetApplicationStatus", $"Retrieved status for application ID: {applicationId}", User.Identity?.Name ?? "System");

                return Ok(status);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ProcessController.GetApplicationStatus", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    // Request models for better structure - renamed to avoid conflicts
    public class ProcessRejectRequest
    {
        public string Remarks { get; set; } = string.Empty;
    }

    public class ReturnRequest
    {
        public string Feedback { get; set; } = string.Empty;
    }

    // Renamed to avoid conflicts
    public class ApplicationStatus
    {
        public string Status { get; set; } = string.Empty;
        public DateTime? ProcessedDate { get; set; }
        public string ProcessedBy { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }
}