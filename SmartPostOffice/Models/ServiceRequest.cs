using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartPostOffice.Models.Enums;

namespace SmartPostOffice.Models
{
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public ServiceType ServiceType { get; set; }
      
        [Required(ErrorMessage = "Sender name is required")]
        [Display(Name = "Sender Name")]
        public string SenderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sender phone is required")]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter 10 digits only.")]
        public string SenderPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sender address is required")]
        [Display(Name = "Postal Address")]
        public string SenderAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Receiver name is required")]
        [Display(Name = "Receiver Name")]
        public string ReceiverName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Receiver phone is required")]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter 10 digits only.")]
        public string ReceiverPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Receiver address is required")]
        [Display(Name = "Postal Address")]
        public string ReceiverAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Estimated weight is required")]
        [Range(1, 50000, ErrorMessage = "Weight must be between 1 and 50,000 grams")]
        [Display(Name = "Estimated Weight(g)")]

        public decimal EstimatedWeightGrams { get; set; }

        [Required(ErrorMessage = "Destination is required")]
        [Display(Name = "Destination")]
        public string Destination { get; set; } = string.Empty;

        // COD SPECIFIC FIELDS 

        [Display(Name = "Item Description")]
        [MaxLength(500)]
        public string? ItemDescription { get; set; }

        [Display(Name = "Seller Profit Amount")]
        [Range(0, 999999, ErrorMessage = "Profit amount must be a positive value")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SellerProfitAmount { get; set; }  

        // TotalCODAmount is NOT filled by the customer
        // Officer calculates: TotalCODAmount = PostageCharge (by weight rules) + SellerProfitAmount

        [Display(Name = "Total COD Amount")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCODAmount { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.PENDING;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}