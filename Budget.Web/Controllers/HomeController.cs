using Microsoft.AspNetCore.Mvc;

namespace Budget.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Entries");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
