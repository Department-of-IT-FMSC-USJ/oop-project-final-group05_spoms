using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPostOffice.Models
{
    public class StampOrder : IOnlinePayable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OrderReference { get; set; } = string.Empty;

        [Required, Display(Name = "Full Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Required, Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter 10 digits only.")]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required]
        public int StampStyleId { get; set; }
        [Required]
        public string StampStyleName { get; set; } = string.Empty;
        public string? OrderLinesJson { get; set; }

        [Required, Range(1, 25, ErrorMessage = "You can order 1 to 25 stamps.")]
        [Display(Name = "Number of Stamps")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerStamp { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ServiceCharge { get; set; } = StampCatalogue.FixedServiceCharge;
        public string PaymentStatus { get; set; } = "Pending";
        public string? PaymentReference { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

       
public string? OrderLinesJson { get; set; }
       public decimal CalculateTotal()
        {
            // Try to compute from OrderLinesJson if available
            if (!string.IsNullOrEmpty(OrderLinesJson))
            {
                try
                {
                    var lines = System.Text.Json.JsonSerializer.Deserialize<List<OrderLine>>(OrderLinesJson);
                    if (lines != null)
                        return lines.Sum(l => l.qty * l.price) + ServiceCharge;
                }
                catch { }
            }
            // Fallback to single-style
            return (PricePerStamp * Quantity) + ServiceCharge;
        }

        public string GetServiceLabel() => "Stamp Order";
    }
    public class OrderLine
    {
        public int     styleId { get; set; }
        public string  name    { get; set; } = string.Empty;
        public int     qty     { get; set; }
        public decimal price   { get; set; }
    }

}
