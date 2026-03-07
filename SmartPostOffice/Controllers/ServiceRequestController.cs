
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

        [HttpGet]
        public IActionResult Confirmation(string refNum)
        {
            if (string.IsNullOrEmpty(refNum))
                return RedirectToAction("Create");

            return View(model: refNum);
        }

       
    }
}