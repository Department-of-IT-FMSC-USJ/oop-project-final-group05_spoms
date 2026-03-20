using System.ComponentModel.DataAnnotations;
using SmartPostOffice.Models.Enums;
 
namespace SmartPostOffice.Models
{
    public class DayBalance
    {
        public int Id { get; set; }
        public DateTime BalanceDate { get; set; } 
        public ServiceType ServiceType { get; set; } 
        public decimal SystemCashTotal { get; set; }   
        public decimal SystemOnlineTotal { get; set; } 
        public decimal SystemTotal { get; set; }
        public decimal PhysicalCashCounted { get; set; }
        public decimal Discrepancy { get; private set; } 
        [StringLength(20)]
        public string Status { get; set; } = "BALANCED"; 
 
        public int ClosedByOfficerId { get; set; }
        public int TotalEntries { get; set; } 
        public void CloseDay(
            ServiceType serviceType,
            decimal physicalCash,
            decimal systemCash,
            decimal systemOnline,
            int officerId,
            int entryCount)
        {
            ServiceType          = serviceType;
            PhysicalCashCounted  = physicalCash;
            SystemCashTotal      = systemCash;
            SystemOnlineTotal    = systemOnline;
            SystemTotal          = systemCash + systemOnline;
            TotalEntries         = entryCount;
            ClosedByOfficerId    = officerId;
            BalanceDate          = DateTime.Today;
            Discrepancy = physicalCash - systemCash;
            Status      = (Discrepancy == 0m) ? "BALANCED" : "DISCREPANCY";
        }
    }
}
