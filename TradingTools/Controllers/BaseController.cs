using Microsoft.AspNetCore.Mvc;

namespace TradingTools.Controllers
{
    public class BaseController : Controller
    {
        protected JsonResult ValidateModelState()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                string allErrors = string.Join(", ", errors);
                return Json(new { error = allErrors });
            }

            return null;
        }
    }
}
