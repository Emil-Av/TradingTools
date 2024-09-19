using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Models.ViewModels.DisplayClasses;
using Newtonsoft.Json;
using NuGet.Protocol;
using Shared;
using SharedEnums.Enums;
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
                    ResearchFirstBarPullback researchTrade = EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(viewData);
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
                #region ToDelete
                //if (NewTradeVM.Strategy == Strategy.FirstBarPullback)
                //{

                //    if (NewTradeVM.ResearchData is ResearchFirstBarPullbackDisplay researchData)
                //    {
                //        ResearchFirstBarPullback newTrade =
                //            EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(researchData);
                //        // Check if there is a sample size for the parameters and if it's full.
                //        var sampleSize = await GetLastSampleSizeData(maxTradesProSampleSize: 100);
                //        // If the sample size is full or there is no sample size for those paramaters (id == 0), create a new sample size
                //        if (sampleSize.isFull || sampleSize.id == 0)
                //        {
                //            SampleSize newSampleSize =
                //                new SampleSize { Strategy = NewTradeVM.Strategy, TimeFrame = NewTradeVM.TimeFrame, TradeType = NewTradeVM.Type };
                //            _unitOfWork.SampleSize.Add(newSampleSize);
                //            await _unitOfWork.SaveAsync();
                //            sampleSize.id = newSampleSize.Id;
                //        }
                //        newTrade.SampleSizeId = sampleSize.id;
                //        await ScreenshotsHelper.SaveFilesAsync(_webHostEnvironment.WebRootPath, NewTradeVM, newTrade, files);
                //        _unitOfWork.ResearchFirstBarPullback.Add(newTrade);
                //        await _unitOfWork.SaveAsync();
                //    }
                //}
                #endregion
            }
            // Trades or PaperTrades
            else
            {
                if (NewTradeVM.Strategy == Strategy.FirstBarPullback)
                {
                    ResearchFirstBarPullbackDisplay viewData = NewTradeVM.ResearchData as ResearchFirstBarPullbackDisplay;
                    ResearchFirstBarPullback researchTrade = EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(viewData);
                    // Save the values from the view and return a DB entity. The entity contains the SampleSizeId.
                    var sampleSizeData = await ProcessSampleSize(maxTradesProSampleSize: 20);
                    // Save the values from the view and return a DB entity.
                    PaperTrade newTrade = EntityMapper.ViewModelToEntity<PaperTrade, TradeDisplay>(NewTradeVM.TradeDisplay);
                    await ScreenshotsHelper.SaveFilesAsync(_webHostEnvironment.WebRootPath, NewTradeVM, newTrade, files);
                    newTrade.ResearchId = researchTrade.Id;
                    newTrade.SampleSizeId = researchTrade.SampleSizeId;
                    // TODO: the new trade needs a review and journal id. Class Trade should have foreign keys to Journal and Review, not the other way around. Check the GetLastSampleSizeData(). There might be no need for if else.. Think about it
                    _unitOfWork.PaperTrade.Add(newTrade);
                    await _unitOfWork.SaveAsync();
                }
                else if (NewTradeVM.Strategy == Strategy.Cradle)
                {

                }
            }
            

            #region Local Helper Methods

            async Task<(int id, bool isFull)> ProcessSampleSize(int maxTradesProSampleSize)
            {
                var sampleSizeData = await GetLastSampleSizeData(maxTradesProSampleSize);
                if (sampleSizeData.isFull || sampleSizeData.id == 0)
                {
                    SampleSize newSampleSize = new() { Strategy = NewTradeVM.Strategy, TimeFrame = NewTradeVM.TimeFrame, TradeType = NewTradeVM.TradeType };
                    _unitOfWork.SampleSize.Add(newSampleSize);
                    await _unitOfWork.SaveAsync();
                    sampleSizeData.id = newSampleSize.Id;
                }

                return sampleSizeData;
            }

            async Task<(int id, bool isFull)> GetLastSampleSizeData(int maxTradesProSampleSize)
            {
                // TradeDisplay tradeDisplay = new(); // What is the purpose here?
                int id = 0;
                bool isFull = false;
                // New trade is Research
                List<SampleSize> listSampleSizes = await _unitOfWork.SampleSize.GetAllAsync(x => x.TimeFrame == NewTradeVM.TimeFrame && x.Strategy == NewTradeVM.Strategy && x.TradeType == NewTradeVM.TradeType);
                if (!listSampleSizes.Any())
                {
                    return (0, false);
                }

                id = listSampleSizes.Last().Id;
                int numberTradesInSampleSize = 0;

                if (NewTradeVM.TradeType == TradeType.Research)
                {
                    if (NewTradeVM.Strategy == Strategy.FirstBarPullback)
                    {
                        List<ResearchFirstBarPullback> researchedTrades = await _unitOfWork.ResearchFirstBarPullback.GetAllAsync(x => x.SampleSizeId == id);
                        numberTradesInSampleSize = researchedTrades.Count;
                    }
                }
                else if (NewTradeVM.Strategy != Strategy.FirstBarPullback)
                {

                }

                if (numberTradesInSampleSize == maxTradesProSampleSize)
                {
                    isFull = true;
                }

                //if (NewTradeVM.Type == TradeType.Research)
                //{
                //    if (NewTradeVM.Strategy == Strategy.FirstBarPullback)
                //    {
                //        List<SampleSize> listSampleSizes = (await _unitOfWork.SampleSize.
                //            GetAllAsync(x => x.TimeFrame == NewTradeVM.TimeFrame && x.Strategy == NewTradeVM.Strategy && x.TradeType == NewTradeVM.Type));
                //        if (listSampleSizes.Any())
                //        {
                //            id = listSampleSizes.Last().Id;

                //            int numberTradesInSampleSize = (await _unitOfWork.ResearchFirstBarPullback.GetAllAsync(x => x.SampleSizeId == id)).Count;

                //            if (numberTradesInSampleSize == maxTradesProSampleSize)
                //            {
                //                isFull = true;
                //            }

                //            return (id, isFull);
                //        }
                //    }
                //}

                return (id, isFull);
            }

            #endregion
        }
    }

    #endregion
}
