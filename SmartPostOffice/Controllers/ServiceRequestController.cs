
// Developer 2 is responsible for the following actions in this controller:
// - Displaying the service request form (GET)
// - Displaying the confirmation page (GET)
// Dev 3 will add the POST action and reference number generator below this.

using Microsoft.AspNetCore.Mvc;
using SmartPostOffice.Data;
using SmartPostOffice.Models;
using SmartPostOffice.Models.Enums;

namespace SmartPostOffice.Controllers
{
    public class ServiceRequestController : Controller
    {
        // Shared database context used by both Dev 2 and Dev 3 actions
        private readonly PostOfficeDbContext _db;

      
        public ServiceRequestController(PostOfficeDbContext db)
        {
            _db = db;
        }

        // DEV 2 — Task: S1-T10 / BE_US_04
        
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

            // Pass the service type string to the View for tab highlighting
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