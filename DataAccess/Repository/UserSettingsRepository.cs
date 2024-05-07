using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserSettingsRepository : Repository<UserSettings>, IUserSettingsRepository
    {
        private ApplicationDbContext _db;

        public UserSettingsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(UserSettings userSettings)
        {
            UserSettings? objFromDb = _db.UserSettings.FirstOrDefault(x => x.Id == userSettings.Id);
            if (objFromDb != null)
            {
                objFromDb.PTStrategy = userSettings.PTStrategy;
                objFromDb.PTTimeFrame = userSettings.PTTimeFrame;
            }
        }
    }
}
