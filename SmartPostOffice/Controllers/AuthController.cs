using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SmartPostOffice.Data;
using SmartPostOffice.Models;


public class AuthController : Controller
{
    private readonly PostOfficeDbContext _db;
    public AuthController(PostOfficeDbContext db) { _db = db; }

    [HttpGet]
    public IActionResult Login() => View();
    

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var officer = _db.Officers
            .FirstOrDefault(o => o.Email == email && o.IsActive);

        if (officer == null)
        { ViewBag.Error = "Invalid credentials"; return View(); }

        var hasher = new PasswordHasher<PostOfficeOfficer>();
        var result = hasher.VerifyHashedPassword(officer, officer.PasswordHash, password);

        if (result == PasswordVerificationResult.Failed)
        { ViewBag.Error = "Invalid credentials"; return View(); }

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, officer.FullName),  
            new Claim(ClaimTypes.Email, officer.Email),
            new Claim("OfficerId", officer.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "OfficerCookies");
        await HttpContext.SignInAsync("OfficerCookies", new ClaimsPrincipal(identity));

        return RedirectToAction("Dashboard", "Counter");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("OfficerCookies");
        return RedirectToAction("Index", "Home");
    }
}
