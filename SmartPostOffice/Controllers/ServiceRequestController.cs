
// Developer 2 — Responsible for:
// - Displaying the service request form (GET)
// - Displaying the confirmation page (GET)

using Microsoft.AspNetCore.Mvc;
using SmartPostOffice.Data;
using SmartPostOffice.Models;
using SmartPostOffice.Models.Enums;

namespace SmartPostOffice.Controllers
{
    public class ServiceRequestController : Controller
    {
        // Shared database context — used by both Dev 2 and Dev 3 actions
        private readonly PostOfficeDbContext _db;
        public ServiceRequestController(PostOfficeDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Create(string serviceType = "OrdinaryLetter")
        {
            // Try to parse the serviceType string into our ServiceType enum
            if (!Enum.TryParse<ServiceType>(serviceType, out var parsedType))
                parsedType = ServiceType.OrdinaryLetter;

            var model = new ServiceRequest
            {
                ServiceType = parsedType
            };

            ViewBag.ServiceType = parsedType.ToString();

            return View(model);
        }
        // DEV 3 — Task: S1-T10 / BE_US_04
        // Receives the submitted form, validates it, generates a reference
        // number, saves to the database, redirects to confirmation page.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest model)
        {
            // Check all [Required], [Phone], [Range] annotations on the model
            // If anything fails validation, return the form with error messages
            if (!ModelState.IsValid)
            {
                ViewBag.ServiceType = model.ServiceType.ToString();
                return View(model);
            }

            // Generate a unique reference number for this request
            // Format: SPO-YYYY-XXXXXX (e.g. SPO-2024-A3F7C2)
            model.ReferenceNumber = GenerateReferenceNumber();

            // Assign PENDING status and current timestamp before saving
            model.Status = RequestStatus.PENDING;
            model.CreatedAt = DateTime.Now;

            // Save the completed request to Azure SQL Database
            _db.ServiceRequests.Add(model);
            await _db.SaveChangesAsync();

            // Redirect to the confirmation page with the reference number
            return RedirectToAction("Confirmation", new { refNum = model.ReferenceNumber });
        }

        [HttpGet]
        public IActionResult Confirmation(string refNum)
        {
            if (string.IsNullOrEmpty(refNum))
                return RedirectToAction("Create");

            return View(model: refNum);
        }

        private string GenerateReferenceNumber()
        {
            string year = DateTime.Now.Year.ToString();
            string uniquePart = Guid.NewGuid().ToString("N")[..6].ToUpper();
            return $"SPO-{year}-{uniquePart}";
            // Example output: SPO-2024-A3F7C2
        }

    }
}