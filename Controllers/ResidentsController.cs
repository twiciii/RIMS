using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RIMS.Models.Entities;
using RIMS.Services;
using RIMS.Services.Interfaces;

namespace RIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentsController : ControllerBase
    {
        private readonly IResidentService _residentService;
        private readonly IAuditService _auditService;

        public ResidentsController(IResidentService residentService, IAuditService auditService)
        {
            _residentService = residentService;
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RIMSResident>>> GetResidents()
        {
            try
            {
                var residents = await _residentService.GetAllResidentsAsync();
                await _auditService.LogActionAsync("ResidentsController.GetResidents", "Retrieved all residents", User.Identity?.Name ?? "System");
                return Ok(residents);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ResidentsController.GetResidents", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RIMSResident>> GetResident(int id)
        {
            try
            {
                var resident = await _residentService.GetResidentByIdAsync(id);

                if (resident == null)
                {
                    return NotFound();
                }

                await _auditService.LogActionAsync("ResidentsController.GetResident", $"Retrieved resident ID: {id}", User.Identity?.Name ?? "System");
                return resident;
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ResidentsController.GetResident", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RIMSResident>> PostResident(RIMSResident resident)
        {
            try
            {
                await _residentService.CreateResidentAsync(resident);
                await _auditService.LogActionAsync("ResidentsController.PostResident", $"Created new resident: {resident.FirstName} {resident.LastName}", User.Identity?.Name ?? "System");

                return CreatedAtAction(nameof(GetResident), new { id = resident.Id }, resident);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ResidentsController.PostResident", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutResident(int id, RIMSResident resident)
        {
            try
            {
                if (id != resident.Id)
                {
                    return BadRequest();
                }

                await _residentService.UpdateResidentAsync(resident);
                await _auditService.LogActionAsync("ResidentsController.PutResident", $"Updated resident ID: {id}", User.Identity?.Name ?? "System");

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ResidentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ResidentsController.PutResident", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResident(int id)
        {
            try
            {
                await _residentService.DeleteResidentAsync(id);
                await _auditService.LogActionAsync("ResidentsController.DeleteResident", $"Deleted resident ID: {id}", User.Identity?.Name ?? "System");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ResidentsController.DeleteResident", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<RIMSResident>>> SearchResidents(string searchTerm)
        {
            try
            {
                var residents = await _residentService.SearchResidentsAsync(searchTerm);
                await _auditService.LogActionAsync("ResidentsController.SearchResidents", $"Searched residents with term: {searchTerm}", User.Identity?.Name ?? "System");

                return Ok(residents);
            }
            catch (Exception ex)
            {
                await _auditService.LogErrorAsync("ResidentsController.SearchResidents", ex.Message, User.Identity?.Name ?? "System");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<bool> ResidentExists(int id)
        {
            return await _residentService.GetResidentByIdAsync(id) != null;
        }
    }
}