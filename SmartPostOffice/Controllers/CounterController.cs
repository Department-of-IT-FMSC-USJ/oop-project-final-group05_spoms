using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SmartPostOffice.Services;

[Authorize(AuthenticationSchemes = "OfficerCookies")]
public class CounterController : Controller
{
    private readonly PostOfficeDbContext _db;
    private readonly IPostalChargeService _chargeService;

        public CounterController(PostOfficeDbContext db, IPostalChargeService chargeService)
    { _db = db; _chargeService = chargeService; }

    public IActionResult Dashboard()
    {
        ViewBag.PendingCount = _db.ServiceRequests
            .Count(r => r.Status == RequestStatus.PENDING);
        ViewBag.TodayCount = _db.Transactions
            .Count(t => t.TransactionDate.Date == DateTime.Today);
        return View();
    }

 
    [HttpGet]
    public IActionResult ProcessRequest() => View();

    [HttpPost]
    public IActionResult ProcessRequest(string referenceNumber)
    {
        var request = _db.ServiceRequests
            .FirstOrDefault(r => r.ReferenceNumber == referenceNumber
                             && r.Status == RequestStatus.PENDING);

        if (request == null)
        {
            ViewBag.Error = "Request not found or already processed.";
            return View();
        }
        return View("ConfirmTransaction", request);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmTransaction(
        int serviceRequestId, decimal actualWeightGrams, string paymentMethod)
    {
        var request = await _db.ServiceRequests.FindAsync(serviceRequestId);
        if (request == null) return NotFound();

        int officerId = int.Parse(User.FindFirst("OfficerId")!.Value);

        decimal charge = _chargeService.CalculateCharge(request.ServiceType, actualWeightGrams);
        if (request.ServiceType == ServiceType.COD && request.SellerProfitAmount.HasValue)
        {

            request.TotalCODAmount = charge + request.SellerProfitAmount.Value;
        }
        _db.ServiceRequests.Update(request);

        string trackingNum = $"TRK-{DateTime.Now.Year}-" + Guid.NewGuid().ToString("N")[..8].ToUpper();
        string receiptNum = $"RCP-{DateTime.Now.Year}-" + Guid.NewGuid().ToString("N")[..6].ToUpper();
        request.Status = RequestStatus.ACCEPTED;

        if (request.ServiceType == ServiceType.COD && request.SellerProfitAmount.HasValue)
            request.TotalCODAmount = charge + request.SellerProfitAmount.Value;

        _db.ServiceRequests.Update(request);

        var transaction = new Transaction
        {
            ServiceRequestId = serviceRequestId,
            TrackingNumber = trackingNum,
            ActualWeightGrams = actualWeightGrams,
            FinalCharge = charge,
            PaymentMethod = paymentMethod,
            ProcessedByOfficerId = officerId,
            ReceiptNumber = receiptNum
        };
        _db.Transactions.Add(transaction);

        _db.TrackingHistory.Add(new TrackingHistory
        {
            ServiceRequestId = serviceRequestId,
            Status = RequestStatus.ACCEPTED,
            UpdatedByOfficerId = officerId
        });

        await _db.SaveChangesAsync();
        return RedirectToAction("Receipt", new { transactionId = transaction.Id });
    }

    public IActionResult Receipt(int transactionId)
    {
        var transaction = _db.Transactions
            .Include(t => t.ServiceRequest) 
            .FirstOrDefault(t => t.Id == transactionId);
        if (transaction == null) return NotFound();
        return View(transaction);
    }
}
