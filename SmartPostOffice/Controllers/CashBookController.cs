
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPostOffice.Data;
using SmartPostOffice.Models;
using SmartPostOffice.Models.Enums;
using SmartPostOffice.Services;
[Authorize(AuthenticationSchemes = "OfficerCookies")]
public class CashBookController : Controller
{
    private readonly ICashBookRepository _cashBook;
    private readonly PostOfficeDbContext _db;

    public CashBookController(ICashBookRepository cashBook, PostOfficeDbContext db)
    { _cashBook = cashBook; _db = db; }

    private static readonly ServiceType[] AllTypes = {
        ServiceType.OrdinaryLetter,
        ServiceType.RegisteredMail,
        ServiceType.SpeedPost,
        ServiceType.OrdinaryParcel,
        ServiceType.COD
    };

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var entries = await _cashBook.GetEntriesForDateAsync(today);
        var closed = await _cashBook.GetAllDayBalancesAsync(today);


        var summaries = new List<object>();
        foreach (var st in AllTypes)
        {
            var cash = await _cashBook.GetCashTotalAsync(today, st);
            var online = await _cashBook.GetOnlineTotalAsync(today, st);
            var bal = closed.FirstOrDefault(b => b.ServiceType == st);
            summaries.Add(new
            {
                ServiceType = st,
                RegisterName = GetRegisterName(st),
                CashTotal = cash,
                OnlineTotal = online,
                Total = cash + online,
                IsClosed = (bal != null),
                Balance = bal
            });
        }

        ViewBag.Today = today.ToString("dd MMM yyyy");
        ViewBag.Summaries = summaries;
        ViewBag.GrandTotal = summaries.Sum(s => (decimal)((dynamic)s).Total);
        ViewBag.AllClosed = closed.Count == 5;

        return View(entries);
    }
    [HttpPost]
    public async Task<IActionResult> CloseRegister(
        ServiceType serviceType, decimal physicalCashCounted)
    {
        var today = DateTime.Today;

        var existing = await _cashBook.GetDayBalanceAsync(today, serviceType);
        if (existing != null)
        {
            TempData["ErrorMessage"] =
                $"{GetRegisterName(serviceType)} has already been closed today.";
            return RedirectToAction("Index");
        }

        var cash = await _cashBook.GetCashTotalAsync(today, serviceType);
        var online = await _cashBook.GetOnlineTotalAsync(today, serviceType);
        var count = await _cashBook.GetEntryCountAsync(today, serviceType);

        var officerIdStr = User.FindFirst("OfficerId")?.Value;
        int officerId = int.Parse(officerIdStr ?? "0");

        var balance = new DayBalance();
        balance.CloseDay(serviceType, physicalCashCounted,
                         cash, online, officerId, count);
        await _cashBook.SaveDayBalanceAsync(balance);

        if (balance.Status == "BALANCED")
            TempData["SuccessMessage"] =
                $"{GetRegisterName(serviceType)} closed. Balanced — LKR {cash:N2}.";
        else
            TempData["SuccessMessage"] =
                $"{GetRegisterName(serviceType)} closed. Discrepancy: LKR {Math.Abs(balance.Discrepancy):N2}.";

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DailyReport(DateTime? date = null)
    {
        var reportDate = date?.Date ?? DateTime.Today;
        var entries = await _cashBook.GetEntriesForDateAsync(reportDate);
        var allBal = await _cashBook.GetAllDayBalancesAsync(reportDate);

        var registers = new List<object>();
        foreach (var st in AllTypes)
        {
            var cash = await _cashBook.GetCashTotalAsync(reportDate, st);
            var online = await _cashBook.GetOnlineTotalAsync(reportDate, st);
            var count = await _cashBook.GetEntryCountAsync(reportDate, st);
            var bal = allBal.FirstOrDefault(b => b.ServiceType == st);
            registers.Add(new
            {
                RegisterName = GetRegisterName(st),
                Count = count,
                CashTotal = cash,
                OnlineTotal = online,
                Total = cash + online,
                Balance = bal
            });
        }

        ViewBag.ReportDate = reportDate.ToString("dd MMMM yyyy");
        ViewBag.Registers = registers;
        ViewBag.GrandCash = registers.Sum(r => (decimal)((dynamic)r).CashTotal);
        ViewBag.GrandOnline = registers.Sum(r => (decimal)((dynamic)r).OnlineTotal);
        ViewBag.GrandTotal = registers.Sum(r => (decimal)((dynamic)r).Total);
        ViewBag.AllBalances = allBal;

        return View(entries);
    }


    public async Task<IActionResult> History()
    {
        var history = await _db.DayBalances
            .OrderByDescending(b => b.BalanceDate)
            .Take(150)  // 30 days x 5 registers
            .ToListAsync();
        return View(history);
    }

    public static string GetRegisterName(ServiceType st) => st switch
    {
        ServiceType.OrdinaryLetter => "Ordinary Letter Register",
        ServiceType.RegisteredMail => "Registered Mail Register",
        ServiceType.SpeedPost => "Speed Post Register",
        ServiceType.OrdinaryParcel => "Ordinary Parcel Register",
        ServiceType.COD => "COD Register",
        _ => st.ToString()
    };
}
