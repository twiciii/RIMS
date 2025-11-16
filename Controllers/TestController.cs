using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RIMS.Data;

namespace RIMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("database")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                return Ok(new
                {
                    message = "Database connection successful",
                    connected = canConnect,
                    database = _context.Database.GetDbConnection().Database,
                    server = _context.Database.GetDbConnection().DataSource,
                    time = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Database connection failed",
                    error = ex.Message,
                    connectionString = _context.Database.GetDbConnection().ConnectionString // Be careful with this in production
                });
            }
        }

        [HttpGet("tables")]
        public async Task<IActionResult> TestTables()
        {
            try
            {
                var residentsCount = await _context.rimsResidents.CountAsync();
                var applicationsCount = await _context.rimsDocumentApplication.CountAsync();

                return Ok(new
                {
                    residents = residentsCount,
                    applications = applicationsCount,
                    status = "Tables are accessible"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Table access failed",
                    error = ex.Message
                });
            }
        }
    }
}