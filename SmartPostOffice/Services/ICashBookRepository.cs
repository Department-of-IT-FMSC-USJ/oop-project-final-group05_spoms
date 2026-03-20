using SmartPostOffice.Models;
using SmartPostOffice.Models.Enums;
 
namespace SmartPostOffice.Services
{
    public interface ICashBookRepository
    {
        Task AddEntryAsync(CashBookEntry entry);

        Task<List<CashBookEntry>> GetEntriesForDateAsync(DateTime date);

        Task<List<CashBookEntry>> GetEntriesByTypeAsync(DateTime date, ServiceType type);
        Task<decimal> GetCashTotalAsync(DateTime date, ServiceType type);
        Task<decimal> GetOnlineTotalAsync(DateTime date, ServiceType type);
        Task<DayBalance?> GetDayBalanceAsync(DateTime date, ServiceType type);
 
        Task<List<DayBalance>> GetAllDayBalancesAsync(DateTime date);

        Task SaveDayBalanceAsync(DayBalance balance);
 
        Task<int> GetEntryCountAsync(DateTime date, ServiceType type);
    }
}
