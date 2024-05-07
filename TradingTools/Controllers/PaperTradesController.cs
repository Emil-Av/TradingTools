using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Protocol;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using static NuGet.Packaging.PackagingConstants;
using System.ComponentModel;
using System.Web.Mvc.Html;
using Utilities;
using Models.ViewModels;

namespace TradingTools.Controllers
{
    public class PaperTradesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PaperTradesVM PaperTradesVM { get; set; }
        public PaperTradesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index()
        {
            PaperTradesVM = new PaperTradesVM()
            {
                ListPaperTrades = _unitOfWork.PaperTrade.GetAll().ToList()
            };

            return View(PaperTradesVM);
        }

        /// <summary>
        ///  Method to process the uploaded .zip file. The .zip file has to have the structure mainFolder\Strategy\TimeFrame\SampleSize\TradeNumber\files
        /// </summary>
        /// <param name="zipFile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadTrades(IFormFile zipFile)
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
                        bool canCreateNewTrade = true;
                        string currentFolder = string.Empty;
                        string lastSampleSize = string.Empty;
                        int currentSampleSizeId = 0;
                        PaperTrade? trade = null;
                        Journal? journal = null;
                        Review? review = new Review();
                        SampleSize? sampleSize = null;
                        List<ZipArchiveEntry> sortedEntries = archive.Entries.OrderBy(e => e.FullName, new NaturalStringComparer()).ToList();
                        List<string> folders = new List<string>();

                        foreach (var entry in sortedEntries)
                        {
                            // Entry is a folder
                            if (entry.FullName.EndsWith('/') || entry.FullName.EndsWith("\\"))
                            {
                                if (canCreateNewTrade)
                                {
                                    int lastTradeId = 0;
                                    // Loop is running > 1 time
                                    if (trade != null)
                                    {
                                        _unitOfWork.PaperTrade.Add(trade);
                                        _unitOfWork.Save();
                                        lastTradeId = _unitOfWork.PaperTrade.GetAll().
                                                                            Select(x => x.Id).OrderByDescending(id => id).FirstOrDefault();
                                        journal.PaperTradeId = lastTradeId;
                                        _unitOfWork.Journal.Add(journal);
                                        _unitOfWork.Save();
                                    }
                                    journal = new Journal();
                                    trade = new PaperTrade();
                                    folders.Clear();
                                }
                                canCreateNewTrade = false;
                                continue;
                            }
                            // The review for the sample size
                            else if (entry.FullName.Contains("Review"))
                            {
                                ParseODTReviewFile(entry, review);
                            }
                            // Entry is either a screenshot or the .odt journal file
                            else
                            {
                                string[] tradeInfo = entry.FullName.Split('/');
                                trade.Strategy = MyEnumConverter.SetStrategy(tradeInfo[1]);
                                trade.TimeFrame = MyEnumConverter.SetTimeFrame(tradeInfo[2]);
                                trade.SampleSizeId = currentSampleSizeId;


                                string currentSampleSize = tradeInfo[3];
                                // First sample size of the loop or a new one
                                if (lastSampleSize != currentSampleSize)
                                {
                                    lastSampleSize = currentSampleSize;
                                    sampleSize = new SampleSize();
                                    sampleSize.Strategy = trade.Strategy;
                                    sampleSize.TimeFrame = trade.TimeFrame;
                                    _unitOfWork.SampleSize.Add(sampleSize);
                                    _unitOfWork.Save();
                                    currentSampleSizeId = _unitOfWork.SampleSize.GetAll().
                                                                                Select(x => x.Id).OrderByDescending(id => id).FirstOrDefault();

                                    review.TradeType = TradeType.PaperTrade;
                                    review.TimeFrame = trade.TimeFrame;
                                    review.Strategy = trade.Strategy;
                                    _unitOfWork.Review.Add(review);
                                    _unitOfWork.Save();
                                }

                                if (!canCreateNewTrade)
                                {
                                    currentFolder = CreateFolders(tradeInfo, currentFolder, entry.FullName, wwwRootPath, folders);
                                }

                                try
                                {
                                    // Save the file
                                    if (entry.FullName.EndsWith(".png"))
                                    {
                                        entry.ExtractToFile(Path.Combine(currentFolder, entry.Name));
                                        string screenshotName = entry.FullName.Split('/').Last();
                                        trade.ScreenshotsUrls.Add(Path.Combine(currentFolder, screenshotName));
                                    }
                                    else if (entry.FullName.EndsWith(".odt"))
                                    {
                                        ParseODTJournalFile(entry, journal);

                                    }
                                }
                                catch
                                {
                                    // Display error message
                                }
                                canCreateNewTrade = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {

            }

            return RedirectToAction("Index");
        }

        string CreateFolders(string[] tradeInfo, string currentFolder, string entryFullName, string wwwRootPath, List<string> folders)
        {
            // wwwroot\Trades folder
            currentFolder = Path.Combine(wwwRootPath, tradeInfo[0]);
            // Get all subfolders
            for (int i = 1; i <= 4; i++)
            {
                folders.Add(tradeInfo[i]);
            }
            // Create the wwwroot\Trades folder
            if (!Directory.Exists(currentFolder))
            {
                Directory.CreateDirectory(currentFolder);
            }
            // Create all subfolders
            for (int i = 0; i < folders.Count; i++)
            {
                currentFolder = Path.Combine(currentFolder, folders[i]);
                if (!Directory.Exists(currentFolder))
                {
                    Directory.CreateDirectory(currentFolder);
                }
            }

            return currentFolder;
        }

        void ParseODTReviewFile(ZipArchiveEntry entry, Review review)
        {
            using (Stream stream = entry.Open())
            {
                using (ZipArchive archive = new ZipArchive(stream))
                {
                    ZipArchiveEntry contentEntry = archive.GetEntry("content.xml");
                    if (contentEntry != null)
                    {
                        using (StreamReader reader = new StreamReader(contentEntry.Open()))
                        {
                            string xmlContent = reader.ReadToEnd();
                            XDocument xmlDoc = XDocument.Parse(xmlContent);
                            List<XElement> nodes = xmlDoc.Descendants().Where(e => e.Name.LocalName == "p").ToList();
                            foreach (XElement node in nodes)
                            {
                                XElement element = XElement.Parse(node.ToString());
                                review.SampleSizeReview += string.IsNullOrEmpty(element.Value) ? "\n" : element.Value;
                            }
                        }
                    }
                }
            }
        }

        void ParseODTJournalFile(ZipArchiveEntry entry, Journal journal)
        {
            using (Stream stream = entry.Open())
            {
                using (ZipArchive archive = new ZipArchive(stream))
                {
                    ZipArchiveEntry contentEntry = archive.GetEntry("content.xml");
                    if (contentEntry != null)
                    {
                        using (StreamReader reader = new StreamReader(contentEntry.Open()))
                        {
                            string xmlContent = reader.ReadToEnd();
                            XDocument xmlDoc = XDocument.Parse(xmlContent);
                            List<XElement> nodes = xmlDoc.Descendants().Where(e => e.Name.LocalName == "p").ToList();
                            string lastJournal = string.Empty;
                            foreach (XElement node in nodes)
                            {
                                XElement element = XElement.Parse(node.ToString());
                                if (element.Value.Contains("[Pre]"))
                                {
                                    lastJournal = "[Pre]";
                                    continue;

                                }
                                else if (element.Value.Contains("[During]"))
                                {
                                    lastJournal = "[During]";
                                    continue;

                                }
                                else if (element.Value.Contains("[Exit]"))
                                {
                                    lastJournal = "[Exit]";
                                    continue;

                                }
                                else if (element.Value.Contains("[Post]"))
                                {
                                    lastJournal = "[Post]";
                                    continue;

                                }

                                if (lastJournal.Equals("[Pre]"))
                                {
                                    journal.Pre += string.IsNullOrEmpty(element.Value) ? "\n" : element.Value;
                                }
                                else if (lastJournal.Equals("[During]"))
                                {
                                    journal.During += string.IsNullOrEmpty(element.Value) ? "\n" : element.Value;
                                }
                                else if (lastJournal.Equals("[Exit]"))
                                {
                                    journal.Exit += string.IsNullOrEmpty(element.Value) ? "\n" : element.Value;
                                }
                                else if (lastJournal.Equals("[Post]"))
                                {
                                    journal.Post += string.IsNullOrEmpty(element.Value) ? "\n" : element.Value;
                                }
                            }
                        }
                    }
                }
            }
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

