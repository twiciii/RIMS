using Microsoft.AspNetCore.Mvc;
using RIMS.Services.Interfaces;
using RIMS.Models.ViewModels;
using System.Security.Claims;

namespace RIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IDController : ControllerBase
    {
        private readonly IResidentService _residentService;
        private readonly IDocumentService _documentService;
        private readonly IAuditService _auditService;

        public IDController(IResidentService residentService, IDocumentService documentService, IAuditService auditService)
        {
            _residentService = residentService;
            _documentService = documentService;
            _auditService = auditService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestID([FromBody] IDRequestViewModel request)
        {
            try
            {
                // Map to service DTO
                var serviceRequest = new IDRequest
                {
                    ResidentId = request.ResidentId,
                    Purpose = request.Purpose ?? string.Empty
                };

                var applicationId = await _documentService.CreateIDApplicationAsync(serviceRequest);
                await _auditService.LogActionAsync("IDController.RequestID", $"Created ID request for resident ID: {request.ResidentId}", GetCurrentUsername());

                return Ok(new { applicationId, message = "ID request submitted successfully" });
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("IDController.RequestID", ex.Message, GetCurrentUsername());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("applications")]
        public async Task<ActionResult<IEnumerable<IDApplicationViewModel>>> GetIDApplications()
        {
            try
            {
                var applications = await _documentService.GetIDApplicationsAsync();

                // Map to view models
                var viewModels = applications.Select(app => new IDApplicationViewModel
                {
                    Id = app.Id,
                    ResidentName = app.ResidentName,
                    Status = app.Status,
                    ApplicationDate = app.ApplicationDate,
                    IDType = "Resident ID",
                    IDNumber = $"ID{app.Id:00000}",
                    ExpirationDate = DateTime.Now.AddYears(2)
                });

                await _auditService.LogActionAsync("IDController.GetIDApplications", "Retrieved all ID applications", GetCurrentUsername());
                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("IDController.GetIDApplications", ex.Message, GetCurrentUsername());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("generate/{residentId}")]
        public async Task<IActionResult> GenerateIDCard(int residentId)
        {
            try
            {
                var idCardBytes = await _documentService.GenerateIDCardAsync(residentId);
                await _auditService.LogActionAsync("IDController.GenerateIDCard", $"Generated ID card for resident ID: {residentId}", GetCurrentUsername());

                return File(idCardBytes, "image/png", $"id_card_{residentId}.png");
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("IDController.GenerateIDCard", ex.Message, GetCurrentUsername());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("approve/{applicationId}")]
        public async Task<IActionResult> ApproveIDApplication(int applicationId)
        {
            try
            {
                var result = await _documentService.ApproveIDApplicationAsync(applicationId);
                if (!result)
                {
                    return BadRequest(new { message = "Failed to approve ID application" });
                }

                await _auditService.LogActionAsync("IDController.ApproveIDApplication", $"Approved ID application ID: {applicationId}", GetCurrentUsername());

                return Ok(new { message = "ID application approved successfully" });
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("IDController.ApproveIDApplication", ex.Message, GetCurrentUsername());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("status/{residentId}")]
        public async Task<ActionResult<IDStatusViewModel>> GetIDStatus(int residentId)
        {
            try
            {
                var status = await _documentService.GetIDStatusAsync(residentId);

                // Map service IDStatus to view model
                var statusViewModel = new IDStatusViewModel
                {
                    HasID = status.HasID,
                    Status = status.Status,
                    IDNumber = status.HasID ? $"ID{residentId:00000}" : string.Empty,
                    IssueDate = status.HasID ? DateTime.Now.AddDays(-30) : null,
                    ExpirationDate = status.HasID ? DateTime.Now.AddYears(2) : null,
                    IsActive = status.HasID && status.Status?.ToLower() == "active"
                };

                await _auditService.LogActionAsync("IDController.GetIDStatus", $"Retrieved ID status for resident ID: {residentId}", GetCurrentUsername());

                return Ok(statusViewModel);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("IDController.GetIDStatus", ex.Message, GetCurrentUsername());
                return StatusCode(500, "Internal server error");
            }
        }

        private string GetCurrentUsername()
        {
            return User?.Identity?.Name ?? "System";
        }
    }
}