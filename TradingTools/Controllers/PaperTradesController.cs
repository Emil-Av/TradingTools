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
using DataAccess.Repository;
using System.Web.Helpers;

namespace TradingTools.Controllers
{
    public class PaperTradesController : Controller
    {
        #region Constructor
        public PaperTradesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            PaperTradesVM = new PaperTradesVM();

        }

        #endregion

        #region Private Properties

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private UserSettings? userSettings;

        #endregion

        #region Public Properties
        public PaperTradesVM PaperTradesVM { get; set; }

        #endregion

        #region Methods
        [HttpPost]

        public async Task<IActionResult> UpdateReview([FromBody] PaperTradesVM data)
        {
            if (data.Review == null)
            {
                return Json(new { error = "Review wasn't updated. Review was null" });
            }

            SanitizationHelper.SanitizeObject(data.Review);
            Review review = await _unitOfWork.Review.GetAsync(x => x.SampleSizeId == data.CurrentTrade.SampleSizeId);
            if (review != null)
            {
                review.First = data.Review.First;
                review.Second = data.Review.Second;
                review.Third = data.Review.Third;
                review.Forth = data.Review.Forth;
                review.Summary = data.Review.Summary;
                _unitOfWork.Review.Update(review);
                _unitOfWork.Save();

                return Json(new { success = "Review updated." });
            }
            else
            {
                return Json(new { error = $"The review for sample size with ID {data.CurrentTrade.SampleSizeId} wasn't found in the data base." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateJournal([FromBody] PaperTradesVM data)
        {
            if (data.Journal == null)
            {
                // Notification error
                return Json(new { error = "Journal wasn't updated. Journal was null." });
            }

            SanitizationHelper.SanitizeObject(data.Journal);

            Journal journal = await _unitOfWork.Journal.GetAsync(x => x.PaperTradeId == data.CurrentTrade.Id);
            if (journal != null)
            {
                journal.Pre = data.Journal.Pre;
                journal.During = data.Journal.During;
                journal.Exit = data.Journal.Exit;
                journal.Post = data.Journal.Post;
                _unitOfWork.Journal.Update(journal);
                _unitOfWork.Save();
            }
            return Json(new { success = "Journal updated." });
        }

        public async Task<IActionResult> LoadTrade(string timeFrame, string strategy, string sampleSize, string trade)
        {
            userSettings = (await _unitOfWork.UserSettings.GetAllAsync()).First();
            TimeFrame timeFrame1 = MyEnumConverter.SetTimeFrameFromString(timeFrame);
            Strategy strategy1 = MyEnumConverter.SetStrategyFromString(strategy);
            _ = int.TryParse(sampleSize, out int sampleSize1);
            _ = int.TryParse(trade, out int trade1);


            List<SampleSize> listSampleSizes = (await _unitOfWork.SampleSize.GetAllAsync(x => x.Strategy == strategy1 && x.TimeFrame == timeFrame1)).OrderByDescending(x => x.Id).ToList();
            // If no sample size is found for the strategy and time frame, then there are no trades for them
            if (listSampleSizes.Count == 0)
            {
                return Json(new {error = $"No trades for {strategy1} strategy on the {timeFrame1} chart." });
            }
            // the paramater "sampleSize1" represents the sampleSize number in descending order (e.g. 3 is the third sample size for the time frame and strategy)
            int sampleSizeId = listSampleSizes[sampleSize1 - 1].Id;
            List<PaperTrade> listTrades = await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == sampleSizeId);
            // If for example a different time frame is selected (or new strategy or new sample size), but this time frame has only 5 trades but the selected trade > 5, then display the latest trade of the sample size
            if (listTrades.Count < trade1)
            {
                PaperTradesVM.CurrentTrade = listTrades.LastOrDefault();
            }
            else
            {
                PaperTradesVM.CurrentTrade = listTrades[trade1 - 1];
            }
            PaperTradesVM.NumberSampleSizes = listSampleSizes.Count();
            PaperTradesVM.TradesInSampleSize = (await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == sampleSizeId)).Count();
            PaperTradesVM.Journal = await _unitOfWork.Journal.GetAsync(x => x.PaperTradeId == PaperTradesVM.CurrentTrade.Id);
            SanitizationHelper.SanitizeObject(PaperTradesVM.Journal);
            PaperTradesVM.Review = await _unitOfWork.Review.GetAsync(x => x.SampleSizeId == PaperTradesVM.CurrentTrade.SampleSizeId);
            SanitizationHelper.SanitizeObject(PaperTradesVM.Review);

            return Json(new { paperTradesVM = PaperTradesVM });
        }
        public async Task<IActionResult> Index()
        {
            // Currently no users, so there is only one data record
            userSettings = (await _unitOfWork.UserSettings.GetAllAsync()).First();
            // Get the latest sample size for the strategy and time frame
            int? latestSampleSize = (await _unitOfWork.SampleSize.GetAllAsync(x => x.TimeFrame == userSettings.PTTimeFrame && x.Strategy == userSettings.PTStrategy)).OrderByDescending(x => x.Id).FirstOrDefault()?.Id;
            // Get the last trade of the sample size
            PaperTradesVM.CurrentTrade = (await _unitOfWork.PaperTrade.GetAllAsync(x => x.TimeFrame == userSettings.PTTimeFrame && x.Strategy == userSettings.PTStrategy && x.SampleSizeId == latestSampleSize)).OrderByDescending(x => x.Id).FirstOrDefault();
            // Get the number of sample sizes for the time frame and strategy
            PaperTradesVM.NumberSampleSizes = (await _unitOfWork.SampleSize.GetAllAsync(x => x.TimeFrame == userSettings.PTTimeFrame && x.Strategy == userSettings.PTStrategy)).Count();
            // Get the number of trades for the sample size
            PaperTradesVM.TradesInSampleSize = (await _unitOfWork.PaperTrade.GetAllAsync(x => x.TimeFrame == userSettings.PTTimeFrame && x.Strategy == userSettings.PTStrategy && x.SampleSizeId == latestSampleSize)).Count();
            PaperTradesVM.Journal = await _unitOfWork.Journal.GetAsync(x => x.PaperTradeId == PaperTradesVM.CurrentTrade.Id);
            SanitizationHelper.SanitizeObject(PaperTradesVM.Journal);
            PaperTradesVM.Review = await _unitOfWork.Review.GetAsync(x => x.SampleSizeId == PaperTradesVM.CurrentTrade.SampleSizeId);
            SanitizationHelper.SanitizeObject(PaperTradesVM.Review);
            
            return View(PaperTradesVM);
        }

        /// <summary>
        ///  Method to process the uploaded .zip file. The .zip file has to have the following structure: mainFolder\Strategy\TimeFrame\SampleSize\TradeNumber\files. Reviews.odt has to be in the SampleSize folder.
        /// </summary>
        /// <param name="zipFile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadTrades(IFormFile zipFile)
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
                        string lastTimeFrame = string.Empty;
                        int currentSampleSizeId = 0;
                        PaperTrade? trade = null;
                        Journal? journal = null;
                        SampleSize? sampleSize = null;
                        List<ZipArchiveEntry> sortedEntries = archive.Entries.
                                                                        OrderBy(e => e.FullName, new NaturalStringComparer()).ToList();
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
                                        lastTradeId = (await _unitOfWork.PaperTrade.GetAllAsync()).
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
                            // Entry is either a screenshot or a .odt file
                            else
                            {
                                string[] tradeInfo = entry.FullName.Split('/');
                                trade.Strategy = MyEnumConverter.SetStrategyFromString(tradeInfo[1]);
                                trade.TimeFrame = MyEnumConverter.SetTimeFrameFromString(tradeInfo[2]);
                                trade.SampleSizeId = currentSampleSizeId;


                                string currentSampleSize = tradeInfo[3];
                                string currentTimeFrame = tradeInfo[2];
                                // First sample size of the loop or a new one
                                if (lastSampleSize != currentSampleSize || lastTimeFrame != currentTimeFrame)
                                {
                                    lastTimeFrame = currentTimeFrame;
                                    lastSampleSize = currentSampleSize;
                                    sampleSize = new SampleSize();
                                    sampleSize.Strategy = (Strategy)trade.Strategy;
                                    sampleSize.TimeFrame = (TimeFrame)trade.TimeFrame;
                                    _unitOfWork.SampleSize.Add(sampleSize);
                                    _unitOfWork.Save();
                                    currentSampleSizeId = (await _unitOfWork.SampleSize.GetAllAsync()).
                                                                                Select(x => x.Id).OrderByDescending(id => id).FirstOrDefault();
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
                                        string screenshotPath = currentFolder.Replace(wwwRootPath, "").Replace("\\\\", "/");
                                        trade.ScreenshotsUrls.Add(Path.Combine(screenshotPath, screenshotName));
                                    }
                                    else if (!entry.FullName.Contains("Reviews"))
                                    {
                                        ParseODTJournalFile(entry, journal);

                                    }
                                }
                                catch
                                {
                                    // Display error message
                                }
                                // The Review file is in SampleSize\Reviews.odt folder, not in the individual trade's folder. Don't create a new trade record in the database for that iteration
                                if (!entry.FullName.Contains("Reviews"))
                                {
                                    canCreateNewTrade = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in parsing the .zip file: {ex.Message}");
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
                // No need for "Reviews" folder
                if (!tradeInfo[i].Contains("Reviews"))
                {
                    folders.Add(tradeInfo[i]);
                }
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

    #endregion

}

