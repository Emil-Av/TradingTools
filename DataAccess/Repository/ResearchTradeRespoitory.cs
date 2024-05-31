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
    internal class ResearchTradeRespoitory : Repository<ResearchTrade>, IResearchTradeRepository
    {
        private ApplicationDbContext _db;

        public ResearchTradeRespoitory(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ResearchTrade researchTrade)
        {

        }
    }
}
