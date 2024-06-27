using DataAccess.Repository.IRepository;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;
using Models;
using Models.ViewModels;
using Models.ViewModels.DisplayClasses;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;
using Utilities;
using SharedEnums.Enums;

namespace TradingTools.Controllers
{
    public class ResearchController : Controller
    {
        #region Constructor
        public ResearchController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            ResearchVM = new ResearchVM();
            ResearchVM.AllTrades = new List<ResearchFirstBarPullbackDisplay>();
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        #region Private Properties

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion

        #region Public Properties

        public ResearchVM ResearchVM { get; set; }

        #endregion

        #region Methods

        [HttpPost]
        public async Task<IActionResult> UpdateTrade([FromBody] ResearchFirstBarPullbackDisplay currentTrade)
        {
            if (currentTrade == null)
            {
                return Json(new { error = "CurrentTrade is null." });
            }

            ResearchFirstBarPullback trade = ResearchMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(currentTrade);
            // The Id of a trade is in the currentTrade paramater. The id is passed to the trade object in ResearchMapper.ViewModelToEntity().
            // The Update() method, queries the database for a trade based on the Id.
            _unitOfWork.ResearchFirstBarPullback.Update(trade);
            _unitOfWork.Save();

            return Json(new { success = "Trade was successfully updated" });
        }


        public async Task<IActionResult> Index()
        {
            // Get research sample sizes
            List<SampleSize> sampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.TradeType == TradeType.Research);

            // Set the NumberSampleSizes for the button menu
            ResearchVM.NumberSampleSizes = sampleSizes.Count();
            if (ResearchVM.NumberSampleSizes == 0)
            {
                return View(ResearchVM);
            }
            int lastSampleSizeId = sampleSizes.LastOrDefault().Id;
            // Get all researched trades from the DB and project the instances into ResearchFirstBarPullbackDisplay
            ResearchVM.AllTrades = (await _unitOfWork.ResearchFirstBarPullback
                                    .GetAllAsync(x => x.SampleSizeId == lastSampleSizeId))
                                    .Select(x => ResearchMapper.EntityToViewModel<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(x))
                                    .ToList();

            // Should not happen
            if (!ResearchVM.AllTrades.Any())
            {
                return View(ResearchVM);
            }
            ResearchVM.CurrentTrade = ResearchVM.AllTrades.FirstOrDefault();
            ResearchVM.CurrentSampleSize = sampleSizes.LastOrDefault();

            // Set the values for the button menus. Display only values for which there are data records.
            foreach (SampleSize sampleSize in sampleSizes)
            {
                if (!ResearchVM.AvailableTimeframes.Contains(sampleSize.TimeFrame))
                {
                    ResearchVM.AvailableTimeframes.Add(sampleSize.TimeFrame);
                }

                if (!ResearchVM.AvailableStrategies.Contains(sampleSize.Strategy))
                {
                    ResearchVM.AvailableStrategies.Add(sampleSize.Strategy);
                }
            }
            return View(ResearchVM);
        }
        /// <summary>
        ///  The .zip file has to have the following format: Research/Sample Size 1/Trades folder and .csv file. The .csv file has to have format Research-EnumStrategy(e.g. 0)-EnumTimeFrame(e.g. 10M): e.g. Research-0-10M.csv
        /// </summary>
        /// <param name="zipFile"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> UploadResearch(IFormFile zipFile)
        {
            if (zipFile == null || zipFile.Length == 0)
            {
                // return notification error
            }
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                using (var zipStream = new MemoryStream())
                {
                    await zipFile.CopyToAsync(zipStream);
                    zipStream.Position = 0; // Reset the stream position to the beginning

                    using (var archive = new ZipArchive(zipStream))
                    {
                        // Sort the entries to have the folders in ascending order (Trade 1, Trade 2..)
                        List<ZipArchiveEntry> sortedEntries = archive.Entries.OrderBy(e => e.FullName, new NaturalStringComparer()).ToList();
                        ResearchFirstBarPullback researchTrade = null;

                        TimeFrame researchedTF;
                        int tradeIndex = 0;
                        int currentTrade = 0;
                        string currentSampleSize = string.Empty;
                        string currentFolder = string.Empty;
                        foreach (var entry in sortedEntries)
                        {
                            if (entry.FullName.Contains("Sample Size") && string.IsNullOrEmpty(currentSampleSize))
                            {
                                string[] researchInfo = entry.FullName.Split('/');
                                if (researchInfo[1].Contains("Sample Size"))
                                {
                                    currentSampleSize = researchInfo[1];
                                    continue;
                                }

                            }
                            if (entry.FullName.Contains(".csv"))
                            {
                                string[] researchInfo = entry.FullName.Split('-');
                                if (!Int32.TryParse(researchInfo[1], out int strategy))
                                {
                                    TempData["error"] = "Error parsing the strategy number from the csv file name. Check the file name.";
                                    return RedirectToAction(nameof(Index));
                                }
                                string tempTF = researchInfo[2].Replace(".csv", "");
                                researchedTF = MyEnumConverter.SetTimeFrameFromString(tempTF);
                                // Set the sample size for the research
                                SampleSize sampleSize = new SampleSize();
                                sampleSize.TradeType = TradeType.Research;
                                sampleSize.Strategy = (Strategy)strategy;
                                sampleSize.TimeFrame = researchedTF;
                                _unitOfWork.SampleSize.Add(sampleSize);
                                _unitOfWork.Save();
                                int sampleSizeId = (await _unitOfWork.SampleSize
                                    .GetAllAsync(x => x.TradeType == TradeType.Research && x.Strategy == (Strategy)strategy && x.TimeFrame == researchedTF))
                                    .OrderByDescending(x => x.Id)
                                    .Select(x => x.Id)
                                    .FirstOrDefault();


                                // Parse the .csv data
                                using (var csvStream = entry.Open())
                                {
                                    var csvData = await ReadCsvFileAsync(csvStream);
                                    if (csvData != null)
                                    {

                                        for (int i = 0; i < csvData.Count; i++)
                                        {
                                            // First row is column names
                                            if (i == 0)
                                            {
                                                continue;
                                            }
                                            // Half ATR
                                            if (i % 2 != 0)
                                            {
                                                researchTrade = new ResearchFirstBarPullback();
                                                researchTrade.SampleSizeId = sampleSizeId;
                                                researchTrade.OneToOneHitOn = csvData[i][1].Length > 0 ? int.Parse(csvData[i][1]) : 0;
                                                researchTrade.IsOneToThreeHit = csvData[i][2] == "Yes" ? true : false;
                                                researchTrade.IsOneToFiveHit = csvData[i][3] == "Yes" ? true : false;
                                                researchTrade.IsBreakeven = csvData[i][4] == "Yes" ? true : false;
                                                researchTrade.IsLoss = csvData[i][5] == "Yes" ? true : false;
                                                // Format in csvData[i][6] is 1-4. Split the string at '-` and get the second item. Then parse that into int.
                                                researchTrade.MaxRR = csvData[i][6].Length > 0 ? int.Parse(csvData[i][6].Split('-')[1]) : 0;
                                                researchTrade.MarketGaveSmth = csvData[i][7].Length > 0 ? true : false;
                                                researchTrade.IsEntryAfter3To5Bars = csvData[i][8] == "Yes" ? true : false;
                                                researchTrade.IsEntryAfter5Bars = csvData[i][9] == "Yes" ? true : false;
                                                researchTrade.IsEntryAtPreviousSwingOnTrigger = csvData[i][10] == "Yes" ? true : false;
                                                researchTrade.IsEntryBeforePreviousSwingOnTrigger = csvData[i][11] == "Yes" ? true : false;
                                                researchTrade.IsEntryBeforePreviousSwingOn4H = csvData[i][12] == "Yes" ? true : false;
                                                researchTrade.IsEntryBeforePreviousSwingOnD = csvData[i][13] == "Yes" ? true : false;
                                                researchTrade.IsMomentumTrade = csvData[i][14] == "Yes" ? true : false;
                                                researchTrade.IsTriggerTrending = csvData[i][15] == "Yes" ? true : false;
                                                researchTrade.Is4HTrending = csvData[i][16] == "Yes" ? true : false;
                                                researchTrade.IsDTrending = csvData[i][17] == "Yes" ? true : false;
                                                researchTrade.IsEntryAfteriBar = csvData[i][18] == "Yes" ? true : false;
                                                researchTrade.IsSignalBarStrongReversal = csvData[i][18] == "Yes" ? true : false;
                                                researchTrade.IsSignalBarInTradeDirection = csvData[i][19] == "Yes" ? true : false;
                                                researchTrade.Comment = csvData[i][22];
                                            }
                                            // Full ATR
                                            else
                                            {
                                                researchTrade.FullATROneToOneHitOn = csvData[i][1].Length > 0 ? int.Parse(csvData[i][1]) : 0;
                                                researchTrade.IsFullATROneToThreeHit = csvData[i][2] == "Yes" ? true : false;
                                                researchTrade.IsFullATROneToFiveHit = csvData[i][3] == "Yes" ? true : false;
                                                researchTrade.IsFullATRBreakeven = csvData[i][4] == "Yes" ? true : false;
                                                researchTrade.IsFullATRLoss = csvData[i][5] == "Yes" ? true : false;
                                                researchTrade.FullATRMaxRR = csvData[i][6].Length > 0 ? int.Parse(csvData[i][6].Split('-')[1]) : 0;
                                                researchTrade.MarketGaveSmth = csvData[i][7].Length > 0 ? true : false;
                                                _unitOfWork.ResearchFirstBarPullback.Add(researchTrade);
                                                _unitOfWork.Save();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Inside the folder with the screenshots
                                if (entry.FullName.EndsWith(".png"))
                                {
                                    // Split the name to get the folder's name
                                    string[] tradeInfo = entry.FullName.Split('/');
                                    // What is left is "Trade x", x is the number of the trade. Remove "Trade" to get the number.
                                    string tempTradeInfo = tradeInfo[2].Replace("Trade", "").Trim();
                                    if (Int32.TryParse(tempTradeInfo, out int tempTradeIndex))
                                    {
                                        tradeIndex = tempTradeIndex - 1;
                                    }
                                    currentFolder = AppHelper.CreateScreenshotFolders(tradeInfo, currentFolder, entry.FullName, wwwRootPath, 2);
                                    List<int> tradeIds = (await _unitOfWork.ResearchFirstBarPullback.GetAllAsync()).Select(x => x.Id).ToList();

                                    // Get the trade for the screenshot of the current iteration
                                    ResearchFirstBarPullback trade = await _unitOfWork.ResearchFirstBarPullback.GetAsync(x => x.Id == tradeIds[tradeIndex]);
                                    if (trade.ScreenshotsUrls == null)
                                    {
                                        trade.ScreenshotsUrls = new List<string>();
                                    }

                                    entry.ExtractToFile(Path.Combine(currentFolder, entry.Name));
                                    string screenshotName = entry.FullName.Split('/').Last();
                                    string screenshotPath = currentFolder.Replace(wwwRootPath, "").Replace("\\\\", "/");
                                    trade.ScreenshotsUrls.Add(Path.Combine(screenshotPath, screenshotName));
                                    _unitOfWork.ResearchFirstBarPullback.Update(trade);
                                    _unitOfWork.Save();

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error parsing the csv file. Error message: {ex.Message}");
                TempData["error"] = $"Error parsing the csv file. Error message: {ex.Message}";
            }

            async Task<List<List<string>>> ReadCsvFileAsync(Stream csvStream)
            {
                var result = new List<List<string>>();
                using (var reader = new StreamReader(csvStream))
                {
                    while (await reader.ReadLineAsync() is string line)
                    {
                        var values = line.Split(',').ToList();
                        result.Add(values);
                    }
                }

                return result;
            }

            return RedirectToAction(nameof(Index));
        }


        #region Helper Methods
        /// <summary>
        ///  Converts the values from the view into the reference of the database object.
        /// </summary>
        /// <param name="objFromDB"></param>
        /// <param name="objFromView"></param>
        /// <returns></returns>
        private ResearchFirstBarPullback ConvertToDbResearchValues(ResearchFirstBarPullback objFromDB, ResearchFirstBarPullbackDisplay objFromView)
        {
            objFromDB.SideType = objFromView.SideTypeDisplay;
            if (Int32.TryParse(objFromView.OneToOneHitOnDisplay, out int oneToOne))
            {
                objFromDB.OneToOneHitOn = oneToOne;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsOneToThreeHitDisplay, out bool oneToThree))
            {
                objFromDB.IsOneToThreeHit = oneToThree;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsOneToFiveHitDisplay, out bool oneToFive))
            {
                objFromDB.IsOneToFiveHit = oneToFive;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsBreakevenDisplay, out bool breakEven))
            {
                objFromDB.IsBreakeven = breakEven;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsLossDisplay, out bool lossDisplay))
            {
                objFromDB.IsLoss = lossDisplay;
            }
            else
            {

            }

            objFromDB.MaxRR = objFromView.MaxRRDisplay;

            if (bool.TryParse(objFromView.MarketGaveSmthDisplay, out bool marketGaveSmth))
            {
                objFromDB.MarketGaveSmth = marketGaveSmth;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsEntryAfter3To5BarsDisplay, out bool entryAfter3To5Bars))
            {
                objFromDB.IsEntryAfter3To5Bars = entryAfter3To5Bars;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsEntryAfter5BarsDisplay, out bool entryAfter5bars))
            {
                objFromDB.IsEntryAfter5Bars = entryAfter5bars;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsEntryAtPreviousSwingOnTriggerDisplay, out bool entryAtPreviousSwingOnTrigger))
            {
                objFromDB.IsEntryAtPreviousSwingOnTrigger = entryAtPreviousSwingOnTrigger;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsEntryBeforePreviousSwingOnTriggerDisplay, out bool entryBeforePreviousSwingOnTriggerDisplay))
            {
                objFromDB.IsEntryBeforePreviousSwingOnTrigger = entryBeforePreviousSwingOnTriggerDisplay;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsEntryBeforePreviousSwingOn4HDisplay, out bool isEntryBeforePreviousSwingOn4H))
            {
                objFromDB.IsEntryBeforePreviousSwingOn4H = isEntryBeforePreviousSwingOn4H;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsEntryBeforePreviousSwingOnDDisplay, out bool isEntryBeforePreviousSwingOnD))
            {
                objFromDB.IsEntryBeforePreviousSwingOnD = isEntryBeforePreviousSwingOnD;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsMomentumTradeDisplay, out bool isMomentumTrade))
            {
                objFromDB.IsMomentumTrade = isMomentumTrade;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsTrendTradeDisplay, out bool isTrendTradeDisplay))
            {
                objFromDB.IsTrendTrade = isTrendTradeDisplay;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsTriggerTrendingDisplay, out bool isTriggerTrendingDisplay))
            {
                objFromDB.IsTriggerTrending = isTriggerTrendingDisplay;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.Is4HTrendingDisplay, out bool is4HTrendingDisplay))
            {
                objFromDB.Is4HTrending = is4HTrendingDisplay;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsDTrendingDisplay, out bool isDTrending))
            {
                objFromDB.IsDTrending = isDTrending;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsEntryAfteriBarDisplay, out bool isEntryAfteriBarDisplay))
            {
                objFromDB.IsEntryAfteriBar = isEntryAfteriBarDisplay;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsSignalBarStrongReversalDisplay, out bool isSignalBarStrongReversalDisplay))
            {
                objFromDB.IsSignalBarStrongReversal = isSignalBarStrongReversalDisplay;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsSignalBarInTradeDirectionDisplay, out bool isSignalBarInTradeDirectionDisplay))
            {
                objFromDB.IsSignalBarInTradeDirection = isSignalBarInTradeDirectionDisplay;
            }
            else
            {

            }

            objFromDB.FullATROneToOneHitOn = objFromView.FullATROneToOneHitOnDisplay;

            if (bool.TryParse(objFromView.IsFullATROneToThreeHitDisplay, out bool isFullATROneToThreeHit))
            {
                objFromDB.IsFullATROneToThreeHit = isFullATROneToThreeHit;
            }
            else
            {

            }

            if (bool.TryParse(objFromView.IsFullATROneToFiveHitDisplay, out bool isFullATROneToFiveHit))
            {
                objFromDB.IsFullATROneToFiveHit = isFullATROneToFiveHit;
            }
            else
            {

            }
            if (bool.TryParse(objFromView.IsFullATRBreakevenDisplay, out bool isATRBreakevenDisplay))
            {
                objFromDB.IsFullATRBreakeven = isATRBreakevenDisplay;
            }
            else
            {

            }

            if (bool.TryParse(objFromView.IsFullATRLossDisplay, out bool isATRLossDisplay))
            {
                objFromDB.IsFullATRLoss = isATRLossDisplay;
            }
            else
            {

            }

            objFromDB.FullATRMaxRR = objFromView.FullATRMaxRRDisplay;
            objFromDB.FullATRMarketGaveSmth = objFromView.FullATRMarketGaveSmthDisplay;
            objFromDB.Comment = objFromView.CommentDisplay;

            return objFromDB;
        }
        #endregion

        #endregion
    }
}
