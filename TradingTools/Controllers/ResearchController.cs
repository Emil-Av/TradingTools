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
using Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Newtonsoft.Json.Serialization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Collections.Generic;

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

        #region Private Properties

        private const int IndexMethod = 0;

        #endregion

        #region Methods

        #region Public Methods

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (ResearchVM.CurrentStrategy == Strategy.FirstBarPullback)
                {
                    JsonResult jsonResul = await DeleteFirstBarPullback(id);
                    return jsonResul;
                }
                return Json(new { error = "Delete method not implemented for this strategy." });
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error in Delete(): {ex.Message})" });
            }
        }

        private async Task<JsonResult> DeleteFirstBarPullback(int id)
        {
            ResearchFirstBarPullback trade = await _unitOfWork.ResearchFirstBarPullback.GetAsync(x => x.Id == id);
            SampleSize sampleSize = await _unitOfWork.SampleSize.GetAsync(x => x.Id == trade.SampleSizeId);
            List<SampleSize> sampleSizes = null;
            Strategy currentStrategy = sampleSize.Strategy;
            TimeFrame currentTF = sampleSize.TimeFrame;
            if (trade == null)
            {
                return Json(new { error = "No trade was found for this id." });
            }
            _unitOfWork.ResearchFirstBarPullback.Remove(trade);
            await _unitOfWork.SaveAsync();
            // Get the rest of the trades in this sample size
            List<ResearchFirstBarPullback> listAllTrades = await _unitOfWork.ResearchFirstBarPullback.GetAllAsync(x => x.SampleSizeId == trade.SampleSizeId);
            string jsonTrades = string.Empty;
            // The sample size is empty now
            if (!listAllTrades.Any())
            {
                // Delete the empty sample size
                _unitOfWork.SampleSize.Remove(sampleSize);
                await _unitOfWork.SaveAsync();

                // Check if there are more sample sizes for the paramaters. If yes get the last
                sampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.Strategy == sampleSize.Strategy && x.TimeFrame == sampleSize.TimeFrame && x.TradeType == TradeType.Research);

                int lastSampleSizeId = 0;
                // No more sample sizes for these parameters. The trade that was deleted was the last for these paramaters
                if (!sampleSizes.Any())
                {
                    ResearchVM.AvailableTimeframes.Remove((TimeFrame)sampleSize.TimeFrame!);
                }
                // Get the last sample size id for these paramaters
                else
                {
                    lastSampleSizeId = sampleSizes.LastOrDefault()!.Id;
                }

                // Get all trades for the last sample size id for these paramaters
                if (lastSampleSizeId != 0)
                {
                    listAllTrades = await _unitOfWork.ResearchFirstBarPullback.GetAllAsync(x => x.SampleSizeId == lastSampleSizeId);
                }
                // Check if there are any other sample sizes (any TF, any Strategy)
                else
                {
                    sampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.TradeType == TradeType.Research);

                    if (sampleSizes.Any())
                    {
                        lastSampleSizeId = sampleSizes.LastOrDefault()!.Id;
                        listAllTrades = await _unitOfWork.ResearchFirstBarPullback.GetAllAsync(x => x.SampleSizeId == lastSampleSizeId);
                    }
                }
            }

            if (listAllTrades.Any())
            {
                if (sampleSizes == null)
                {
                    sampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.Id == listAllTrades.First().SampleSizeId);
                }
                foreach (ResearchFirstBarPullback researchFirstBarPullback in listAllTrades)
                {
                    ResearchVM.AllTrades.Add(EntityMapper.EntityToViewModel<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(researchFirstBarPullback));
                }
            }
            else
            {
                RedirectToAction(nameof(Index));
            }

            // Set the values for the view
            SampleSize currentSampleSize = sampleSizes.SingleOrDefault(x => x.Id == listAllTrades[0].SampleSizeId);
            ResearchVM.CurrentStrategy = currentSampleSize.Strategy;
            ResearchVM.CurrentTimeFrame = currentSampleSize.TimeFrame;
            ResearchVM.CurrentSampleSizeNumber = sampleSizes.Count;
            ResearchVM.TradesInSampleSize = listAllTrades.Count;
            ResearchVM.NumberSampleSizes = sampleSizes.Count;
            string researchVM = JsonConvert.SerializeObject(ResearchVM);
            // The method should be able to delete the sample size, and then get the trades from the last sample size for the given params.
            // Convert the trades and the new menu values in json and return that.
            return Json(new { researchVM });
        }

        [HttpPost]
        public async Task<IActionResult> LoadResearchSampleSize(string timeFrame, string strategy, string sampleSizeNumber, string isSampleSizeChanged)
        {
            string errorMsg = ResearchVM.SetSampleSizeParams(timeFrame, strategy, sampleSizeNumber, isSampleSizeChanged);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Json(new { error = errorMsg });
            }

            List<SampleSize> sampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.TradeType == TradeType.Research && x.TimeFrame == ResearchVM.CurrentTimeFrame && x.Strategy == ResearchVM.CurrentStrategy);

            errorMsg = await LoadViewModelData(sampleSizes, ResearchVM.CurrentSampleSizeId);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Json(new { error = errorMsg });
            }

            string researchVM = JsonConvert.SerializeObject(ResearchVM);

            return Json(new { researchVM });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTrade([FromBody] ResearchFirstBarPullbackDisplay currentTrade)
        {
            if (currentTrade == null)
            {
                return Json(new { error = "CurrentTrade is null." });
            }
            if (!ModelState.IsValid)
            {
                return Json(new { error = "Wrong values. Please note the messages" });
            }

            ResearchFirstBarPullback trade = EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(currentTrade);
            // The Id of a trade is in the currentTrade paramater. The id is passed to the trade object in ResearchMapper.ViewModelToEntity().
            // The Update() method, queries the database for a trade based on the Id.
            SanitizationHelper.SanitizeObject(trade);
            await _unitOfWork.ResearchFirstBarPullback.UpdateAsync(trade);
            await _unitOfWork.SaveAsync();

            return Json(new { success = "Trade was successfully updated" });
        }


        public async Task<IActionResult> Index()
        {
            // Get research sample sizes
            List<SampleSize> sampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.TradeType == TradeType.Research);

            // No researched trades
            if (sampleSizes.Count == 0)
            {
                return View(ResearchVM);
            }

            string errorMsg = await LoadViewModelData(sampleSizes, IndexMethod);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Json(new { error = errorMsg });
            }

            // Display only values for which there are data records.
            foreach (SampleSize sampleSize in sampleSizes)
            {
                if (!ResearchVM.AvailableTimeframes.Contains(sampleSize.TimeFrame))
                {
                    ResearchVM.AvailableTimeframes.Add(sampleSize.TimeFrame);
                }
                // Set the time frames in ascending order
                ResearchVM.AvailableTimeframes.Sort();

                if (!ResearchVM.AvailableStrategies.Contains(sampleSize.Strategy))
                {
                    ResearchVM.AvailableStrategies.Add(sampleSize.Strategy);
                }
                // Sort the strategies in ascending order
                ResearchVM.AvailableStrategies.Sort();
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
                        List<ZipArchiveEntry> sortedEntries = [..archive.Entries.OrderBy(e => e.FullName, new NaturalStringComparer())];
                        List<ResearchFirstBarPullback> researchTrades = new List<ResearchFirstBarPullback>();

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
                                researchedTF = MyEnumConverter.TimeFrameFromString(tempTF).Value;
                                // Set the sample size for the research
                                SampleSize sampleSize = new SampleSize();
                                sampleSize.TradeType = TradeType.Research;
                                sampleSize.Strategy = (Strategy)strategy;
                                sampleSize.TimeFrame = researchedTF;
                                _unitOfWork.SampleSize.Add(sampleSize);
                                await _unitOfWork.SaveAsync();

                                // Parse the .csv data
                                using (var csvStream = entry.Open())
                                {
                                    var csvData = await ReadCsvFileAsync(csvStream);
                                    if (csvData != null)
                                    {
                                        ResearchFirstBarPullback researchTrade = new ResearchFirstBarPullback();
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
                                                researchTrade.SampleSizeId = sampleSize.Id;
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
                                                researchTrades.Add(researchTrade);
                                                researchTrade = new ResearchFirstBarPullback();
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
                                    string tempTradeInfo = tradeInfo[3].Replace("Trade", "").Trim();
                                    if (Int32.TryParse(tempTradeInfo, out int tempTradeIndex))
                                    {
                                        tradeIndex = tempTradeIndex - 1;
                                    }
                                    currentFolder = ScreenshotsHelper.CreateScreenshotFolders(tradeInfo, currentFolder, entry.FullName, wwwRootPath, 3);

                                    if (researchTrades[tradeIndex].ScreenshotsUrls == null)
                                    {
                                        researchTrades[tradeIndex].ScreenshotsUrls = new List<string>();
                                    }

                                    entry.ExtractToFile(Path.Combine(currentFolder, entry.Name));
                                    string screenshotName = entry.FullName.Split('/').Last();
                                    string screenshotPath = currentFolder.Replace(wwwRootPath, "").Replace("\\", "/");
                                    researchTrades[tradeIndex].ScreenshotsUrls.Add(Path.Combine(screenshotPath, screenshotName));
                                }
                            }
                        }
                        _unitOfWork.ResearchFirstBarPullback.AddRange(researchTrades);
                        await _unitOfWork.SaveAsync();
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

        #endregion

        #region Private Methods

        private async Task<string> LoadViewModelData(List<SampleSize> sampleSizes, int sampleSizeNumber)
        {
            string errorMsg = string.Empty;
            int lastSampleSizeId = 0;
            // Method is called from the Index()
            if (sampleSizeNumber == IndexMethod)
            {
                lastSampleSizeId = sampleSizes.LastOrDefault().Id;
            }
            // Ajax API call
            else
            {
                // If different sample size is selected for the same time frame and strategy
                if (ResearchVM.HasSampleSizeChanged)
                {
                    lastSampleSizeId = sampleSizes[sampleSizeNumber - 1].Id;
                }
                // Different time frame and/or strategy is selected
                else
                {
                    sampleSizeNumber = sampleSizes.Count;
                    lastSampleSizeId = sampleSizes.Last().Id;
                }
            }
            // Get all researched trades from the DB and project the instances into ResearchFirstBarPullbackDisplay
            ResearchVM.AllTrades = (await _unitOfWork.ResearchFirstBarPullback
                                    .GetAllAsync(x => x.SampleSizeId == lastSampleSizeId))
                                    .Select(x => EntityMapper.EntityToViewModel<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(x))
                                    .ToList();
            ResearchVM.AllTrades.ForEach(x => SanitizationHelper.SanitizeObject(x));

            // Should not happen
            if (!ResearchVM.AllTrades.Any())
            {
                errorMsg = "No trades available for this sample size.";
            }
            // Set the values for the button menus
            ResearchVM.ResearchFirstBarPullbackDisplay = ResearchVM.AllTrades.FirstOrDefault()!;
            ResearchVM.CurrentSampleSize = sampleSizes.FirstOrDefault(x => x.Id == lastSampleSizeId)!;
            ResearchVM.CurrentTimeFrame = ResearchVM.CurrentSampleSize.TimeFrame;
            ResearchVM.CurrentSampleSizeNumber = sampleSizeNumber;
            ResearchVM.CurrentSampleSizeId = lastSampleSizeId;
            // Set the NumberSampleSizes for the button menu
            ResearchVM.NumberSampleSizes = sampleSizes.Count(x => x.TimeFrame == ResearchVM.CurrentTimeFrame);
            ResearchVM.TradesInSampleSize = ResearchVM.AllTrades.Count;

            return errorMsg;
        }

        private void LoadTradeData()
        {

        }

        #endregion

        #endregion
    }
}
