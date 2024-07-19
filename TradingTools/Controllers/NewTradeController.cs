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

        public NewTradeVM NewTradeVM { get; set; }

        #endregion


        #region Methods

        public IActionResult Index()
        {
            return View(NewTradeVM);
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewTrade([FromForm] IFormFile[] files, [FromForm] string tradeParams, [FromForm] string tradeData)
        {
            // Set the values of NewTradeVM properties
            string errorMsg = NewTradeVM.SetValues(tradeParams, tradeData);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Json(new { error = errorMsg });

            }

            await SaveTrade(files);
            return Json(new { error = errorMsg });
        }

        private async Task SaveTrade(IFormFile[] files)
        {
            if (NewTradeVM.TradeType == TradeType.Research)
            {
                // Convert the values
                ResearchFirstBarPullback newTrade = EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(NewTradeVM.NewTrade);
                newTrade.SampleSizeId = await GetSampleSizeId();
                newTrade.ScreenshotsUrls = await AppHelper.SaveFiles(_webHostEnvironment.WebRootPath, NewTradeVM, newTrade, files);
                await _unitOfWork.ResearchFirstBarPullback.AddAsync(newTrade);
                await _unitOfWork.SaveAsync();
            }
        }

        private async Task<int> GetSampleSizeId()
        {
            int id = 0;
            int numberTradesInSampleSize = 0;
            // New trade is Research
            if (NewTradeVM.TradeType == TradeType.Research)
            {
                if (NewTradeVM.Strategy == Strategy.FirstBarBelowAbove)
                {
                    // Get the last sample size for the given parameters
                    SampleSize? lastSampleSize = (await _unitOfWork.SampleSize
                                                .GetAllAsync(x => x.Strategy == NewTradeVM.Strategy && 
                                                            x.TimeFrame == NewTradeVM.TimeFrame && 
                                                            x.TradeType == NewTradeVM.TradeType))
                                                .LastOrDefault();

                    if (lastSampleSize != null)
                    {
                        numberTradesInSampleSize = (await _unitOfWork.ResearchFirstBarPullback.GetAllAsync(x => x.SampleSizeId == lastSampleSize.Id)).Count;
                    }
                    // If there is no sample size or the sample size is full, create a new one
                    if (lastSampleSize == null || numberTradesInSampleSize == 100)
                    {
                        SampleSize sampleSize = new SampleSize { Strategy = NewTradeVM.Strategy, TimeFrame = NewTradeVM.TimeFrame, TradeType = NewTradeVM.TradeType};
                        await _unitOfWork.SampleSize.AddAsync(sampleSize);
                        await _unitOfWork.SaveAsync();
                        lastSampleSize = sampleSize;
                    }

                    return lastSampleSize.Id;
                }
            }

            return id;
        }
    }

    #endregion
}
