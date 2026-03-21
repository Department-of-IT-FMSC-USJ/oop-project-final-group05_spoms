using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPostOffice.Models
{
    public class BungalowBooking : IOnlinePayable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string BookingReference { get; set; } = string.Empty;  // BNG-20260321-4821

        // Guest details
        [Required, Display(Name = "Full Name")]
        public string GuestName { get; set; } = string.Empty;

        [Required, Display(Name = "NIC Number")]
        public string NIC { get; set; } = string.Empty;

        [Required, Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter 10 digits only.")]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Booking details
        [Required]
        public string Location { get; set; } = string.Empty;
        public string? RoomOption { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "Check-In Date")]
        public DateTime CheckIn { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "Check-Out Date")]
        public DateTime CheckOut { get; set; }

        [Required, Range(1, 13), Display(Name = "Number of Guests")]
        public int GuestCount { get; set; }

        public bool IsStaffRate { get; set; } = false;

        // Pricing — set by controller from BungalowCatalogue, never from the form
        [Column(TypeName = "decimal(10,2)")]
        public decimal RoomRatePerNight { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ServiceCharge { get; set; } = 300m;

        // Payment
        public string PaymentStatus { get; set; } = "Pending";  // Pending / Paid
        public string? PaymentReference { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Computed helpers
        public int Nights => Math.Max(1, (CheckOut - CheckIn).Days);

        // IOnlinePayable implementation
        public decimal CalculateTotal() => (RoomRatePerNight * Nights) + ServiceCharge;
        public string GetServiceLabel() => "Bungalow Booking";
    }
}
