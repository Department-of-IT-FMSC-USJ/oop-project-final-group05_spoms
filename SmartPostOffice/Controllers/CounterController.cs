using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SmartPostOffice.Services;
using SmartPostOffice.Data;
using SmartPostOffice.Models;
using SmartPostOffice.Models.Enums;
[Authorize(AuthenticationSchemes = "OfficerCookies")]
public class CounterController : Controller
{
    private readonly PostOfficeDbContext _db;
    private readonly IPostalChargeService _chargeService;
    private readonly ICashBookRepository _cashBook;

    public CounterController(PostOfficeDbContext db, IPostalChargeService chargeService, ICashBookRepository cashBook)
    {
        _db = db;
        _chargeService = chargeService;
        _cashBook = cashBook;
    }


    public async Task<IActionResult> Dashboard()
    {
        ViewBag.PendingCount = await _db.ServiceRequests
            .CountAsync(r => r.Status == RequestStatus.PENDING);

        ViewBag.TodayCount = await _db.Transactions
            .CountAsync(t => t.TransactionDate.Date == DateTime.Today);

        ViewBag.TodayCashTotal = await _db.Transactions
    .Where(t => t.TransactionDate.Date == DateTime.Today
             && t.PaymentMethod == "Cash")
    .SumAsync(t => (decimal?)t.FinalCharge) ?? 0m;

        ViewBag.TodayOnlineTotal = await _db.Transactions
            .Where(t => t.TransactionDate.Date == DateTime.Today
                     && t.PaymentMethod == "Online")
                             .SumAsync(t => (decimal?)t.FinalCharge) ?? 0m;

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
        var registerEntry = CreateRegisterEntry(request.ServiceType, charge, paymentMethod, transaction.Id);
        await _cashBook.AddEntryAsync(registerEntry);

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
    [HttpGet]
    public IActionResult WalkIn() => View(new ServiceRequest());

    // WALK-IN process
    [HttpPost]
    public async Task<IActionResult> WalkIn(ServiceRequest model, decimal actualWeightGrams, string paymentMethod)
    {   
        ModelState.Remove("EstimatedWeightGrams");
        model.EstimatedWeightGrams = actualWeightGrams;
        if (!ModelState.IsValid) return View(model);

        int officerId = int.Parse(User.FindFirst("OfficerId")!.Value);

        string refNum = $"SPO-{DateTime.Now.Year}-" + Guid.NewGuid().ToString("N")[..6].ToUpper();
        string trackingNum = $"TRK-{DateTime.Now.Year}-" + Guid.NewGuid().ToString("N")[..8].ToUpper();
        string receiptNum = $"RCP-{DateTime.Now.Year}-" + Guid.NewGuid().ToString("N")[..6].ToUpper();

        model.ReferenceNumber = refNum;
        model.Status = RequestStatus.ACCEPTED;
        model.CreatedAt = DateTime.Now;
        _db.ServiceRequests.Add(model);
        await _db.SaveChangesAsync();

        decimal charge = _chargeService.CalculateCharge(model.ServiceType, actualWeightGrams);
        if (model.ServiceType == ServiceType.COD && model.SellerProfitAmount.HasValue)
            {
                model.TotalCODAmount = charge + model.SellerProfitAmount.Value;
            }
        var transaction = new Transaction
        {
            ServiceRequestId = model.Id,
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
            ServiceRequestId = model.Id,
            Status = RequestStatus.ACCEPTED,
            UpdatedByOfficerId = officerId
        });
        await _db.SaveChangesAsync();
        var registerEntry = CreateRegisterEntry(model.ServiceType, charge, paymentMethod, transaction.Id);
        await _cashBook.AddEntryAsync(registerEntry);

        return RedirectToAction("Receipt", new { transactionId = transaction.Id });
    }
    private CashBookEntry CreateRegisterEntry(
    ServiceType serviceType, decimal amount, string paymentMethod, int transactionId)
    {
        CashBookEntry entry = serviceType switch
        {
            ServiceType.OrdinaryLetter => new OrdinaryLetterEntry(),
            ServiceType.RegisteredMail => new RegisteredMailEntry(),
            ServiceType.SpeedPost => new SpeedPostEntry(),
            ServiceType.OrdinaryParcel => new OrdinaryParcelEntry(),
            ServiceType.COD => new CODEntry(),
            _ => throw new ArgumentOutOfRangeException(nameof(serviceType))
        };

        entry.TransactionId = transactionId;
        entry.Amount = amount;
        entry.PaymentMethod = paymentMethod;
        entry.EntryDate = DateTime.Now;
        entry.EntryType = "CREDIT";
        return entry;
    }


}
