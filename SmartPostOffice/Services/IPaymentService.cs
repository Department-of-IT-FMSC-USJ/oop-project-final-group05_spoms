namespace SmartPostOffice.Services
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
    }

    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(
            decimal amount,
            string cardNumber,
            string expiryDate,
            string cvv,
            string cardHolderName);
    }
}
