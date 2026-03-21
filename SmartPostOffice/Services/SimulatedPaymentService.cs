namespace SmartPostOffice.Services
{
    public class SimulatedPaymentService : IPaymentService
    {
        public async Task<PaymentResult> ProcessPaymentAsync(
            decimal amount, string cardNumber, string expiryDate,
            string cvv, string cardHolderName)
        {
            await Task.Delay(800);

            if (string.IsNullOrWhiteSpace(cardNumber)
                || cardNumber.Replace(" ", "").Length != 16)
                return new PaymentResult { Success = false, ErrorMessage = "Invalid card number." };

            if (string.IsNullOrWhiteSpace(cvv) || cvv.Length < 3)
                return new PaymentResult { Success = false, ErrorMessage = "Invalid CVV." };

            if (amount <= 0)
                return new PaymentResult { Success = false, ErrorMessage = "Invalid amount." };

            var reference = $"PAY-{DateTime.Now:yyyyMMddHHmm}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
            return new PaymentResult { Success = true, Reference = reference };
        }
    }
}
