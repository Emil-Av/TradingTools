using Microsoft.AspNetCore.Mvc;

namespace TradingTools.Controllers
{
    public class NewTradeController : Controller
    {
        public NewTradeController()
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
