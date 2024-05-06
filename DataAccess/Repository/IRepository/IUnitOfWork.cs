using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IPaperTradeRepository PaperTrade { get; }

        IJournalRepository Journal { get; }

        IReviewRepository Review { get; }

        void Save();
    }
}
