using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        // When the class gets injected, the connection string is passed to the DbContext as a paramater in the constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<PaperTrade> PaperTrades { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<SampleSize> SampleSizes { get; set; }

        public DbSet<UserSettings> UserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaperTrade>().HasData(
                new PaperTrade { Id=1, Symbol="BTCUSD", TimeFrame = TimeFrame.M10, Strategy = Strategy.FirstBarBelowAbove, ScreenshotsUrls = new List<string>() { "~/img/myimg/1.png", "~/img/myimg/2.png", "~/img/myimg/3.png" } });
        }
    }
}
