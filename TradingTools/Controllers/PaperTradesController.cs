using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using System.IO.Compression;
using System.Xml.Linq;
using Utilities;
using Models.ViewModels;
using SharedEnums.Enums;
using Shared;


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
                return Json(new { error = "Review wasn't updated. Review was null." });
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
                await _unitOfWork.Review.UpdateAsync(review);
                await _unitOfWork.SaveAsync();

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
                await _unitOfWork.Journal.UpdateAsync(journal);
                await _unitOfWork.SaveAsync();
            }
            return Json(new { success = "Journal updated." });
        }

        public async Task<IActionResult> LoadTrade(string timeFrame, string strategy, string sampleSize, string trade, string showLastTrade, string sampleSizeChanged)
        {
            //userSettings = (await _unitOfWork.UserSettings.GetAllAsync()).First();
            TimeFrame timeFrame1 = TimeFrame.M5;
            Strategy strategy1 = Strategy.Cradle;
            List<string> errors = new List<string>();   
            // Check and parse the paramaters

            ToEnumFromStringResult<TimeFrame> resultTimeFrame = MyEnumConverter.TimeFrameFromString(timeFrame);
            if (!resultTimeFrame.Success)
            {
                errors.Add(resultTimeFrame.ErrorMessage);
            }
            else
            {
                timeFrame1 = resultTimeFrame.Value;
            }

            ToEnumFromStringResult<Strategy> resultStrategy = MyEnumConverter.StrategyFromString(strategy);

            if (!resultStrategy.Success)
            {
                errors.Add(resultStrategy.ErrorMessage);
            }
            else
            {
                strategy1 = resultStrategy.Value;
            }


            if (!int.TryParse(sampleSize, out int sampleSize1))
            {
                errors.Add("Error loading the trade. Wrong paramater: Could not parse the sample size id");
            }

            if (!int.TryParse(trade, out int trade1))
            {
                errors.Add("Error loading the trade. Wrong paramater: Could not parse the trade id");
            }
            if (!bool.TryParse(showLastTrade, out bool showLastTrade1))
            {
                errors.Add("Error loading the trade. Wrong paramater: Could not parse showLastTrade");
            }
            if (!bool.TryParse(sampleSizeChanged, out bool sampleSizeChanged1))
            {
                errors.Add("Error loading the trade. Wrong paramater: Could not parse sampleSizeChanged");
            }

            if (errors.Any())
            {
                string errorMsg = string.Join("<br>", errors);
                return Json(new {error = errorMsg});    
            }

            List<SampleSize> listSampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.Strategy == strategy1 && x.TimeFrame == timeFrame1);
            // If no sample size is found for the strategy and time frame, then there are no trades for them
            if (listSampleSizes.Count == 0)
            {
                return Json(new { info = $"No trades for {strategy1} strategy on the {timeFrame1} chart." });
            }
            int sampleSizeId;
            if (sampleSizeChanged1 || !showLastTrade1)
            {
                // the paramater "sampleSize1" represents the sampleSize number in descending order (e.g. 3 is the third sample size for the time frame and strategy)
                sampleSizeId = listSampleSizes[sampleSize1 - 1].Id;
                PaperTradesVM.CurrentSampleSize = sampleSize1;
            }
            else
            {
                sampleSizeId = listSampleSizes.LastOrDefault().Id;
                PaperTradesVM.CurrentSampleSize = listSampleSizes.Count;
            }

            List<PaperTrade> listTrades = await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == sampleSizeId);
            // If for example a different time frame or new strategy or new sample size is selected, then display the latest trade of the sample size
            if (showLastTrade1)
            {
                PaperTradesVM.CurrentTrade = listTrades.LastOrDefault();
            }
            else
            {
                PaperTradesVM.CurrentTrade = listTrades[trade1 - 1];
            }
            // Set the values for the ajax response
            PaperTradesVM.NumberSampleSizes = listSampleSizes.Count();
            PaperTradesVM.TradesInSampleSize = (await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == sampleSizeId)).Count();
            PaperTradesVM.Journal = await _unitOfWork.Journal.GetAsync(x => x.PaperTradeId == PaperTradesVM.CurrentTrade.Id);
            SanitizationHelper.SanitizeObject(PaperTradesVM.Journal);
            PaperTradesVM.Review = await _unitOfWork.Review.GetAsync(x => x.SampleSizeId == PaperTradesVM.CurrentTrade.SampleSizeId);
            SanitizationHelper.SanitizeObject(PaperTradesVM.Review);

            // Send the response
            return Json(new { paperTradesVM = PaperTradesVM });
        }
        public async Task<IActionResult> Index()
        {
            // Currently no users, so there is only one data record
            userSettings = (await _unitOfWork.UserSettings.GetAllAsync()).First();
            // Get the latest sample size for the strategy and time frame
            int? latestSampleSize = (await _unitOfWork.SampleSize.GetAllAsync(x => x.TimeFrame == userSettings.PTTimeFrame && x.Strategy == userSettings.PTStrategy)).Last().Id;
            // Get the last trade of the sample size
            PaperTradesVM.CurrentTrade = (await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == latestSampleSize)).LastOrDefault()!;
            // No trades yet
            if (PaperTradesVM.CurrentTrade == null)
            {
                return View(PaperTradesVM);
            }
            // Get the number of sample sizes for the time frame and strategy
            PaperTradesVM.NumberSampleSizes = (await _unitOfWork.SampleSize.GetAllAsync(x => x.TimeFrame == userSettings.PTTimeFrame && x.Strategy == userSettings.PTStrategy)).Count();
            // Get the number of trades for the sample size
            PaperTradesVM.TradesInSampleSize = (await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == latestSampleSize)).Count();
            PaperTradesVM.Journal = await _unitOfWork.Journal.GetAsync(x => x.PaperTradeId == PaperTradesVM.CurrentTrade.Id);
            SanitizationHelper.SanitizeObject(PaperTradesVM.Journal);
            PaperTradesVM.Review = await _unitOfWork.Review.GetAsync(x => x.SampleSizeId == PaperTradesVM.CurrentTrade.SampleSizeId);
            SanitizationHelper.SanitizeObject(PaperTradesVM.Review);

            return View(PaperTradesVM);
        }

        /// <summary>
        ///  Method to process the uploaded .zip file. The .zip file has to have the following structure: PaperTrades\Strategy\TimeFrame\SampleSize\TradeNumber\files. Reviews.odt has to be in the SampleSize folder.
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
                        foreach (var entry in sortedEntries)
                        {
                            // Entry is a folder
                            if (entry.FullName.EndsWith('/') || entry.FullName.EndsWith("\\"))
                            {
                                if (canCreateNewTrade)
                                {
                                    int lastTradeId = 0;
                                    // Loop is running > 1 time (first time trade is null)
                                    if (trade != null)
                                    {
                                        _unitOfWork.PaperTrade.Add(trade);
                                        await _unitOfWork.SaveAsync();
                                        journal.PaperTradeId = trade.Id;
                                        _unitOfWork.Journal.Add(journal);
                                        _unitOfWork.SaveAsync();
                                    }
                                    journal = new Journal();
                                    trade = new PaperTrade();
                                }
                                canCreateNewTrade = false;
                                continue;
                            }
                            // Entry is either a screenshot or a .odt file
                            else
                            {
                                string[] tradeInfo = entry.FullName.Split('/');
                                trade.Strategy = MyEnumConverter.StrategyFromString(tradeInfo[1]).Value;
                                trade.TimeFrame = MyEnumConverter.TimeFrameFromString(tradeInfo[2]).Value;
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
                                    _unitOfWork.SaveAsync();
                                    currentSampleSizeId = (await _unitOfWork.SampleSize.GetAllAsync()).
                                                                                Select(x => x.Id).OrderByDescending(id => id).FirstOrDefault();
                                    // Each sample size has it's own review
                                    Review review = new Review();
                                    review.SampleSizeId = currentSampleSizeId;
                                    _unitOfWork.Review.Add(review);
                                    await _unitOfWork.SaveAsync();
                                }

                                if (!canCreateNewTrade)
                                {
                                    currentFolder = ScreenshotsHelper.CreateScreenshotFolders(tradeInfo, currentFolder, entry.FullName, wwwRootPath, 4);
                                }

                                try
                                {
                                    // Save the file
                                    if (entry.FullName.EndsWith(".png"))
                                    {
                                        entry.ExtractToFile(Path.Combine(currentFolder, entry.Name));
                                        string screenshotName = entry.FullName.Split('/').Last();
                                        string screenshotPath = currentFolder.Replace(wwwRootPath, "").Replace("\\", "/");
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

