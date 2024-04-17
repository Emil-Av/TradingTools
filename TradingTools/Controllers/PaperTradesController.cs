using Microsoft.AspNetCore.Mvc;

namespace TradingTools.Controllers
{
    public class PaperTradesController : Controller
    {
        public PaperTradesController() 
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
