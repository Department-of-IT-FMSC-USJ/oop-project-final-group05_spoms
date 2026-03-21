namespace SmartPostOffice.Models
{
    public interface IOnlinePayable
    {
        decimal CalculateTotal();      // total the customer must pay online
        string  GetServiceLabel();     // e.g. "Bungalow Booking", "Stamp Order"
    }
}
