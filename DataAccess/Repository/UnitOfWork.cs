using DataAccess.Data;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public IPaperTradeRepository PaperTrade { get; private set; }

        public IReviewRepository Review { get; private set; }

        public IJournalRepository Journal { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            PaperTrade = new PaperTradeRepository(_db);
            Review = new ReviewRepository(_db);
            Journal = new JournalRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
