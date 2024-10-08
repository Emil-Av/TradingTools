using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using SharedEnums.Enums;

namespace TradingTools.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            await CheckOrCreateUserSettings();

            return View();
        }

        /// <summary>
        ///  Creates new user settings.
        /// </summary>
        private async Task CheckOrCreateUserSettings()
        {
            int usetSettingsCount = (await _unitOfWork.UserSettings.GetAllAsync()).Count;
            if (usetSettingsCount == 0)
            {
                _unitOfWork.UserSettings.Add(new UserSettings()
                {
                    PTTimeFrame = TimeFrame.M10,
                    PTStrategy = Strategy.FirstBarPullback
                });

                _unitOfWork.SaveAsync();
            }
        }
    }
}
