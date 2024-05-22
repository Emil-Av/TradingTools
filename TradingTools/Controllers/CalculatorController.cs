using Microsoft.AspNetCore.Mvc;

namespace TradingTools.Controllers
{
    public class CalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
