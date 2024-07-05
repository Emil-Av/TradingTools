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
        public IActionResult SaveNewTrade([FromForm] IFormFile[] files, [FromForm] string tradeParams, [FromForm] string tradeData)
        {
            if (tradeParams == null || tradeData == null)
            {
                return Json(new { error = "Trade values are empty." });
            }

            string errorMsg = NewTradeVM.SetValues(tradeParams, tradeData);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Json(new { error = errorMsg });

            }

            SaveTrade();
            
            return View(NewTradeVM);
        }

        public IActionResult Index()
        {
            return View(NewTradeVM);
        }

        private void SaveTrade() 
        {
            object newTrade;
            SampleSize lastSampleSize;
            if (NewTradeVM.TradeType == TradeType.Research)
            {
                newTrade = EntityMapper.ViewModelToEntity<ResearchFirstBarPullback, ResearchFirstBarPullbackDisplay>(NewTradeVM.NewTrade);
                // Change the structure of the Research table. Add enums for the different strategies that I have researched. That means a new model
                // for the Research table. ResearchFirstBarPullback will be a property of the new model. Also the new enum will be a new property of ResearchFirstBarPullback.
                // Then update the DB to set the enum of ResearchFirstBarPullback. Also the parsing script of the .csv has to be adjusted.
                // When that is done, in this method, get the last sample size for the ResearchFirstBarPullback enum. Check if it has less than 100 trades, if yes, save the new trade under this sample size number, otherwise create new sample size.
            }
        }

    }

    #endregion
}
