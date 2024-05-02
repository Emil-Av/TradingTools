using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.IO.Compression;

namespace TradingTools.Controllers
{
    public class PaperTradesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PaperTradesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<PaperTrade> objPaperTrades = _unitOfWork.PaperTrade.GetAll().ToList();

            return View(objPaperTrades);
        }

        /// <summary>
        ///  Method to upload existing trades.
        /// </summary>
        /// <param name="zipFile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadTradesAsync(IFormFile zipFile)
        {
            // Check if the submitted file is a zip file
            if (zipFile == null || Path.GetExtension(zipFile.FileName).ToLower() != ".zip")
            {
                // Show error notification
            }

            return RedirectToAction("Index"); 
        }
    }
}
