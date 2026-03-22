using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPostOffice.Data;
using SmartPostOffice.Models;
using SmartPostOffice.Services;

namespace SmartPostOffice.Controllers
{
    public class TelimailController : Controller
    {
        private readonly PostOfficeDbContext _db;
        private readonly IPaymentService _payment;

        public TelimailController(PostOfficeDbContext db, IPaymentService payment)
        { _db = db; _payment = payment; }


        public IActionResult Index() => View();


        public IActionResult Send() => View(new TelimailMessage());


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Send(TelimailMessage model)
        {
            ModelState.Remove(nameof(TelimailMessage.TelimailReference));
            ModelState.Remove(nameof(TelimailMessage.PaymentStatus));

            if (!ModelState.IsValid) return View(model);

            model.TelimailReference = $"TLM-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
            model.PaymentStatus = "Pending";
            _db.TelimailMessages.Add(model);
            _db.SaveChanges();

            return RedirectToAction("Pay", new { id = model.Id });
        }


        public IActionResult Pay(int id)
        {
            var msg = _db.TelimailMessages.Find(id);
            if (msg == null) return NotFound();
            ViewBag.Amount = msg.CalculateTotal();
            ViewBag.Reference = msg.TelimailReference;
            ViewBag.ServiceLabel = msg.GetServiceLabel();
            ViewBag.BookingId = id;
            ViewBag.Module = "Telimail";
            return View("~/Views/Payment/Pay.cshtml");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int bookingId, string cardNumber,
                                              string expiryDate, string cvv,
                                              string cardHolderName)
        {
            var msg = _db.TelimailMessages.Find(bookingId);
            if (msg == null) return NotFound();

            var result = await _payment.ProcessPaymentAsync(
                msg.CalculateTotal(), cardNumber, expiryDate, cvv, cardHolderName);

            if (!result.Success)
            {
                TempData["PaymentError"] = result.ErrorMessage;
                return RedirectToAction("Pay", new { id = bookingId });
            }

            msg.PaymentStatus = "Paid";
            msg.PaymentReference = result.Reference;
            msg.PaidAt = DateTime.Now;
            _db.SaveChanges();

            var txn = new Transaction
            {
                ServiceRequestId = null,
                TrackingNumber = msg.TelimailReference,
                ActualWeightGrams = 0,
                FinalCharge = msg.CalculateTotal(),
                PaymentMethod = "Online",
                ProcessedByOfficerId = 0,
                ReceiptNumber = $"ONL-{DateTime.Now.Year}-" + Guid.NewGuid().ToString("N")[..6].ToUpper(),
                TransactionDate = DateTime.Now
            };
            _db.Transactions.Add(txn);
            _db.SaveChanges();

            _db.CashBookEntries.Add(new TelimailEntry
            {
                TransactionId = txn.Id,
                Amount = msg.CalculateTotal(),
                PaymentMethod = "Online",
                EntryType = "CREDIT",
                EntryDate = DateTime.Now
            });
            _db.SaveChanges();

            TempData["SuccessMessage"] = $"Telemail submitted! Reference: {msg.TelimailReference}";
            return RedirectToAction("Confirmation", new { id = msg.Id });
        }


        public IActionResult Confirmation(int id)
        {
            var msg = _db.TelimailMessages.Find(id);
            if (msg == null) return NotFound();
            return View(msg);
        }


        [Authorize(AuthenticationSchemes = "OfficerCookies")]
        public IActionResult Inbox()
        {
            var msgs = _db.TelimailMessages
                          .Where(m => m.PaymentStatus == "Paid")
                          .OrderByDescending(m => m.CreatedAt)
                          .ToList();
            return View(msgs);
        }

        [Authorize(AuthenticationSchemes = "OfficerCookies")]
        public IActionResult Detail(int id)
        {
            var msg = _db.TelimailMessages.Find(id);
            if (msg == null) return NotFound();
            return View(msg);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "OfficerCookies")]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            var msg = _db.TelimailMessages.Find(id);
            if (msg == null) return NotFound();

            msg.MessageStatus = newStatus;
            msg.HandledBy = User.Identity?.Name;
            _db.SaveChanges();

            TempData["SuccessMessage"] = $"Telemail {msg.TelimailReference} marked as {newStatus}.";
            return RedirectToAction("Inbox");
        }
    }
}