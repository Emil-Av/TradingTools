using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Models.ViewModels.DisplayClasses;
using Newtonsoft.Json;
using NuGet.Protocol;
using Shared;
using SharedEnums.Enums;
using System.Diagnostics;
using Utilities;

namespace TradingTools.Controllers
{
    public class NewTradeController : Controller
    {
        public NewTradeController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            NewTradeVM = new NewTradeVM();
        }

        #region Fields

        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion

        #region Properties

        NewTradeVM NewTradeVM { get; set; }

        #endregion


        #region Methods
        [HttpPost]
        public async Task<IActionResult> SaveNewTrade([FromForm] IFormFile[] files, [FromForm] string tradeParams, [FromForm] string researchData, string tradeData)
        {
            if (tradeParams == null || researchData == null || tradeData == null)
            {
                return Json(new { error = "Trade values are empty." });
            }
            // Set the values of NewTradeVM properties
            string errorMsg = NewTradeVM.SetValues(tradeParams, researchData, tradeData);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Json(new { error = errorMsg });
            }

            try
            {
                await SaveTrade(files);
                return Json(new { success = "Trade saved." });
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error while saving the trade: {ex.Message} " });
            }
        }

        public IActionResult Index()
        {
            return View(NewTradeVM);
        }

        private async Task SaveTrade(IFormFile[] files)
        {
            // Research
            if (NewTradeVM.TradeType == TradeType.Research)
            {
                if (NewTradeVM.Strategy == Strategy.FirstBarPullback)
                {
                    ResearchFirstBarPullbackDisplay viewData = NewTradeVM.ResearchData as ResearchFirstBarPullbackDisplay;
                    // Save the values from the view and return a DB entity. The entity contains the SampleSizeId.
                    ResearchFirstBarPullback researchTrade = EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(viewData, existingEntity: null);
                    var sampleSizeData = await ProcessSampleSize(maxTradesProSampleSize: 100);
                    researchTrade.SampleSizeId = sampleSizeData.id;
                    await ScreenshotsHelper.SaveFilesAsync(_webHostEnvironment.WebRootPath, NewTradeVM, researchTrade, files);

                    _unitOfWork.ResearchFirstBarPullback.Add(researchTrade);
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    // Research other strategies
                }
            }
            // Trades or PaperTrades
            else
            {
                if (NewTradeVM.Strategy == Strategy.FirstBarPullback)
                {
                    ResearchFirstBarPullbackDisplay viewData = NewTradeVM.ResearchData as ResearchFirstBarPullbackDisplay;
                    // Save the values from the view and return a DB entity.
                    ResearchFirstBarPullback researchData = EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(viewData, existingEntity: null);
                    var sampleSizeData = await ProcessSampleSize(maxTradesProSampleSize: 20);
                    researchData.SampleSizeId = sampleSizeData.id;
                    _unitOfWork.ResearchFirstBarPullback.Add(researchData);
                    await _unitOfWork.SaveAsync();

                    // Save the values from the view and return a DB entity.
                    PaperTrade newTrade = EntityMapper.ViewModelToEntity<PaperTrade, TradeDisplay>(NewTradeVM.TradeDisplay, existingEntity: null);
                    await ScreenshotsHelper.SaveFilesAsync(_webHostEnvironment.WebRootPath, NewTradeVM, newTrade, files);
                    newTrade.ResearchId = researchData.Id;
                    newTrade.SampleSizeId = sampleSizeData.id;

                    // Set the new Journal reference
                    Journal journal = new();
                    _unitOfWork.Journal.Add(journal);
                    await _unitOfWork.SaveAsync();
                    newTrade.JournalId = journal.Id;

                    // Save the new trade
                    _unitOfWork.PaperTrade.Add(newTrade);
                    await _unitOfWork.SaveAsync();
                }
                else if (NewTradeVM.Strategy == Strategy.Cradle)
                {

                }
            }
        }

        /// <summary>
        ///  Checks if the last sample size for the given trade paramaters is full. Retuns true/false and the sample size id.
        /// </summary>
        /// <param name="maxTradesProSampleSize"></param>
        /// <returns></returns>
        private async Task<(int id, bool isFull)> CheckLastSampleSize(int maxTradesProSampleSize)
        {
            int id = 0;
            bool isFull = false;
            List<SampleSize> listSampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.TimeFrame == NewTradeVM.TimeFrame && x.Strategy == NewTradeVM.Strategy && x.TradeType == NewTradeVM.TradeType);
            if (!listSampleSizes.Any())
            {
                return (0, false);
            }

            id = listSampleSizes.Last().Id;
            int numberTradesInSampleSize = 0;

            // Research
            if (NewTradeVM.TradeType == TradeType.Research)
            {
                if (NewTradeVM.Strategy == Strategy.FirstBarPullback)
                {
                    List<ResearchFirstBarPullback> researchedTrades = await _unitOfWork.ResearchFirstBarPullback.GetAllAsync(x => x.SampleSizeId == id);
                    numberTradesInSampleSize = researchedTrades.Count;
                }
            }
            // Trade or PaperTrade
            else if (NewTradeVM.TradeType == TradeType.PaperTrade)
            {
                List<PaperTrade> trades = await _unitOfWork.PaperTrade.GetAllAsync(x => x.SampleSizeId == id);
                numberTradesInSampleSize = trades.Count;
            }

            if (numberTradesInSampleSize == maxTradesProSampleSize)
            {
                isFull = true;
            }

            return (id, isFull);
        }

        /// <summary>
        ///  Creates a new sample size if the last one was full. Creates a new review if new sample size is created.
        /// </summary>
        /// <param name="maxTradesProSampleSize"></param>
        /// <returns></returns>
        private async Task<(int id, bool wasFull)> ProcessSampleSize(int maxTradesProSampleSize)
        {
            var sampleSizeData = await CheckLastSampleSize(maxTradesProSampleSize);
            if (sampleSizeData.isFull || sampleSizeData.id == 0)
            {
                SampleSize newSampleSize = new() { Strategy = NewTradeVM.Strategy, TimeFrame = NewTradeVM.TimeFrame, TradeType = NewTradeVM.TradeType };
                _unitOfWork.SampleSize.Add(newSampleSize);
                await _unitOfWork.SaveAsync();
                sampleSizeData.id = newSampleSize.Id;

                // Create a new review for the new sample size
                if (NewTradeVM.TradeType != TradeType.Research)
                {
                    Review review = new();
                    _unitOfWork.Review.Add(review);
                    await _unitOfWork.SaveAsync();
                    newSampleSize.ReviewId = review.Id;
                    await _unitOfWork.SampleSize.UpdateAsync(newSampleSize);
                }
            }

            return sampleSizeData;
        }

        #endregion
    }
}