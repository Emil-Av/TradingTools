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

        ISampleSizeRepository SampleSize { get; }

        IUserSettingsRepository UserSettings { get; }

        void Save();
    }
}
