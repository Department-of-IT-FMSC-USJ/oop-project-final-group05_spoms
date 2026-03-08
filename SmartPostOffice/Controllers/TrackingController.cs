using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SmartPostOffice.Data;
using SmartPostOffice.Models;
using SmartPostOffice.Models.Enums;

public class TrackingController : Controller
{
    private readonly PostOfficeDbContext _db;
    public TrackingController(PostOfficeDbContext db) { _db = db; }

    // GET /Track — PUBLIC
    public IActionResult Track() => View();

    // POST /Track — accepts both reference number OR tracking number
    [HttpPost]
    public IActionResult Track(string trackingInput)
    {
        ServiceRequest? request = null;

        request = _db.ServiceRequests
            .FirstOrDefault(r => r.ReferenceNumber == trackingInput);

        if (request == null)
        {
            var txn = _db.Transactions
                .Include(t => t.ServiceRequest)
                .FirstOrDefault(t => t.TrackingNumber == trackingInput);
            request = txn?.ServiceRequest;
        }

        if (request == null)
        {
            ViewBag.Error = "No record found. Check the number and try again.";
            return View();
        }

        var history = _db.TrackingHistory
            .Where(h => h.ServiceRequestId == request.Id)
            .OrderBy(h => h.UpdatedAt)
            .ToList();

        ViewBag.Request = request;
        ViewBag.History = history;
        return View();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "OfficerCookies")]
    public async Task<IActionResult> UpdateStatus(
        int serviceRequestId, RequestStatus newStatus, string? notes)
    {
        var request = await _db.ServiceRequests.FindAsync(serviceRequestId);
        if (request == null) return NotFound();

        int officerId = int.Parse(User.FindFirst("OfficerId")!.Value);
        request.Status = newStatus;

        _db.TrackingHistory.Add(new TrackingHistory
        {
            ServiceRequestId = serviceRequestId,
            Status = newStatus,
            UpdatedByOfficerId = officerId,
            Notes = notes
        });
        await _db.SaveChangesAsync();
        return RedirectToAction("Track");
    }
}
