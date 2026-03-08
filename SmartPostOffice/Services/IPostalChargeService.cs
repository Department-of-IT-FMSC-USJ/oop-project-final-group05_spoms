using SmartPostOffice.Models.Enums;

namespace SmartPostOffice.Services
{
    public interface IPostalChargeService
    {
        decimal CalculateCharge(ServiceType serviceType, decimal weightGrams);
    }
}
