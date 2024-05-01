using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace TradingTools.Controllers
{
    public class PaperTradesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PaperTradesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<PaperTrade> objPaperTrades = _db.PaperTrades.ToList();

            return View(objPaperTrades);
        }

        [HttpPost]
        public IActionResult UploadTrades()
        {

            return RedirectToAction("Index"); 
        }
    }
}
