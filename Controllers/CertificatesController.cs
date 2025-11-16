using Microsoft.AspNetCore.Mvc;
using RIMS.Models.Entities;
using RIMS.Services;
using RIMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IAuditService _auditService;

        public CertificatesController(IDocumentService documentService, IAuditService auditService)
        {
            _documentService = documentService;
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>> GetCertificateApplications()
        {
            try
            {
                var certificates = await _documentService.GetCertificateApplicationsAsync();
                await _auditService.LogActionAsync("CertificatesController.GetCertificateApplications", "Retrieved all certificate applications", User.Identity?.Name ?? "System");
                return Ok(certificates);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CertificatesController.GetCertificateApplications", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>> GetPendingCertificates()
        {
            try
            {
                var pendingCertificates = await _documentService.GetPendingCertificateApplicationsAsync();
                await _auditService.LogActionAsync("CertificatesController.GetPendingCertificates", "Retrieved pending certificate applications", User.Identity?.Name ?? "System");
                return Ok(pendingCertificates);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CertificatesController.GetPendingCertificates", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("issue/{applicationId}")]
        public async Task<IActionResult> IssueCertificate(int applicationId)
        {
            try
            {
                var result = await _documentService.IssueCertificateAsync(applicationId);
                if (!result)
                {
                    await _auditService.LogErrorAsync("CertificatesController.IssueCertificate", $"Failed to issue certificate for application ID: {applicationId}", User.Identity?.Name ?? "System");
                    return BadRequest(new { message = "Failed to issue certificate" });
                }

                await _auditService.LogActionAsync("CertificatesController.IssueCertificate", $"Issued certificate for application ID: {applicationId}", User.Identity?.Name ?? "System");
                return Ok(new { message = "Certificate issued successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                await _auditService.LogErrorAsync("CertificatesController.IssueCertificate", ex.Message, User.Identity?.Name ?? "System");
                return NotFound(new { message = "Application not found" });
            }
            catch (InvalidOperationException ex)
            {
                await _auditService.LogErrorAsync("CertificatesController.IssueCertificate", ex.Message, User.Identity?.Name ?? "System");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CertificatesController.IssueCertificate", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("reject/{applicationId}")]
        public async Task<IActionResult> RejectCertificate(int applicationId, [FromBody] string reason)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reason))
                {
                    return BadRequest(new { message = "Rejection reason is required" });
                }

                var result = await _documentService.RejectCertificateAsync(applicationId, reason);
                if (!result)
                {
                    await _auditService.LogErrorAsync("CertificatesController.RejectCertificate", $"Failed to reject certificate for application ID: {applicationId}", User.Identity?.Name ?? "System");
                    return BadRequest(new { message = "Failed to reject certificate application" });
                }

                await _auditService.LogActionAsync("CertificatesController.RejectCertificate", $"Rejected certificate application ID: {applicationId}. Reason: {reason}", User.Identity?.Name ?? "System");
                return Ok(new { message = "Certificate application rejected" });
            }
            catch (KeyNotFoundException ex)
            {
                await _auditService.LogErrorAsync("CertificatesController.RejectCertificate", ex.Message, User.Identity?.Name ?? "System");
                return NotFound(new { message = "Application not found" });
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CertificatesController.RejectCertificate", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("generate/{applicationId}")]
        public async Task<IActionResult> GenerateCertificate(int applicationId)
        {
            try
            {
                var certificateBytes = await _documentService.GenerateCertificatePdfAsync(applicationId);
                if (certificateBytes == null || certificateBytes.Length == 0)
                {
                    await _auditService.LogErrorAsync("CertificatesController.GenerateCertificate", $"Failed to generate PDF for application ID: {applicationId}", User.Identity?.Name ?? "System");
                    return BadRequest(new { message = "Failed to generate certificate PDF" });
                }

                await _auditService.LogActionAsync("CertificatesController.GenerateCertificate", $"Generated certificate PDF for application ID: {applicationId}", User.Identity?.Name ?? "System");
                return File(certificateBytes, "application/pdf", $"certificate_{applicationId}.pdf");
            }
            catch (KeyNotFoundException ex)
            {
                await _auditService.LogErrorAsync("CertificatesController.GenerateCertificate", ex.Message, User.Identity?.Name ?? "System");
                return NotFound(new { message = "Application not found" });
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("CertificatesController.GenerateCertificate", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}