using Microsoft.AspNetCore.Mvc;
using SmartPostOffice.Data;
using SmartPostOffice.Models;
using SmartPostOffice.Services;

namespace SmartPostOffice.Controllers
{
    public class BungalowController : Controller
    {
        private readonly PostOfficeDbContext _db;
        private readonly IPaymentService _payment;

        public BungalowController(PostOfficeDbContext db, IPaymentService payment)
        { _db = db; _payment = payment; }

        public IActionResult Index()
            => View(BungalowCatalogue.All);
        public IActionResult Book(string locationId)
        {
            var loc = BungalowCatalogue.All.FirstOrDefault(l => l.Id == locationId);
            if (loc == null) return NotFound();
            ViewBag.Location = loc;
            return View(new BungalowBooking { Location = loc.Name });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Book(BungalowBooking model, string locationId, string roomLabel)
        {
            var loc = BungalowCatalogue.All.FirstOrDefault(l => l.Id == locationId);
            if (loc == null) return NotFound();

            var room = loc.Rooms.FirstOrDefault(r => r.RoomLabel == roomLabel)
                       ?? loc.Rooms.First();

            model.RoomRatePerNight = model.IsStaffRate ? room.StaffRate : room.OutsiderRate;
            model.ServiceCharge = room.ServiceCharge;
            model.RoomOption = room.RoomLabel;
            model.Location = loc.Name;
            model.BookingReference = $"BNG-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
            model.PaymentStatus = "Pending";

            ModelState.Remove(nameof(BungalowBooking.BookingReference));
            ModelState.Remove(nameof(BungalowBooking.PaymentStatus));
            ModelState.Remove(nameof(BungalowBooking.RoomRatePerNight));
            ModelState.Remove(nameof(BungalowBooking.Location));

            if (!ModelState.IsValid)
            {
                ViewBag.Location = loc;
                return View(model);
            }

            _db.BungalowBookings.Add(model);
            _db.SaveChanges();

            return RedirectToAction("Pay", new { id = model.Id });
        }
        public IActionResult Pay(int id)
        {
            var booking = _db.BungalowBookings.Find(id);
            if (booking == null) return NotFound();
            ViewBag.Amount = booking.CalculateTotal();
            ViewBag.Reference = booking.BookingReference;
            ViewBag.ServiceLabel = booking.GetServiceLabel();
            ViewBag.BookingId = id;
            ViewBag.Module = "Bungalow";
            return View("~/Views/Payment/Pay.cshtml");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int bookingId, string cardNumber,
                                              string expiryDate, string cvv,
                                              string cardHolderName)
        {
            var booking = _db.BungalowBookings.Find(bookingId);
            if (booking == null) return NotFound();

            var result = await _payment.ProcessPaymentAsync(
                booking.CalculateTotal(), cardNumber, expiryDate, cvv, cardHolderName);

            if (!result.Success)
            {
                TempData["PaymentError"] = result.ErrorMessage;
                return RedirectToAction("Pay", new { id = bookingId });
            }

            booking.PaymentStatus = "Paid";
            booking.PaymentReference = result.Reference;
            booking.PaidAt = DateTime.Now;
            _db.SaveChanges();

            _db.CashBookEntries.Add(new BungalowEntry
            {
                TransactionId = null,
                Amount = booking.CalculateTotal(),
                PaymentMethod = "Online",
                EntryType = "CREDIT",
                EntryDate = DateTime.Now
            });
            _db.SaveChanges();

            TempData["SuccessMessage"] = $"Booking confirmed! Reference: {booking.BookingReference}";
            return RedirectToAction("Confirmation", new { id = booking.Id });
        }
        public IActionResult Confirmation(int id)
        {
            var booking = _db.BungalowBookings.Find(id);
            if (booking == null) return NotFound();
            return View(booking);
        }
    }
}
