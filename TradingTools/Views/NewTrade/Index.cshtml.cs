using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TradingTools.Pages
{
    public class NewTradeModel : PageModel
    {
        private readonly ILogger<NewTradeModel> _logger;

        public NewTradeModel(ILogger<NewTradeModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
