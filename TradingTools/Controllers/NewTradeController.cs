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
    public class NewTradeController : BaseController
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
            JsonResult validationResult = ValidateModelState();
            if (validationResult != null)
            {
                return validationResult;
            }

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
                    await SaveResearchDataFirstbarPullback(maxTradesProSampleSize: 100);
                }
                else if (NewTradeVM.Strategy == Strategy.Cradle)
                {

                }
            }
            // Trades or Paper Trades
            else
            {
                if (NewTradeVM.Strategy == Strategy.FirstBarPullback)
                {
                    ResearchFirstBarPullback researchData = await SaveResearchDataFirstbarPullback(maxTradesProSampleSize: 20);
                    PaperTrade newTrade = await SetNewTradeData(researchData);

                    await CreateJournal(newTrade);

                    _unitOfWork.PaperTrade.Add(newTrade);
                    await _unitOfWork.SaveAsync();
                }
                else if (NewTradeVM.Strategy == Strategy.Cradle)
                {

                }
            }

            #region Helper Methods

            async Task CreateJournal(PaperTrade newTrade)
            {
                Journal journal = new();
                _unitOfWork.Journal.Add(journal);
                await _unitOfWork.SaveAsync();
                newTrade.JournalId = journal.Id;
            }

            async Task<PaperTrade> SetNewTradeData(ResearchFirstBarPullback researchData)
            {
                PaperTrade newTrade = EntityMapper.ViewModelToEntity<PaperTrade, TradeDisplay>(NewTradeVM.TradeData, existingEntity: null);
                await ScreenshotsHelper.SaveFilesAsync(_webHostEnvironment.WebRootPath, NewTradeVM, newTrade, files);
                newTrade.ResearchId = researchData.Id;
                newTrade.SampleSizeId = researchData.SampleSizeId;
                newTrade.Status = NewTradeVM.Status;
                newTrade.SideType = NewTradeVM.SideType;
                newTrade.OrderType = NewTradeVM.OrderType;
                newTrade.TradeType = NewTradeVM.TradeType;

                return newTrade;
            }

            async Task<ResearchFirstBarPullback> SaveResearchDataFirstbarPullback(int maxTradesProSampleSize)
            {
                ResearchFirstBarPullbackDisplay viewData = NewTradeVM.ResearchData as ResearchFirstBarPullbackDisplay;
                // Convert the ViewData into DB entity
                ResearchFirstBarPullback researchData = EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(viewData, existingEntity: null);
                researchData.SampleSizeId = (await ProcessSampleSize(maxTradesProSampleSize: maxTradesProSampleSize)).id;
                // Called for a research trade
                if (maxTradesProSampleSize == 100)
                {
                    await ScreenshotsHelper.SaveFilesAsync(_webHostEnvironment.WebRootPath, NewTradeVM, researchData, files);
                }
                _unitOfWork.ResearchFirstBarPullback.Add(researchData);
                await _unitOfWork.SaveAsync();

                return researchData;
            }

            #endregion
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

                Review review = null;
                // Create a new review for the new sample size
                if (NewTradeVM.TradeType != TradeType.Research)
                {
                    review = new();
                    _unitOfWork.Review.Add(review);
                    try
                    {
                        await _unitOfWork.SaveAsync();
                    }
                    catch (Exception ex)
                    {

                    }

                    //await _unitOfWork.SampleSize.UpdateAsync(newSampleSize);
                }
                SampleSize newSampleSize = new() { Strategy = NewTradeVM.Strategy, TimeFrame = NewTradeVM.TimeFrame, TradeType = NewTradeVM.TradeType };
                if (review != null)
                {
                    newSampleSize.ReviewId = review.Id;
                }
                _unitOfWork.SampleSize.Add(newSampleSize);
                try
                {
                    await _unitOfWork.SaveAsync();
                }
                catch (Exception ex)
                {

                }
                sampleSizeData.id = newSampleSize.Id;
            }

            return sampleSizeData;
        }

        #endregion
    }
}