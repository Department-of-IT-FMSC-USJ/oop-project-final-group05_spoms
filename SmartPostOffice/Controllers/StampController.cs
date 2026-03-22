using Microsoft.AspNetCore.Mvc;
using SmartPostOffice.Data;
using SmartPostOffice.Models;
using SmartPostOffice.Services;
using Microsoft.AspNetCore.Authorization;
using SmartPostOffice.Models.Enums;

namespace SmartPostOffice.Controllers
{
    public class StampController : Controller
    {
        private readonly PostOfficeDbContext _db;
        private readonly IPaymentService _payment;

        public StampController(PostOfficeDbContext db, IPaymentService payment)
        { _db = db; _payment = payment; }

        public IActionResult Index()
            => View(StampCatalogue.All);

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Order(StampOrder model, int stampStyleId)
        {
            var style = StampCatalogue.All.FirstOrDefault(s => s.Id == stampStyleId);
            if (style == null) return BadRequest();

            model.StampStyleId = style.Id;
            model.StampStyleName = style.Name;
            model.PricePerStamp = style.PricePerStamp;
            model.ServiceCharge = StampCatalogue.FixedServiceCharge;

            if (!ModelState.IsValid) return View("Index", StampCatalogue.All);

            model.OrderReference = $"STO-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
            model.PaymentStatus = "Pending";
            _db.StampOrders.Add(model);
            _db.SaveChanges();

            return RedirectToAction("Pay", new { id = model.Id });
        }

        public IActionResult Pay(int id)
        {
            var order = _db.StampOrders.Find(id);
            if (order == null) return NotFound();
            ViewBag.Amount = order.CalculateTotal();
            ViewBag.Reference = order.OrderReference;
            ViewBag.ServiceLabel = order.GetServiceLabel();
            ViewBag.BookingId = id;
            ViewBag.Module = "Stamp";
            return View("~/Views/Payment/Pay.cshtml");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int bookingId, string cardNumber, string expiryDate, string cvv, string cardHolderName)
        {
            var order = _db.StampOrders.Find(bookingId);
            if (order == null) return NotFound();

            var result = await _payment.ProcessPaymentAsync(
                order.CalculateTotal(), cardNumber, expiryDate, cvv, cardHolderName);

            if (!result.Success)
            {
                TempData["PaymentError"] = result.ErrorMessage;
                return RedirectToAction("Pay", new { id = bookingId });
            }

            order.PaymentStatus = "Paid";
            order.PaymentReference = result.Reference;
            order.PaidAt = DateTime.Now;
            _db.SaveChanges();

            var txn = new Transaction
            {
                ServiceRequestId = null,
                TrackingNumber = order.OrderReference,
                ActualWeightGrams = 0,
                FinalCharge = order.CalculateTotal(),
                PaymentMethod = "Online",
                ProcessedByOfficerId = 0,
                ReceiptNumber = $"ONL-{DateTime.Now.Year}-" + Guid.NewGuid().ToString("N")[..6].ToUpper(),
                TransactionDate = DateTime.Now
            };
            _db.Transactions.Add(txn);
            _db.SaveChanges();


            _db.CashBookEntries.Add(new StampOrderEntry
            {
                TransactionId = txn.Id,
                Amount = order.CalculateTotal(),
                PaymentMethod = "Online",
                EntryType = "CREDIT",
                EntryDate = DateTime.Now
            });
            _db.SaveChanges();

            TempData["SuccessMessage"] = $"Order confirmed! Reference: {order.OrderReference}";
            return RedirectToAction("Confirmation", new { id = order.Id });
        }
        public IActionResult Confirmation(int id)
        {
            var order = _db.StampOrders.Find(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [Authorize(AuthenticationSchemes = "OfficerCookies")]
        public IActionResult OfficerOrders()
        {
            var orders = _db.StampOrders
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }
        [Authorize(AuthenticationSchemes = "OfficerCookies")]
        [HttpGet]
        public IActionResult UpdateStampStatus(int id)
        {
            var order = _db.StampOrders.Find(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [Authorize(AuthenticationSchemes = "OfficerCookies")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStampStatus(
            int id, StampOrderStatus newStatus, string? notes)
        {
            var order = await _db.StampOrders.FindAsync(id);
            if (order == null) return NotFound();

            int officerId = int.Parse(User.FindFirst("OfficerId")!.Value);

            order.FulfilmentStatus = newStatus;

            _db.StampTrackingHistories.Add(new StampTrackingHistory
            {
                StampOrderId       = id,
                Status             = newStatus,
                UpdatedByOfficerId = officerId,
                Notes              = notes,
                UpdatedAt          = DateTime.Now
            });

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Order {order.OrderReference} updated to {newStatus}.";

            return RedirectToAction("OfficerOrders");
        }
    }
}
