using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace TradingTools.Controllers
{
    public class NewTradeController : Controller
    {
        public NewTradeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        #region Fields

        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion


        #region Methods
        [HttpPost]
        public IActionResult UploadScreenshots([FromForm] IFormFile[] files, [FromForm] string tradeData)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    
                }
            }
            catch { }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }

    #endregion
}
