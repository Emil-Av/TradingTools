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
            PaperTrade? objFromDb = await _db.PaperTrades.FindAsync(paperTrade.Id);
            if (objFromDb != null)
            {
                objFromDb.Symbol = paperTrade.Symbol;
                objFromDb.TriggerPrice = paperTrade.TriggerPrice;
                objFromDb.EntryPrice = paperTrade.EntryPrice;
                objFromDb.StopPrice = paperTrade.StopPrice;
                objFromDb.Targets = paperTrade.Targets;
                objFromDb.ExitPrice = paperTrade.ExitPrice; 
                objFromDb.PnL = paperTrade.PnL; 
                objFromDb.IsLoss = paperTrade.IsLoss;
                objFromDb.Fee = paperTrade.Fee;
                objFromDb.Status = paperTrade.Status;
                objFromDb.SideType = paperTrade.SideType;
                objFromDb.OrderType = paperTrade.OrderType;
                objFromDb.ScreenshotsUrls = paperTrade.ScreenshotsUrls;
                objFromDb.TradeDurationInCandles = paperTrade.TradeDurationInCandles;
            }
        }
    }
}
