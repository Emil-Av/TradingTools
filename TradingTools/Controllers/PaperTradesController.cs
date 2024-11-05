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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using Models.ViewModels.DisplayClasses;


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
        public async Task<IActionResult> UpdateTradeData([FromBody] TradeDisplay tradeData)
        {
            if (!ModelState.IsValid)
            {
                // Inspect model binding errors here
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string allErrors = string.Join(", ", errors);
                return Json(new { error = allErrors });
            }

            try
            {
                PaperTrade trade = await _unitOfWork.PaperTrade.GetAsync(x => x.Id == int.Parse(tradeData.IdDisplay));
                trade = EntityMapper.ViewModelToEntity(tradeData, trade);
                await _unitOfWork.PaperTrade.UpdateAsync(trade);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return Json(new { error = $"An error occured while updating the trade: {ex.Message}" });
            }

            return Json(new { success = "Trade updated" });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateReview([FromBody] PaperTradesVM data)
        {
            if (!ModelState.IsValid)
            {
                // Inspect model binding errors here
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string allErrors = string.Join(", ", errors);
                return Json(new { error = allErrors });
            }
            if (!CanUpdateReview(out string errorMsg))
            {
                return Json(new { error = errorMsg });
            }


            SanitizationHelper.SanitizeObject(data.CurrentSampleSize.Review);
            Review review = await _unitOfWork.Review.GetAsync(x => x.Id == data.CurrentSampleSize.Review.Id);
            if (review != null)
            {
                SetReviewValues();
                await _unitOfWork.Review.UpdateAsync(review);
                await _unitOfWork.SaveAsync();

                return Json(new { success = "Review updated." });
            }
            else
            {
                return Json(new { error = $"The review for sample size with ID {data.CurrentTrade.SampleSizeId} wasn't found in the data base." });
            }

            #region Helper Methods

            void SetReviewValues()
            {
                review.First = data.CurrentSampleSize.Review.First;
                review.Second = data.CurrentSampleSize.Review.Second;
                review.Third = data.CurrentSampleSize.Review.Third;
                review.Forth = data.CurrentSampleSize.Review.Forth;
                review.Summary = data.CurrentSampleSize.Review.Summary;
            }

            bool CanUpdateReview(out string errorMsg)
            {
                if (data.CurrentSampleSize == null)
                {
                    errorMsg = "CurrentSampleSize is null.";
                    return false;
                }
                else if (data.CurrentSampleSize.Id == 0)
                {
                    errorMsg = "SampleSize Id is 0";
                    return false;
                }
                else if (data.CurrentSampleSize.Review == null)
                {
                    errorMsg = "Review is null.";
                    return false;
                }
                else if (data.CurrentSampleSize.Review.Id == 0)
                {
                    errorMsg = "Review Id is 0";
                    return false;
                }
                errorMsg = string.Empty;

                return true;
            }

            #endregion
        }

        [HttpPost]
        public async Task<IActionResult> UpdateJournal([FromBody] PaperTradesVM data)
        {
            if (data.CurrentTrade.Journal == null)
            {
                // Notification error
                return Json(new { error = "Journal wasn't updated. Journal was null." });
            }

            SanitizationHelper.SanitizeObject(data.CurrentTrade.Journal);

            Journal journal = await _unitOfWork.Journal.GetAsync(x => x.Id == data.CurrentTrade.JournalId);
            if (journal != null)
            {
                SetJournalValues();
                await _unitOfWork.Journal.UpdateAsync(journal);
                await _unitOfWork.SaveAsync();
            }
            return Json(new { success = "Journal updated." });

            #region Helper Methods

            void SetJournalValues()
            {
                journal.Pre = data.CurrentTrade.Journal.Pre;
                journal.During = data.CurrentTrade.Journal.During;
                journal.Exit = data.CurrentTrade.Journal.Exit;
                journal.Post = data.CurrentTrade.Journal.Post;
            }

            #endregion
        }

        public async Task<IActionResult> LoadTrade(LoadTradeParams tradeParams)
        {
            if (tradeParams != null)
            {
                tradeParams.ConvertValuesFromView();
            }
            else
            {
                return Json(new { error = "tradeParams was null." });
            }


            List<SampleSize> listSampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.Strategy == tradeParams.Strategy && x.TimeFrame == tradeParams.TimeFrame && x.TradeType == tradeParams.TradeType);
            // If no sample size is found for the strategy and time frame, there are no trades for this params
            if (listSampleSizes.Count == 0)
            {
                return Json(new { info = $"No trades for the selected trade paramaters." });
            }
            int sampleSizeId = GetCurrentSampleSizeId();

            List<PaperTrade> sampleSizeTrades = await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == sampleSizeId);
            
            SetCurrentTrade();
            if (PaperTradesVM.CurrentTrade == null)
            {
                return Json(new { info = $"No trades for the selected trade paramaters." });
            }

            SetAjaxResponseValues();

            // Send the response
            return Json(new { paperTradesVM = PaperTradesVM });

            #region Helper Methods

            void SetCurrentTrade()
            {
                // If different paramaters were selected
                if (tradeParams.ShowLastTrade)
                {
                    PaperTradesVM.CurrentTrade = sampleSizeTrades.LastOrDefault(x => x.Status == tradeParams.Status && x.TradeType == tradeParams.TradeType && x.SideType == tradeParams.SideType && x.OrderType == tradeParams.OrderType);
                }
                // Same sample size, same paramaters, just another trade number
                else
                {
                    PaperTradesVM.CurrentTrade = sampleSizeTrades[tradeParams.TradeNumber - 1];
                }
            }

            int GetCurrentSampleSizeId()
            {
                int sampleSizeId;
                PaperTradesVM.CurrentSampleSize = listSampleSizes[tradeParams.SampleSizeNumber - 1];
                if (tradeParams.SampleSizeChanged || !tradeParams.ShowLastTrade)
                {
                    sampleSizeId = listSampleSizes[tradeParams.SampleSizeNumber - 1].Id;
                    PaperTradesVM.CurrentSampleSizeNumber = tradeParams.SampleSizeNumber;
                }
                else
                {
                    sampleSizeId = listSampleSizes.LastOrDefault().Id;
                    PaperTradesVM.CurrentSampleSizeNumber = listSampleSizes.Count;
                }

                return sampleSizeId;
            }

            async void SetAjaxResponseValues()
            {
                PaperTradesVM.NumberSampleSizes = listSampleSizes.Count();
                PaperTradesVM.TradesInSampleSize = (await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == sampleSizeId)).Count();
                PaperTradesVM.CurrentTrade.Journal = await _unitOfWork.Journal.GetAsync(x => x.Id == PaperTradesVM.CurrentTrade!.JournalId);
                SanitizationHelper.SanitizeObject(PaperTradesVM.CurrentTrade.Journal);
                int? reviewID = (await _unitOfWork.SampleSize.GetAsync(x => x.Id == PaperTradesVM.CurrentTrade!.SampleSizeId)).ReviewId;
                PaperTradesVM.CurrentSampleSize.Review = await _unitOfWork.Review.GetAsync(x => x.Id == reviewID);
                SanitizationHelper.SanitizeObject(PaperTradesVM.CurrentSampleSize.Review);
            }

            #endregion
        }
        public async Task<IActionResult> Index()
        {
            // Get the latest sample size for the strategy and time frame
            List<SampleSize> sampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.TradeType == TradeType.PaperTrade);

            if (!sampleSizes.Any())
            {
                return View(PaperTradesVM);
            }

            string errorMsg = await SetViewModelData(sampleSizes);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                PaperTradesVM.ErrorMsg = errorMsg;
            }

            return View(PaperTradesVM);
        }

        private async Task<string> SetViewModelData(List<SampleSize> sampleSizes)
        {
            string errorMsg = string.Empty;
            PaperTradesVM.CurrentSampleSize = sampleSizes.Last();
            // Get the last trade of the sample size
            PaperTradesVM.CurrentTrade =
                (await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == PaperTradesVM.CurrentSampleSize.Id)).LastOrDefault()!;
            // No trades yet
            if (PaperTradesVM.CurrentTrade == null)
            {
                return string.Empty;
            }

            // Get the number of sample sizes for the time frame and strategy
            PaperTradesVM.NumberSampleSizes = sampleSizes.Where(x => x.Strategy == PaperTradesVM.CurrentSampleSize.Strategy && x.TimeFrame == PaperTradesVM.CurrentSampleSize.TimeFrame).Count();
            // Get the number of trades for the sample size
            PaperTradesVM.TradesInSampleSize =
                (await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == PaperTradesVM.CurrentSampleSize.Id)).Count();
            PaperTradesVM.CurrentTrade.Journal = await _unitOfWork.Journal.GetAsync(x => x.Id == PaperTradesVM.CurrentTrade.JournalId);
            SanitizationHelper.SanitizeObject(PaperTradesVM.CurrentTrade.Journal);
            int? reviewID = (await _unitOfWork.SampleSize.GetAsync(x => x.Id == PaperTradesVM.CurrentTrade!.SampleSizeId)).ReviewId;
            PaperTradesVM.CurrentSampleSize.Review = await _unitOfWork.Review.GetAsync(x => x.Id == reviewID);
            SanitizationHelper.SanitizeObject(PaperTradesVM.CurrentSampleSize.Review);
            PaperTradesVM.TradeData = EntityMapper.EntityToViewModel<Trade, TradeDisplay>(PaperTradesVM.CurrentTrade);

            SetAvailableMenus();

            return errorMsg;

            #region Helper Methods

            void SetAvailableMenus()
            {
                // Set Available TimeFrames and Strategies
                foreach (SampleSize sampleSize in sampleSizes)
                {
                    if (!PaperTradesVM.AvailableTimeframes.Contains(sampleSize.TimeFrame))
                    {
                        PaperTradesVM.AvailableTimeframes.Add(sampleSize.TimeFrame);
                    }
                    // Set the time frames in ascending order
                    PaperTradesVM.AvailableTimeframes.Sort();

                    if (!PaperTradesVM.AvailableStrategies.Contains(sampleSize.Strategy))
                    {
                        PaperTradesVM.AvailableStrategies.Add(sampleSize.Strategy);
                    }
                    // Sort the strategies in ascending order
                    PaperTradesVM.AvailableStrategies.Sort();
                }
            }

            #endregion
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
                        List<ZipArchiveEntry> sortedEntries = [.. archive.Entries.OrderBy(e => e.FullName, new NaturalStringComparer())];
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
                                        trade.JournalId = journal.Id;
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
                                trade.SampleSizeId = currentSampleSizeId;


                                string currentSampleSize = tradeInfo[3];
                                string currentTimeFrame = tradeInfo[2];
                                // First sample size of the loop or a new one
                                if (lastSampleSize != currentSampleSize || lastTimeFrame != currentTimeFrame)
                                {
                                    lastTimeFrame = currentTimeFrame;
                                    lastSampleSize = currentSampleSize;
                                    sampleSize = new SampleSize();
                                    //sampleSize.Strategy = (Strategy)trade.Strategy;
                                    //sampleSize.TimeFrame = (TimeFrame)trade.TimeFrame;
                                    _unitOfWork.SampleSize.Add(sampleSize);

                                    // Each sample size has it's own review
                                    Review review = new Review();
                                    _unitOfWork.Review.Add(review);
                                    sampleSize.ReviewId = review.Id;
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
                            List<XElement> nodes = [.. xmlDoc.Descendants().Where(e => e.Name.LocalName == "p")];
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

