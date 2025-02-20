using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModels;

namespace DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        // When the class gets injected, the connection string is passed to the DbContext as a paramater in the constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<BaseTrade> BaseTrades { get; set; }

        public DbSet<Trade> Trades { get; set; }

        public DbSet<ResearchFirstBarPullback> ResearchFirstBarPullbacks { get; set; }
        public DbSet<Journal> Journals { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<SampleSize> SampleSizes { get; set; }

        public DbSet<UserSettings> UserSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the relationship between Trade and BaseTrade
            modelBuilder.Entity<Trade>()
                .HasOne<BaseTrade>()
                .WithOne()
                .HasForeignKey<Trade>(t => t.Id)
                .OnDelete(DeleteBehavior.NoAction);  // Avoid cascading delete

            // Configuring the relationship between Research and BaseTrade
            modelBuilder.Entity<ResearchFirstBarPullback>()
                .HasOne<BaseTrade>()
                .WithOne()
                .HasForeignKey<ResearchFirstBarPullback>(r => r.Id)
                .OnDelete(DeleteBehavior.NoAction);  // Avoid cascading delete
        }
    }
}
