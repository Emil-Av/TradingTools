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
        public async Task<IActionResult> SaveNewTrade([FromForm] IFormFile[] files, [FromForm] string tradeParams, [FromForm] string tradeData)
        {
            if (tradeParams == null || tradeData == null)
            {
                return Json(new { error = "Trade values are empty." });
            }
            // Set the values of NewTradeVM properties
            string errorMsg = NewTradeVM.SetValues(tradeParams, tradeData);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Json(new { error = errorMsg });

            }

            await SaveTrade(files);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View(NewTradeVM);
        }

        private async Task SaveTrade(IFormFile[] files)
        {
            int lastSampleSizeId;
            if (NewTradeVM.TradeType == TradeType.Research)
            {
                if (NewTradeVM.ResearchData is ResearchFirstBarPullbackDisplay researchData)
                {
                    ResearchFirstBarPullback newTrade = EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(researchData);
                    // Check if there is a sample size for the parameters and if it's full.
                    var sampleSizeData = await GetLastSampleSizeData();
                    lastSampleSizeId = sampleSizeData.id;
                    bool isFull = sampleSizeData.isFull;
                    // If the sample size is full or there is no sample size for those paramaters (lastSampleSize == 0), create a new sample size
                    if (isFull || lastSampleSizeId == 0)
                    {
                        SampleSize newSampleSize = 
                            new SampleSize { Strategy = NewTradeVM.Strategy , TimeFrame = NewTradeVM.TimeFrame, TradeType = NewTradeVM.TradeType};
                        _unitOfWork.SampleSize.Add(newSampleSize);
                        await _unitOfWork.SaveAsync();
                        lastSampleSizeId = newSampleSize.Id;
                    }
                    newTrade.SampleSizeId = lastSampleSizeId;
                    // TODO: Adjust the FilePaths to be able to be displayed
                    // Create either properties or a new class for the entry prices.
                    // The files are not saved in the right folder
                    await AppHelper.SaveFilesAsync(_webHostEnvironment.WebRootPath, NewTradeVM, newTrade, files);
                    _unitOfWork.ResearchFirstBarPullback.Add(newTrade);
                    await _unitOfWork.SaveAsync();
                }
            }
        }

        private async Task<(int id, bool isFull)> GetLastSampleSizeData()
        {
            int id = 0;
            bool isFull = false;
            // New trade is Research
            if (NewTradeVM.TradeType == TradeType.Research)
            {
                if (NewTradeVM.Strategy == Strategy.FirstBarBelowAbove)
                {
                    id = (await _unitOfWork.SampleSize.
                        GetAllAsync(x => x.TimeFrame == NewTradeVM.TimeFrame && x.Strategy == NewTradeVM.Strategy && x.TradeType == NewTradeVM.TradeType)).LastOrDefault().Id;
                    int numberTradesInSampleSize = (await _unitOfWork.ResearchFirstBarPullback.GetAllAsync(x => x.SampleSizeId == id)).Count;
                    if (numberTradesInSampleSize == 100)
                    {
                        isFull = true;
                    }

                    return (id, isFull);
                }
            }

            return (id, isFull);
        }
    }

    #endregion
}
