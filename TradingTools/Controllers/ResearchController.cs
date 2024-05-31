using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Models.ViewModels;

namespace TradingTools.Controllers
{
    public class ResearchController : Controller
    {
        #region Constructor
        public ResearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ResearchVM = new ResearchVM();
        }

        #endregion

        #region Private Properties

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Public Properties

        public ResearchVM ResearchVM { get; set; }

        #endregion

        #region Methods

        public IActionResult Index()
        {
            return View(ResearchVM);
        }

        [HttpPost]
        public IActionResult UploadResearch(IFormFile zipFile)
        {


            return RedirectToAction(nameof(Index));
        }

        #endregion

    }
}
