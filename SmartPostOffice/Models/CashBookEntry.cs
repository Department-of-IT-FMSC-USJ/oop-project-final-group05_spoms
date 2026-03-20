using System.ComponentModel.DataAnnotations;

namespace SmartPostOffice.Models
{
    public abstract class CashBookEntry
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;

        public decimal Amount { get; set; }

        [Required]
        [StringLength(10)]
        public string PaymentMethod { get; set; } = "Cash";

        public DateTime EntryDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(10)]
        public string EntryType { get; set; } = "CREDIT";

        public abstract string GetRegisterName();
        public abstract string GetDescription();
    }

    // 1. Ordinary Letter Register
    public class OrdinaryLetterEntry : CashBookEntry
    {
        public override string GetRegisterName() => "Ordinary Letter Register";
        public override string GetDescription()
            => $"Ordinary Letter — {PaymentMethod} — LKR {Amount:N2}";
    }

    // 2. Registered Mail Register 
    public class RegisteredMailEntry : CashBookEntry
    {
        public override string GetRegisterName() => "Registered Mail Register";
        public override string GetDescription()
            => $"Registered Mail — {PaymentMethod} — LKR {Amount:N2}";
    }

    // 3. Speed Post Register 
    public class SpeedPostEntry : CashBookEntry
    {
        public override string GetRegisterName() => "Speed Post Register";
        public override string GetDescription()
            => $"Speed Post — {PaymentMethod} — LKR {Amount:N2}";
    }

    // 4. Ordinary Parcel Register 
    public class OrdinaryParcelEntry : CashBookEntry
    {
        public override string GetRegisterName() => "Ordinary Parcel Register";
        public override string GetDescription()
            => $"Ordinary Parcel — {PaymentMethod} — LKR {Amount:N2}";
    }

    // 5. COD Register 
    public class CODEntry : CashBookEntry
    {
        public override string GetRegisterName() => "COD Register";
        public override string GetDescription()
            => $"Cash on Delivery — {PaymentMethod} — LKR {Amount:N2}";
    }
}
