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

    // ── Officer-only: Tracking Management ──────────────────────────────────

    [Authorize(AuthenticationSchemes = "OfficerCookies")]
    public IActionResult ManageTracking()
    {
        var acceptedParcels = _db.ServiceRequests
            .Where(r => r.Status == RequestStatus.ACCEPTED)
            .OrderBy(r => r.CreatedAt)
            .ToList();

        var outForDelivery = _db.ServiceRequests
            .Where(r => r.Status == RequestStatus.OUT_FOR_DELIVERY)
            .OrderBy(r => r.CreatedAt)
            .ToList();

        ViewBag.AcceptedParcels = acceptedParcels;
        ViewBag.OutForDelivery = outForDelivery;
        return View();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "OfficerCookies")]
    public IActionResult ManageTracking(string trackingInput)
    {
        void LoadBatchLists()
        {
            ViewBag.AcceptedParcels = _db.ServiceRequests
                .Where(r => r.Status == RequestStatus.ACCEPTED)
                .OrderBy(r => r.CreatedAt).ToList();
            ViewBag.OutForDelivery = _db.ServiceRequests
                .Where(r => r.Status == RequestStatus.OUT_FOR_DELIVERY)
                .OrderBy(r => r.CreatedAt).ToList();
        }

        ServiceRequest? request = _db.ServiceRequests
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
            LoadBatchLists();
            return View();
        }

        if (request.Status == RequestStatus.PENDING)
        {
            ViewBag.Error = "This request is still PENDING. Process it at the counter first.";
            LoadBatchLists();
            return View();
        }

        if (request.Status == RequestStatus.DELIVERED)
        {
            ViewBag.Error = "This parcel is already DELIVERED. No further updates possible.";
            LoadBatchLists();
            return View();
        }

        var history = _db.TrackingHistory
            .Where(h => h.ServiceRequestId == request.Id)
            .OrderBy(h => h.UpdatedAt)
            .ToList();

        RequestStatus nextStatus = request.Status switch
        {
            RequestStatus.ACCEPTED => RequestStatus.OUT_FOR_DELIVERY,
            RequestStatus.OUT_FOR_DELIVERY => RequestStatus.DELIVERED,
            _ => RequestStatus.DELIVERED
        };

        LoadBatchLists();
        ViewBag.Request = request;
        ViewBag.History = history;
        ViewBag.NextStatus = nextStatus;
        return View();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "OfficerCookies")]
    public async Task<IActionResult> ConfirmStatusUpdate(
        int serviceRequestId, RequestStatus newStatus, string? notes)
    {
        var request = await _db.ServiceRequests.FindAsync(serviceRequestId);
        if (request == null) return NotFound();

        RequestStatus expectedNext = request.Status switch
        {
            RequestStatus.ACCEPTED => RequestStatus.OUT_FOR_DELIVERY,
            RequestStatus.OUT_FOR_DELIVERY => RequestStatus.DELIVERED,
            _ => request.Status
        };

        if (newStatus != expectedNext)
        {
            TempData["ErrorMessage"] = "Invalid status update. Status can only move forward one step.";
            return RedirectToAction("ManageTracking");
        }

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
        TempData["SuccessMessage"] = $"Tracking updated to \"{newStatus}\" successfully.";
        return RedirectToAction("ManageTracking");
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "OfficerCookies")]
    public async Task<IActionResult> BatchStatusUpdate(
        List<int> selectedIds, RequestStatus newStatus, string? notes)
    {
        if (selectedIds == null || selectedIds.Count == 0)
        {
            TempData["ErrorMessage"] = "No parcels selected.";
            return RedirectToAction("ManageTracking");
        }

        int officerId = int.Parse(User.FindFirst("OfficerId")!.Value);

        RequestStatus expectedCurrent = newStatus == RequestStatus.OUT_FOR_DELIVERY
            ? RequestStatus.ACCEPTED
            : RequestStatus.OUT_FOR_DELIVERY;

        foreach (var id in selectedIds)
        {
            var request = await _db.ServiceRequests.FindAsync(id);
            if (request == null || request.Status != expectedCurrent) continue;

            request.Status = newStatus;
            _db.TrackingHistory.Add(new TrackingHistory
            {
                ServiceRequestId = id,
                Status = newStatus,
                UpdatedByOfficerId = officerId,
                Notes = notes
            });
        }

        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = $"{selectedIds.Count} parcel(s) marked as {newStatus}.";
        return RedirectToAction("ManageTracking");
    }
}
