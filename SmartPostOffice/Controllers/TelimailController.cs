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
        private readonly IPaymentService     _payment;

        public TelimailController(PostOfficeDbContext db, IPaymentService payment)
        { _db = db; _payment = payment; }

       
        public IActionResult Index() => View();

       
        public IActionResult Send() => View(new TelimailMessage());

        
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Send(TelimailMessage model)
        {
            if (!ModelState.IsValid) return View(model);

            model.TelimailReference = $"TLM-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000,9999)}";
            model.PaymentStatus     = "Pending";
            _db.TelimailMessages.Add(model);
            _db.SaveChanges();

            return RedirectToAction("Pay", new { id = model.Id });
        }

     
        public IActionResult Pay(int id)
        {
            var msg = _db.TelimailMessages.Find(id);
            if (msg == null) return NotFound();
            ViewBag.Amount       = msg.CalculateTotal();
            ViewBag.Reference    = msg.TelimailReference;
            ViewBag.ServiceLabel = msg.GetServiceLabel();
            ViewBag.BookingId    = id;
            ViewBag.Module       = "Telemail";
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

            msg.PaymentStatus    = "Paid";
            msg.PaymentReference = result.Reference;
            msg.PaidAt           = DateTime.Now;
            _db.SaveChanges();

            _db.CashBookEntries.Add(new TelimailEntry
            {
                TransactionId = null,
                Amount        = msg.CalculateTotal(),
                PaymentMethod = "Online",
                EntryType     = "CREDIT",
                EntryDate     = DateTime.Now
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
    }
}
