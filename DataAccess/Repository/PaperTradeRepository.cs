using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class PaperTradeRepository : Repository<PaperTrade>, IPaperTradeRepository
    {
        private ApplicationDbContext _db;

        public PaperTradeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(PaperTrade paperTrade)
        {
            PaperTrade? objFromDb = await _db.PaperTrades.FirstOrDefaultAsync(x => x.Id == paperTrade.Id);
            if (objFromDb != null)
            {
                objFromDb.Symbol = paperTrade.Symbol;
                objFromDb.TriggerPrice = paperTrade.TriggerPrice;
                objFromDb.EntryPrice = paperTrade.EntryPrice;
                objFromDb.StopPrice = paperTrade.StopPrice;
                objFromDb.Targets = paperTrade.Targets;
                objFromDb.ExitPrice = paperTrade.ExitPrice; 
                objFromDb.Profit = paperTrade.Profit; 
                objFromDb.Loss = paperTrade.Loss;
                objFromDb.Fee = paperTrade.Fee;
                objFromDb.TimeFrame = paperTrade.TimeFrame;
                objFromDb.Status = paperTrade.Status;
                objFromDb.Strategy = paperTrade.Strategy;
                objFromDb.SideType = paperTrade.SideType;
                objFromDb.OrderType = paperTrade.OrderType;
                objFromDb.ScreenshotsUrls = paperTrade.ScreenshotsUrls;
                objFromDb.EntryTime = paperTrade.EntryTime;
                objFromDb.ExitTime = paperTrade.ExitTime;
                objFromDb.TradeDuration = paperTrade.TradeDuration;
            }
        }
    }
}
