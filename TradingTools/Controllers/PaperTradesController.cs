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
using NuGet.ProjectModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


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
        private const TradeType _defaultTradeType = TradeType.PaperTrade;

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
            if (!ModelState.IsValid)
            {
                // Inspect model binding errors here
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string allErrors = string.Join(", ", errors);
                return Json(new { error = allErrors });
            }

            tradeParams.ConvertParamsFromView();

            List<SampleSize> sampleSizes = await GetSampleSizesForTradeParams(tradeParams);
            if (!sampleSizes.Any())
            {
                return Json(new { info = $"No sample sizes for the selected trade paramaters." });
            }
            int sampleSizeId = GetCurrentSampleSizeId();

            List<PaperTrade> listTrades = await GetAllTrades();
            SetCurrentTrade();
            if (PaperTradesVM.CurrentTrade == null)
            {
                return Json(new { info = $"No trades for the selected trade paramaters." });
            }

            if (tradeParams.Status == Status.All)
            {
                await SetViewDataStatusAll(sampleSizes);
            }
            else
            {
                await SetViewDataStatusNotAll(sampleSizes);
            }

            return Json(new { paperTradesVM = PaperTradesVM });


            #region Helper Methods

            async Task<List<PaperTrade>> GetAllTrades()
            {
                List<PaperTrade> list;
                if (tradeParams.Status == Status.All)
                {
                    list = (await _unitOfWork.PaperTrade
                                .GetAllAsync(x => x.TradeType == tradeParams.TradeType && x.SampleSizeId == sampleSizeId))
                                .Where(trade => tradeParams.Status == Status.All || trade.Status == tradeParams.Status)
                                .ToList();
                }
                else
                {
                    list = await _unitOfWork.PaperTrade
                                                    .GetAllAsync(x => x.SampleSize.Strategy == tradeParams.Strategy &&
                                                                      x.TradeType == tradeParams.TradeType &&
                                                                      x.SampleSize.TimeFrame == tradeParams.TimeFrame &&
                                                                      x.Status == tradeParams.Status);
                }

                return list;
            }

            void SetCurrentTrade()
            {
                // If different paramaters were selected
                if (tradeParams.ShowLastTrade)
                {
                    PaperTradesVM.CurrentTrade = listTrades.LastOrDefault();
                }
                else if (listTrades.Count == 1)
                {
                    PaperTradesVM.CurrentTrade = listTrades.FirstOrDefault();
                }
                else if (tradeParams.Status != Status.All)
                {
                    List<PaperTrade> filteredTrades = listTrades.Where(trade => trade.Status == tradeParams.Status).ToList();
                    if (tradeParams.TradeNumber > filteredTrades.Count)
                    {
                        PaperTradesVM.CurrentTrade = filteredTrades.LastOrDefault();
                    }
                    else
                    {
                        PaperTradesVM.CurrentTrade = filteredTrades[tradeParams.TradeNumber - 1];
                    }

                }
                // Same sample size, same paramaters, just another trade number
                else
                {
                    PaperTradesVM.CurrentTrade = listTrades[tradeParams.TradeNumber - 1];
                }
            }

            int GetCurrentSampleSizeId()
            {
                SetCurrentSampleSize();
                int id = SetCurrentSampleSizeNumberAndId();

                return id;

                #region Helpers

                int SetCurrentSampleSizeNumberAndId()
                {
                    List<SampleSize> temp = sampleSizes.Where(sampleSize => sampleSize.TimeFrame == tradeParams.TimeFrame).ToList();
                    if (tradeParams.SampleSizeChanged || !tradeParams.ShowLastTrade)
                    {
                        id = temp[tradeParams.SampleSizeNumber - 1].Id;
                        PaperTradesVM.CurrentSampleSizeNumber = tradeParams.SampleSizeNumber;
                    }
                    else if (!temp.Any())
                    {
                        id = sampleSizes.LastOrDefault()!.Id;
                        PaperTradesVM.CurrentSampleSizeNumber = sampleSizes.Count;
                    }
                    else
                    {
                        id = temp.LastOrDefault()!.Id;
                        PaperTradesVM.CurrentSampleSizeNumber = temp.Count;
                    }

                    return id;
                }

                void SetCurrentSampleSize()
                {
                    List<SampleSize> filteredSampleSizes = sampleSizes.Where(sampleSize => sampleSize.TimeFrame == tradeParams.TimeFrame).ToList();
                    if (filteredSampleSizes.Any())
                    {
                        PaperTradesVM.CurrentSampleSize = filteredSampleSizes[tradeParams.SampleSizeNumber - 1];
                    }
                    else
                    {
                        PaperTradesVM.CurrentSampleSize = sampleSizes.LastOrDefault()!;
                    }
                }

                #endregion

            }

            async Task SetViewDataStatusNotAll(List<SampleSize> sampleSizes)
            {
                PaperTradesVM.NumberSampleSizes = -1;
                PaperTradesVM.TradesInTimeFrame =
                                                (await _unitOfWork.PaperTrade.GetAllAsync(x =>
                                                x.TradeType == tradeParams.TradeType &&
                                                x.SampleSize!.TimeFrame == tradeParams.TimeFrame &&
                                                x.Status == tradeParams.Status))
                                                .Count();
                await SetJournalAndReviewData();
                SetAvailableMenus(sampleSizes);
            }

            async Task SetViewDataStatusAll(List<SampleSize> sampleSizes)
            {
                PaperTradesVM.NumberSampleSizes = sampleSizes.Where(sampleSize => sampleSize.TimeFrame == tradeParams.TimeFrame).Count();
                PaperTradesVM.TradesInSampleSize =
                                                (await _unitOfWork.PaperTrade.GetAllAsync(x =>
                                                x.TradeType == tradeParams.TradeType &&
                                                x.SampleSizeId == sampleSizeId &&
                                                (tradeParams.Status == Status.All || x.Status == tradeParams.Status)))
                                                .Count();

                await SetJournalAndReviewData();
                SetAvailableMenus(sampleSizes);
            }

            async Task SetJournalAndReviewData()
            {
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
            List<SampleSize> sampleSizes = await GetSampleSizesForTradeParams();

            if (!sampleSizes.Any())
            {
                return View(PaperTradesVM);
            }

            bool hasCurrentTrade = await SetViewModelData(sampleSizes);
            if (!hasCurrentTrade)
            {
                PaperTradesVM.ErrorMsg = "No trade for the selected paramaters.";
            }

            return View(PaperTradesVM);
        }

        private void SetAvailableMenus(List<SampleSize> sampleSizes)
        {
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

        private async Task<List<SampleSize>> GetSampleSizesForTradeParams(LoadTradeParams tradeParams = null)
        {
            Status tradeStatus = tradeParams == null ? PaperTradesVM.DefaultTradeStatus : tradeParams.Status;

            List<int> sampleSizeIds = (await _unitOfWork.PaperTrade
                                            .GetAllAsync(trade =>
                                            tradeStatus == Status.All || trade.Status == tradeStatus))
                                            .Select(trade => trade.SampleSizeId)
                                            .Distinct()
                                            .ToList();
            if (sampleSizeIds.Count == 0 && tradeParams == null)
            {
                sampleSizeIds = await GetSampleSizeIdsForAnyTrade();
            }
            List<SampleSize> listSampleSizes = await GetSampleSizes();

            if (CalledFromLoadTrade())
            {
                CheckIfTradeParamTimeframeExistsInSampleSizes();
            }

            return listSampleSizes;

            #region Helper Methods

            async Task<List<SampleSize>> GetSampleSizes()
            {
                List<SampleSize> list = new List<SampleSize>();
                foreach (int id in sampleSizeIds)
                {
                    SampleSize sampleSize = await _unitOfWork.SampleSize.GetAsync(sampleSize => sampleSize.Id == id);
                    list.Add(sampleSize);
                }

                return list;
            }

            bool CalledFromLoadTrade()
            {
                return tradeParams != null;
            }

            void CheckIfTradeParamTimeframeExistsInSampleSizes()
            {
                bool tfFound = false;
                listSampleSizes.ForEach(sampleSize =>
                {
                    if (sampleSize.TimeFrame == tradeParams.TimeFrame)
                    {
                        tfFound = true;
                    }
                });
                if (!tfFound && listSampleSizes.Any())
                {
                    tradeParams.TimeFrame = listSampleSizes.LastOrDefault().TimeFrame;
                }
            }

            async Task<List<int>> GetSampleSizeIdsForAnyTrade()
            {
                return (await _unitOfWork.PaperTrade.GetAllAsync())
                                                        .Select(trade => trade.SampleSizeId)
                                                        .Distinct()
                                                        .ToList();
            }

            #region
        }

        private async Task<bool> SetViewModelData(List<SampleSize> sampleSizes)
        {
            PaperTradesVM.CurrentSampleSize = sampleSizes.Last();

            bool hasCurrentTrade = await SetCurrentTradeValues();
            if (!hasCurrentTrade)
            {
                return false;
            }

            await SetSampleSizeValues();
            PaperTradesVM.TradeData = EntityMapper.EntityToViewModel<Trade, TradeDisplay>(PaperTradesVM.CurrentTrade);
            SetAvailableMenus(sampleSizes);

            return hasCurrentTrade;

            #region Helper Methods

            async Task SetSampleSizeValues()
            {
                PaperTradesVM.NumberSampleSizes =
                    sampleSizes.Where(x => x.Strategy == PaperTradesVM.CurrentSampleSize.Strategy && x.TimeFrame == PaperTradesVM.CurrentSampleSize.TimeFrame)
                               .Count();
                // Get the number of trades for the sample size
                PaperTradesVM.TradesInSampleSize =
                    (await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == PaperTradesVM.CurrentSampleSize.Id)).Count();

                int? reviewID = (await _unitOfWork.SampleSize.GetAsync(x => x.Id == PaperTradesVM.CurrentTrade!.SampleSizeId)).ReviewId;
                PaperTradesVM.CurrentSampleSize.Review = await _unitOfWork.Review.GetAsync(x => x.Id == reviewID);
                SanitizationHelper.SanitizeObject(PaperTradesVM.CurrentSampleSize.Review);
            }

            async Task<bool> SetCurrentTradeValues()
            {
                PaperTradesVM.CurrentTrade =
                (await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == PaperTradesVM.CurrentSampleSize.Id)).LastOrDefault();

                if (PaperTradesVM.CurrentTrade == null)
                {
                    return false;
                }
                PaperTradesVM.CurrentTrade.Journal = await _unitOfWork.Journal.GetAsync(x => x.Id == PaperTradesVM.CurrentTrade.JournalId);
                SanitizationHelper.SanitizeObject(PaperTradesVM.CurrentTrade.Journal);
                PaperTradesVM.CurrentTrade.TradeType = _defaultTradeType;

                return true;
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
        #endregion
    }

    #endregion
}
#endregion