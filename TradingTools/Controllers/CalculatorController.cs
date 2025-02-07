using Microsoft.AspNetCore.Mvc;

namespace TradingTools.Controllers
{
    public class CalculatorController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
