using Microsoft.AspNetCore.Mvc;
using RIMS.Models.Entities;
using RIMS.Models.ViewModels;
using RIMS.Services.Interfaces;

namespace RIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IAuditService _auditService;

        public DocumentsController(IDocumentService documentService, IAuditService auditService)
        {
            _documentService = documentService;
            _auditService = auditService;
        }

        // Document CRUD operations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RIMSDocument>>> GetDocuments()
        {
            try
            {
                var documents = await _documentService.GetAllDocumentsAsync();
                await _auditService.LogActionAsync("DocumentsController.GetDocuments", "Retrieved all documents", User.Identity?.Name ?? "System");
                return Ok(documents);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.GetDocuments", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RIMSDocument>> GetDocument(int id)
        {
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(id);

                if (document == null)
                {
                    return NotFound();
                }

                await _auditService.LogActionAsync("DocumentsController.GetDocument", $"Retrieved document ID: {id}", User.Identity?.Name ?? "System");
                return Ok(document);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.GetDocument", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RIMSDocument>> PostDocument(RIMSDocument document)
        {
            try
            {
                await _documentService.CreateDocumentAsync(document);
                await _auditService.LogActionAsync("DocumentsController.PostDocument", $"Created new document: {document.DocumentName}", User.Identity?.Name ?? "System");

                return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.PostDocument", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument(int id, RIMSDocument document)
        {
            try
            {
                if (id != document.Id)
                {
                    return BadRequest();
                }

                await _documentService.UpdateDocumentAsync(document);
                await _auditService.LogActionAsync("DocumentsController.PutDocument", $"Updated document ID: {id}", User.Identity?.Name ?? "System");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.PutDocument", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                await _documentService.DeleteDocumentAsync(id);
                await _auditService.LogActionAsync("DocumentsController.DeleteDocument", $"Deleted document ID: {id}", User.Identity?.Name ?? "System");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.DeleteDocument", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        // Document Application operations
        [HttpGet("applications")]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>> GetDocumentApplications()
        {
            try
            {
                var applications = await _documentService.GetAllDocumentApplicationsAsync();
                await _auditService.LogActionAsync("DocumentsController.GetDocumentApplications", "Retrieved all document applications", User.Identity?.Name ?? "System");
                return Ok(applications);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.GetDocumentApplications", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("applications/pending")]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>> GetPendingApplications()
        {
            try
            {
                var applications = await _documentService.GetPendingApplicationsAsync();
                await _auditService.LogActionAsync("DocumentsController.GetPendingApplications", "Retrieved pending document applications", User.Identity?.Name ?? "System");
                return Ok(applications);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.GetPendingApplications", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("applications/certificates/pending")]
        public async Task<ActionResult<IEnumerable<RIMSDocumentApplication>>> GetPendingCertificateApplications()
        {
            try
            {
                var applications = await _documentService.GetPendingCertificateApplicationsAsync();
                await _auditService.LogActionAsync("DocumentsController.GetPendingCertificateApplications", "Retrieved pending certificate applications", User.Identity?.Name ?? "System");
                return Ok(applications);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.GetPendingCertificateApplications", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("applications")]
        public async Task<ActionResult<RIMSDocumentApplication>> PostDocumentApplication(RIMSDocumentApplication application)
        {
            try
            {
                await _documentService.CreateDocumentApplicationAsync(application);
                await _auditService.LogActionAsync("DocumentsController.PostDocumentApplication", $"Created document application for resident ID: {application.FK_ResidentId}", User.Identity?.Name ?? "System");

                return CreatedAtAction(nameof(GetDocumentApplications), new { id = application.Id }, application);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.PostDocumentApplication", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("applications/{id}/approve")]
        public async Task<IActionResult> ApproveApplication(int id)
        {
            try
            {
                var result = await _documentService.ApproveApplicationAsync(id);
                if (result)
                {
                    await _auditService.LogActionAsync("DocumentsController.ApproveApplication", $"Approved document application ID: {id}", User.Identity?.Name ?? "System");
                    return Ok(new { message = "Application approved successfully" });
                }
                else
                {
                    return NotFound("Application not found");
                }
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.ApproveApplication", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("applications/{id}/reject")]
        public async Task<IActionResult> RejectApplication(int id, [FromBody] DocumentRejectRequest request)
        {
            try
            {
                var result = await _documentService.RejectApplicationAsync(id, request.Remarks);
                if (result)
                {
                    await _auditService.LogActionAsync("DocumentsController.RejectApplication", $"Rejected document application ID: {id}", User.Identity?.Name ?? "System");
                    return Ok(new { message = "Application rejected successfully" });
                }
                else
                {
                    return NotFound("Application not found");
                }
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.RejectApplication", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("certificates/{id}/reject")]
        public async Task<IActionResult> RejectCertificate(int id, [FromBody] DocumentRejectRequest request)
        {
            try
            {
                var result = await _documentService.RejectCertificateAsync(id, request.Remarks);
                if (result)
                {
                    await _auditService.LogActionAsync("DocumentsController.RejectCertificate", $"Rejected certificate application ID: {id}", User.Identity?.Name ?? "System");
                    return Ok(new { message = "Certificate application rejected successfully" });
                }
                else
                {
                    return NotFound("Certificate application not found");
                }
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("DocumentsController.RejectCertificate", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class DocumentRejectRequest
    {
        public string Remarks { get; set; } = string.Empty;
    }
}