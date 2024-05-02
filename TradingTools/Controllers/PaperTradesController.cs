using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.IO.Compression;
using System.Text.RegularExpressions;

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

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    zipFile.CopyTo(memoryStream);

                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        bool isNewTrade = true;
                        PaperTrade? trade = null;
                        string strategy = string.Empty;
                        string timeFrame = string.Empty;
                        string sampleSize = string.Empty;
                        string tradeNumber = string.Empty; 

                        List<ZipArchiveEntry> sortedEntries = archive.Entries.OrderBy(e => e.FullName, new NaturalStringComparer()).ToList();
                        foreach (var entry in sortedEntries)
                        {
                            if (entry.FullName.EndsWith('/') || entry.FullName.EndsWith("\\"))
                            {
                                if (isNewTrade)
                                {
                                    //_unitOfWork.PaperTrade.Add(trade);
                                    trade = new PaperTrade();
                                }
                                isNewTrade = false;
                                continue;
                            }
                            else if (!entry.FullName.Contains("Reviews"))
                            {
                                if (!isNewTrade)
                                {
                                    string[] tradeInfo = entry.FullName.Split('/');
                                    strategy = tradeInfo[1];
                                    timeFrame = tradeInfo[2];
                                    sampleSize = tradeInfo[3];
                                    tradeNumber = tradeInfo[4];


                                }
                                isNewTrade = true;
                                if (entry.FullName.EndsWith("png"))
                                {
                                    trade?.ScreenshotsUrls?.Add(entry.FullName);
                                }

                            }
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {

            }

            return RedirectToAction("Index");
        }
    }

    public class NaturalStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            string[] xParts = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
            string[] yParts = Regex.Split(y.Replace(" ", ""), "([0-9]+)");

            int minLength = Math.Min(xParts.Length, yParts.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (xParts[i] != yParts[i])
                {
                    if (int.TryParse(xParts[i], out int xNum) && int.TryParse(yParts[i], out int yNum))
                    {
                        return xNum.CompareTo(yNum);
                    }
                    else
                    {
                        return string.Compare(xParts[i], yParts[i], StringComparison.Ordinal);
                    }
                }
            }

            return xParts.Length.CompareTo(yParts.Length);
        }
    }
}
