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
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private ApplicationDbContext _db;

        public ReviewRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(Review review)
        {
            Review? objFromDb = _db.Reviews.FirstOrDefault(x => x.Id == review.Id);
            if (objFromDb != null)
            {
                objFromDb.SampleSizeReview = review.SampleSizeReview;
                objFromDb.Strategy = review.Strategy;
                objFromDb.TimeFrame = review.TimeFrame;
                objFromDb.TradeType = review.TradeType;
            }
        }
    }
}
