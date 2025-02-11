using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using SharedEnums.Enums;
using System.Diagnostics;
using Utilities;

namespace TradingTools.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork unitOfWork, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Fields

        private readonly IUnitOfWork _unitOfWork;

        private readonly ApplicationDbContext _db;

        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion

        public async Task<IActionResult> CreateBackup()
        {
            string screenshotsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Screenshots");
            string zipFile = await DatabaseBackupHelper.CreateBackupZipFile(_db, screenshotsFolder);
            FileStream zipStream = null;
            try
            {
                // zipStream is disposed in File()
                zipStream = new FileStream(zipFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                DeleteBackupFileAfterResponseIsCompleted();
               
                return File(zipStream, "application/zip", Path.GetFileName(zipFile));
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error in {GetType().Name}.{nameof(CreateBackup)}: {ex.Message}\r\n{ex.StackTrace}" });
            }

            void DeleteBackupFileAfterResponseIsCompleted()
            {
                HttpContext.Response.OnCompleted(() =>
                {
                    System.IO.File.Delete(zipFile);
                    return Task.CompletedTask;
                });
            }
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
                    PTTimeFrame = ETimeFrame.M10,
                    PTStrategy = EStrategy.FirstBarPullback
                });

                await _unitOfWork.SaveAsync();
            }
        }
    }
}
