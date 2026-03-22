using SmartPostOffice.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartPostOffice.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int? ServiceRequestId { get; set; }
        public ServiceRequest? ServiceRequest { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string TrackingNumber { get; set; } = string.Empty; // TRK-2024-XXXXXXXX

        [Range(0, 40000)]
        public decimal ActualWeightGrams { get; set; }
        public decimal FinalCharge { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentMethod { get; set; } = "Cash";
        public int ProcessedByOfficerId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string ReceiptNumber { get; set; } = string.Empty;  // RCP-2024-XXXXXX
    }
}
