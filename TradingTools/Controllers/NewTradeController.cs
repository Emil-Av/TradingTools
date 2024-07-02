using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Newtonsoft.Json;
using NuGet.Protocol;
using Utilities;

namespace TradingTools.Controllers
{
    public class NewTradeController : Controller
    {
        public NewTradeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            NewTradeVM = new NewTradeVM();
        }

        #region Fields

        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion

        #region Properties

        NewTradeVM NewTradeVM { get; set; }

        #endregion


        #region Methods
        [HttpPost]
        public IActionResult UploadScreenshots([FromForm] IFormFile[] files, [FromBody] string tradeData)
        {
            Dictionary<string, string> tradeDataObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(tradeData);
            if (tradeData == null)
            {
                return Json(new { error = "Trade data was empty." });
            }

            NewTradeVM.TimeFrame = MyEnumConverter.TimeFrameFromString(tradeDataObject["timeFrame"]);
            NewTradeVM.Strategy = MyEnumConverter.StrategyFromString(tradeDataObject["strategy"]);
            NewTradeVM.TradeType = MyEnumConverter.TradeTypeFromString(tradeDataObject["tradeType"]);
            NewTradeVM.Side = MyEnumConverter.SideTypeFromString(tradeDataObject["tradeSide"]);

            
            return View(NewTradeVM);
        }

        public IActionResult Index()
        {
            return View(NewTradeVM);
        }
    }

    #endregion
}
