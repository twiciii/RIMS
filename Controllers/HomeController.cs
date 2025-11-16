using Microsoft.AspNetCore.Mvc;
using RIMS.Models;
using RIMS.Services;
using RIMS.Services.Interfaces;
using System.Diagnostics;

namespace RIMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IResidentService _residentService;
        private readonly IDocumentService _documentService;
        private readonly IAuditService _auditService;

        public HomeController(
            IResidentService residentService,
            IDocumentService documentService,
            IAuditService auditService)
        {
            _residentService = residentService;
            _documentService = documentService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get dashboard statistics
                var residentCount = await _residentService.GetResidentCountAsync();
                var pendingApplications = await _documentService.GetPendingApplicationsCountAsync();
                var completedApplications = await _documentService.GetCompletedApplicationsCountAsync();

                ViewBag.ResidentCount = residentCount;
                ViewBag.PendingApplications = pendingApplications;
                ViewBag.CompletedApplications = completedApplications;

                return View();
            }
            catch (Exception ex)
            {
                // Log error
                await _auditService.LogErrorAsync("HomeController.Index", ex.Message, User.Identity?.Name ?? "Anonymous");
                return View("Error");
            }
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}