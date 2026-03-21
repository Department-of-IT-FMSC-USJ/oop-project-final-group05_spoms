using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPostOffice.Models
{
    public class TelimailMessage : IOnlinePayable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TelimailReference { get; set; } = string.Empty;

        [Required, Display(Name = "Sender Full Name")]
        public string SenderName  { get; set; } = string.Empty;

        [Required, Display(Name = "Sender NIC")]
        public string SenderNIC   { get; set; } = string.Empty;

        [Required, Display(Name = "Sender Phone")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter 10 digits only.")]
        public string SenderPhone { get; set; } = string.Empty;

        [Required, EmailAddress, Display(Name = "Sender Email")]
        public string SenderEmail { get; set; } = string.Empty;

        [Required, Display(Name = "Sender Address")]
        public string SenderAddress { get; set; } = string.Empty;

        [Required, Display(Name = "Recipient Name")]
        public string RecipientName    { get; set; } = string.Empty;

        [Required, Display(Name = "Recipient Address")]
        public string RecipientAddress { get; set; } = string.Empty;

        [Required, Display(Name = "Destination Post Office")]
        public string DestinationPostOffice { get; set; } = string.Empty;

        [Required, MaxLength(500), Display(Name = "Message")]
        public string MessageText { get; set; } = string.Empty;

        public string? HandledBy     { get; set; }
        public string  MessageStatus { get; set; } = "Submitted"; 

        [Column(TypeName = "decimal(10,2)")]
        public decimal TelimailCharge { get; set; } = 250m; 
        public string   PaymentStatus    { get; set; } = "Pending";
        public string?  PaymentReference { get; set; }
        public DateTime? PaidAt          { get; set; }
        public DateTime  CreatedAt       { get; set; } = DateTime.Now;
        public decimal CalculateTotal()  => TelimailCharge;
        public string  GetServiceLabel() => "Telemail";
    }
}
