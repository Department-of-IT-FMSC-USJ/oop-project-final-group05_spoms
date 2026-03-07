using Microsoft.AspNetCore.Mvc;
namespace SmartPostOffice.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}