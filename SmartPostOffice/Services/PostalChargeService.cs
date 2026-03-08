using SmartPostOffice.Models.Enums;

namespace SmartPostOffice.Services
{
    public class PostalChargeService : IPostalChargeService
    {
        private static readonly (decimal min, decimal max, decimal charge)[] OrdinaryRates =
        {
            (0,20,50),(20,30,60),(30,40,70),(40,50,80),(50,60,90),(60,70,100),
            (70,80,110),(80,90,120),(90,100,130),(100,150,150),(150,200,170),
            (200,250,190),(250,300,210),(300,350,230),(350,400,250),(400,450,270),
            (450,500,290),(500,550,310),(550,600,330),(600,650,350),(650,700,370),
            (700,750,390),(750,800,410),(800,850,430),(850,900,450),(900,950,470),
            (950,1000,490),(1000,1250,520),(1250,1500,550),(1500,1750,580),(1750,2000,610)
        };

        private static readonly (decimal min, decimal max, decimal charge)[] RegisteredRates =
        {
            (0,20,110),(20,30,120),(30,40,130),(40,50,140),(50,60,150),(60,70,160),
            (70,80,170),(80,90,180),(90,100,190),(100,150,210),(150,200,230),
            (200,250,250),(250,300,270),(300,350,290),(350,400,310),(400,450,330),
            (450,500,350),(500,550,370),(550,600,390),(600,650,410),(650,700,430),
            (700,750,450),(750,800,470),(800,850,490),(850,900,510),(900,950,530),
            (950,1000,550),(1000,1250,580),(1250,1500,610),(1500,1750,640),
            (1750,2000,670),(2000,3000,720),(3000,4000,770),(4000,5000,820)
        };
        private static readonly (decimal min, decimal max, decimal charge)[] CourierRates =
        {
            (0,250,200),(250,500,250),(500,1000,350),(1000,2000,400),(2000,3000,450),
            (3000,4000,500),(4000,5000,550),(5000,6000,600),(6000,7000,650),
            (7000,8000,700),(8000,9000,750),(9000,10000,800),(10000,15000,950),
            (15000,20000,1100),(20000,25000,1600),(25000,30000,2100),
            (30000,35000,2600),(35000,40000,3100)
        };
        private static readonly (decimal min, decimal max, decimal charge)[] ParcelRates =
        {
            (0,250,150),(250,500,200),(500,1000,250),(1000,2000,300),(2000,3000,350),
            (3000,4000,400),(4000,5000,450),(5000,6000,500),(6000,7000,550),
            (7000,8000,600),(8000,9000,650),(9000,10000,700),(10000,15000,800),
            (15000,20000,900)
        };

        public decimal CalculateCharge(ServiceType serviceType, decimal weightGrams)
        {
            return serviceType switch
            {
                ServiceType.OrdinaryLetter => LookupRate(OrdinaryRates, weightGrams),
                ServiceType.RegisteredMail => LookupRate(RegisteredRates, weightGrams),
                ServiceType.SpeedPost => LookupRate(CourierRates, weightGrams),
                ServiceType.OrdinaryParcel => LookupRate(ParcelRates, weightGrams),
                ServiceType.COD => LookupRate(ParcelRates, weightGrams),
                _ => throw new ArgumentOutOfRangeException(nameof(serviceType))
            };
        }

        private decimal LookupRate((decimal min, decimal max, decimal charge)[] rates, decimal w)
        {
            foreach (var (min, max, charge) in rates)
                if (w > min && w <= max) return charge;
            return rates[rates.Length - 1].charge; // max band fallback
        }
    }
}
