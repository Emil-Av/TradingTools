﻿using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using System.IO.Compression;
using System.Xml.Linq;
using Utilities;
using Models.ViewModels;
using SharedEnums.Enums;
using Shared;
using Models.ViewModels.DisplayClasses;
using NuGet.ProjectModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;


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
        private int _tradeNumberForReviewParsing = 0;
        private List<SampleSize> _allAvailableSampleSizes;

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

            SetCurrentSampleSize();

            List<PaperTrade> listTrades = await GetAllTrades();
            SetCurrentTrade();

            await SetViewData(sampleSizes, listTrades.Count);

            return Json(new { paperTradesVM = PaperTradesVM });


            #region Helper Methods

            void SetCurrentSampleSize()
            {
                if (tradeParams.LoadLastSampleSize)
                {
                    PaperTradesVM.CurrentSampleSize = sampleSizes.Where(sampleSize => sampleSize.TimeFrame == tradeParams.TimeFrame).LastOrDefault();
                    PaperTradesVM.CurrentSampleSizeNumber = sampleSizes.Where(sampleSize => sampleSize.TimeFrame == tradeParams.TimeFrame).Count();
                }
                else if (tradeParams.StatusChanged)
                {
                    PaperTradesVM.CurrentSampleSize = sampleSizes.LastOrDefault();
                    PaperTradesVM.CurrentSampleSizeNumber = sampleSizes.Where(sampleSize => sampleSize.TimeFrame == tradeParams.TimeFrame).Count();
                }
                else
                {
                    PaperTradesVM.CurrentSampleSize = sampleSizes
                                                                .Where(sampleSize => sampleSize.TimeFrame == tradeParams.TimeFrame)
                                                                .ToList()[tradeParams.SampleSizeNumber - 1];
                    PaperTradesVM.CurrentSampleSizeNumber = tradeParams.SampleSizeNumber;
                }
            }

            async Task SetViewData(List<SampleSize> sampleSizes, int tradesInSampleSize)
            {
                if (tradeParams.Status == Status.All)
                {
                    await SetViewDataStatusAll(sampleSizes, tradesInSampleSize);
                }
                else
                {
                    await SetViewDataStatusNotAll(sampleSizes, tradesInSampleSize);
                }
                await SetJournalAndReviewData();
                SetAvailableMenus();
            }

            async Task<List<PaperTrade>> GetAllTrades()
            {
                List<PaperTrade> list;
                if (tradeParams.Status == Status.All)
                {
                    list = (await _unitOfWork.PaperTrade
                                                        .GetAllAsync(x => x.SampleSizeId == PaperTradesVM.CurrentSampleSize.Id))
                                                        .ToList();
                }
                else
                {
                    list = (await _unitOfWork.PaperTrade
                                                        .GetAllAsync(x => x.SampleSizeId == PaperTradesVM.CurrentSampleSize.Id &&
                                                                          x.Status == tradeParams.Status))
                                                                          .ToList();
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

            async Task SetViewDataStatusNotAll(List<SampleSize> sampleSizes, int tradesInSampleSize)
            {
                PaperTradesVM.NumberSampleSizes = sampleSizes.Where(sampleSize => sampleSize.TimeFrame == tradeParams.TimeFrame).Count();
                PaperTradesVM.TradesInSampleSize = tradesInSampleSize;
            }

            async Task SetViewDataStatusAll(List<SampleSize> sampleSizes, int tradesInSampleSize)
            {
                PaperTradesVM.NumberSampleSizes = sampleSizes.Where(sampleSize => sampleSize.TimeFrame == tradeParams.TimeFrame).Count();
                PaperTradesVM.TradesInSampleSize = tradesInSampleSize;
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
            _allAvailableSampleSizes = await GetAllSampleSizes();

            if (!_allAvailableSampleSizes.Any())
            {
                return View(PaperTradesVM);
            }

            bool hasCurrentTrade = await SetViewModelData();
            if (!hasCurrentTrade)
            {
                PaperTradesVM.ErrorMsg = "No trade for the selected paramaters.";
            }

            return View(PaperTradesVM);
        }

        private async Task<List<SampleSize>> GetAllSampleSizes()
        {
            List<int> sampleSizeIds = (await _unitOfWork.PaperTrade
                                        .GetAllAsync())
                                        .Select(trade => trade.SampleSizeId)
                                        .Distinct()
                                        .ToList();

            return await GetSampleSizes(sampleSizeIds);
        }

        private async void SetAvailableMenus()
        {
            if (_allAvailableSampleSizes == null)
            {
                _allAvailableSampleSizes = await _unitOfWork.SampleSize.GetAllAsync();
            }
            foreach (SampleSize sampleSize in _allAvailableSampleSizes)
            {
                if (!PaperTradesVM.AvailableTimeframes.Contains(sampleSize.TimeFrame))
                {
                    PaperTradesVM.AvailableTimeframes.Add(sampleSize.TimeFrame);
                }
                if (!PaperTradesVM.AvailableStrategies.Contains(sampleSize.Strategy))
                {
                    PaperTradesVM.AvailableStrategies.Add(sampleSize.Strategy);
                }
            }
            PaperTradesVM.AvailableTimeframes.Sort();
            PaperTradesVM.AvailableStrategies.Sort();
        }

        private async Task<List<SampleSize>> GetSampleSizesForTradeParams(LoadTradeParams tradeParams)
        {
            List<int> sampleSizeIds = await GetSampleSizeIds(tradeParams);
            List<SampleSize> listSampleSizes = await GetSampleSizes(sampleSizeIds);
            CheckIfTradeParamTimeframeExistsInSampleSizes();

            return listSampleSizes;

            #region Helper Methods

            async Task<List<int>> GetSampleSizeIds(LoadTradeParams tradeParams)
            {
                if (tradeParams.Status == Status.All)
                {
                    return (await _unitOfWork.PaperTrade
                                        .GetAllAsync(trade =>
                                                        trade.SampleSize.Strategy == tradeParams.Strategy &&
                                                        trade.TradeType == tradeParams.TradeType &&
                                                        tradeParams.Status == Status.All &&
                                                        trade.SampleSize.TimeFrame == tradeParams.TimeFrame))
                                                        .Select(trade => trade.SampleSizeId)
                                                        .Distinct()
                                                        .ToList();
                }
                else
                {
                    return (await _unitOfWork.PaperTrade
                                        .GetAllAsync(trade =>
                                                        trade.SampleSize.Strategy == tradeParams.Strategy &&
                                                        trade.TradeType == tradeParams.TradeType &&
                                                        trade.Status == tradeParams.Status))
                                                        .Select(trade => trade.SampleSizeId)
                                                        .Distinct()
                                                        .ToList();
                }
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

            #region
        }

        private async Task<List<SampleSize>> GetSampleSizes(List<int> sampleSizeIds)
        {
            List<SampleSize> list = new List<SampleSize>();
            foreach (int id in sampleSizeIds)
            {
                SampleSize sampleSize = await _unitOfWork.SampleSize.GetAsync(sampleSize => sampleSize.Id == id);
                list.Add(sampleSize);
            }

            return list;
        }

        private async Task<bool> SetViewModelData()
        {
            PaperTradesVM.CurrentSampleSize = _allAvailableSampleSizes.Last();

            bool hasCurrentTrade = await SetCurrentTradeValues();
            if (!hasCurrentTrade)
            {
                return false;
            }

            await SetMenuSampleSizeValues();
            PaperTradesVM.TradeData = EntityMapper.EntityToViewModel<Trade, TradeDisplay>(PaperTradesVM.CurrentTrade);
            SetAvailableMenus();

            return hasCurrentTrade;

            #region Helper Methods

            async Task SetMenuSampleSizeValues()
            {
                PaperTradesVM.NumberSampleSizes =
                    _allAvailableSampleSizes.Where(x => x.Strategy == PaperTradesVM.CurrentSampleSize.Strategy &&
                                                   x.TimeFrame == PaperTradesVM.CurrentSampleSize.TimeFrame)
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
                        Review review = null;
                        List<ZipArchiveEntry> sortedEntries = [.. archive.Entries.OrderBy(e => e.FullName, new NaturalStringComparer())];
                        int count = sortedEntries.Count;
                        int currentIndex = 0;
                        foreach (var entry in sortedEntries)
                        {
                            bool isLastEntry = currentIndex == count - 1;
                            // Entry is a folder
                            if (entry.FullName.EndsWith('/') || entry.FullName.EndsWith("\\") || isLastEntry)
                            {
                                if (canCreateNewTrade || isLastEntry)
                                {
                                    int lastTradeId = 0;
                                    // Loop is running > 1 time (first time trade is null)
                                    if (trade != null)
                                    {
                                        _unitOfWork.Journal.Add(journal);
                                        await _unitOfWork.SaveAsync();
                                        trade.JournalId = journal.Id;
                                        trade.SampleSizeId = sampleSize.Id;
                                        _unitOfWork.PaperTrade.Add(trade);
                                        await _unitOfWork.SaveAsync();
                                    }
                                    journal = new Journal();
                                    trade = new PaperTrade();
                                    trade.TradeType = TradeType.PaperTrade;
                                }
                                canCreateNewTrade = false;
                                currentIndex++;
                                continue;
                            }

                            // Entry is either a screenshot or a .odt file
                            else
                            {
                                string[] tradeInfo = entry.FullName.Split('/');
                                //trade.SampleSizeId = currentSampleSizeId;
                                string currentSampleSize = tradeInfo[3];
                                string currentTimeFrame = tradeInfo[2];
                                // First sample size of the loop or a new one
                                if (lastSampleSize != currentSampleSize || lastTimeFrame != currentTimeFrame)
                                {
                                    lastTimeFrame = currentTimeFrame;
                                    lastSampleSize = currentSampleSize;
                                    sampleSize = new();
                                    await SetSampleSizeValues(sampleSize, tradeInfo);
                                    review = new Review();
                                    _unitOfWork.Review.Add(review);
                                    await _unitOfWork.SaveAsync();
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
                                        if (trade.ScreenshotsUrls == null)
                                        {
                                            trade.ScreenshotsUrls = new();
                                        }
                                        trade.ScreenshotsUrls.Add(Path.Combine(screenshotPath, screenshotName));
                                    }
                                    else if (entry.FullName.Contains("Reviews"))
                                    {
                                        await ParseODTReviewsFile(entry, review);
                                    }
                                    else
                                    {
                                        ParseODTJournalFile(entry, journal);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Display error message
                                }
                                // The Review file is in SampleSize\Reviews.odt folder, not in the individual trade's folder. Don't create a new trade record in the database for that iteration
                                if (!entry.FullName.Contains("Reviews"))
                                {
                                    canCreateNewTrade = true;
                                }
                            }
                            currentIndex++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in parsing the .zip file: {ex.Message}");
            }

            return RedirectToAction("Index");


            #region Helper Methods

            async Task SetSampleSizeValues(SampleSize sampleSize, string[] tradeInfo)
            {
                Strategy strategy = MyEnumConverter.StrategyFromString(tradeInfo[1]).Value;
                TimeFrame timeFrame = MyEnumConverter.TimeFrameFromString(tradeInfo[2]).Value;
                sampleSize.Strategy = strategy;
                sampleSize.TimeFrame = timeFrame;
                sampleSize.TradeType = TradeType.PaperTrade;
                _unitOfWork.SampleSize.Add(sampleSize);
                await _unitOfWork.SaveAsync();
            }

            #endregion
        }

        private async Task ParseODTReviewsFile(ZipArchiveEntry entry, Review review)
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
                            string lastReview = string.Empty;
                            int tradeNumber = 0;
                            foreach (XElement node in nodes)
                            {
                                XElement element = XElement.Parse(node.ToString());
                                await SaveReview(review, element.Value);
                            }
                        }
                    }
                }
            }

            async Task SaveReview(Review review, string reviewEntry)
            {
                if (string.IsNullOrEmpty(reviewEntry))
                {
                    return;
                }

                if (reviewEntry.StartsWith("[Trade"))
                {
                    _tradeNumberForReviewParsing = GetTradeNumber(reviewEntry);
                }
                else if (reviewEntry.Contains("Summary"))
                {
                    _tradeNumberForReviewParsing = 21;
                }

                if (_tradeNumberForReviewParsing <= 5)
                {
                    review.First += reviewEntry + Environment.NewLine;
                }
                else if (_tradeNumberForReviewParsing <= 10)
                {
                    review.Second += reviewEntry + Environment.NewLine;
                }
                else if (_tradeNumberForReviewParsing <= 15)
                {
                    review.Third += reviewEntry + Environment.NewLine;
                }
                else if (_tradeNumberForReviewParsing <= 20)
                {
                    review.Forth += reviewEntry + Environment.NewLine;
                }
                else
                {
                    review.Summary += reviewEntry + Environment.NewLine;
                }
                await _unitOfWork.SaveAsync();
            }

            int GetTradeNumber(string reviewEntry)
            {
                // Regular expression to match [Trade X] where X is a number
                /*
                   - \[Trade: Matches the literal text [Trade.
                   - \s?: Matches zero or one whitespace character, allowing for either [TradeX] or [Trade X].
                   - \d+: Matches one or more digits (X being the integer).
                   - (\d+): Wraps the digits in parentheses to define a capturing group. This capturing group will store the value of X.
                   - \]: Matches the closing bracket ].
                 */
                Regex regex = new Regex(@"\[Trade\s?(\d+)\]");
                Match match = regex.Match(reviewEntry);

                if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
                {
                    return number;
                }
                else
                {
                    throw new FormatException("Input does not match the expected format [Trade X]");
                }
            }
        }

        void ParseODTJournalFile(ZipArchiveEntry entry, Journal journal)
        {
            using (Stream stream = entry.Open())
            {
                using (ZipArchive archive = new ZipArchive(stream))
                {
                    ZipArchiveEntry? contentEntry = archive.GetEntry("content.xml");
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