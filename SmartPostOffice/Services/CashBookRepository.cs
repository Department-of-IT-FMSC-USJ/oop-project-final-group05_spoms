using Microsoft.EntityFrameworkCore;
using SmartPostOffice.Data;
using SmartPostOffice.Models;
using SmartPostOffice.Models.Enums;
 
namespace SmartPostOffice.Services
{
    public class CashBookRepository : ICashBookRepository
    {
        private readonly PostOfficeDbContext _db;
        public CashBookRepository(PostOfficeDbContext db) { _db = db; }
 
        public async Task AddEntryAsync(CashBookEntry entry)
        {
            _db.CashBookEntries.Add(entry);
            await _db.SaveChangesAsync();
        }
 
        public async Task<List<CashBookEntry>> GetEntriesForDateAsync(DateTime date)
            => await _db.CashBookEntries
                .Include(e => e.Transaction)
                    .ThenInclude(t => t.ServiceRequest)
                .Where(e => e.EntryDate.Date == date.Date)
                .OrderBy(e => e.Transaction.ServiceRequest.ServiceType)
                .ThenBy(e => e.EntryDate)
                .ToListAsync();
 
        public async Task<List<CashBookEntry>> GetEntriesByTypeAsync(
            DateTime date, ServiceType type)
            => await _db.CashBookEntries
                .Include(e => e.Transaction)
                    .ThenInclude(t => t.ServiceRequest)
                .Where(e => e.EntryDate.Date == date.Date
                         && e.Transaction.ServiceRequest.ServiceType == type)
                .OrderBy(e => e.EntryDate)
                .ToListAsync();

        public async Task<decimal> GetCashTotalAsync(DateTime date, ServiceType type)
            => await _db.CashBookEntries
                .Where(e => e.EntryDate.Date == date.Date
                         && e.Transaction.ServiceRequest.ServiceType == type
                         && e.PaymentMethod == "Cash")
                .SumAsync(e => (decimal?)e.Amount) ?? 0m;
 
        public async Task<decimal> GetOnlineTotalAsync(DateTime date, ServiceType type)
            => await _db.CashBookEntries
                .Where(e => e.EntryDate.Date == date.Date
                         && e.Transaction.ServiceRequest.ServiceType == type
                         && e.PaymentMethod == "Online")
                .SumAsync(e => (decimal?)e.Amount) ?? 0m;
 
        public async Task<DayBalance?> GetDayBalanceAsync(
            DateTime date, ServiceType type)
            => await _db.DayBalances
                .FirstOrDefaultAsync(b => b.BalanceDate.Date == date.Date
                                      && b.ServiceType == type);
 
        public async Task<List<DayBalance>> GetAllDayBalancesAsync(DateTime date)
            => await _db.DayBalances
                .Where(b => b.BalanceDate.Date == date.Date)
                .OrderBy(b => b.ServiceType)
                .ToListAsync();
 
        public async Task SaveDayBalanceAsync(DayBalance balance)
        {
            _db.DayBalances.Add(balance);
            await _db.SaveChangesAsync();
        }
 
        public async Task<int> GetEntryCountAsync(DateTime date, ServiceType type)
            => await _db.CashBookEntries
                .CountAsync(e => e.EntryDate.Date == date.Date
                             && e.Transaction.ServiceRequest.ServiceType == type);
    }
}
