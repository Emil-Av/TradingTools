using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.Identity.Client;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    internal class ResearchRepository : Repository<Research>, IResearchRepository
    {
        private ApplicationDbContext _db;

        public ResearchRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Research researchTrade)
        {
            Research? objFromDb = _db.Research.FirstOrDefault(x => x.Id == researchTrade.Id);
            if (objFromDb != null)
            {
                objFromDb.ScreenshotsUrls = researchTrade.ScreenshotsUrls;
            }
        }
    }
}
