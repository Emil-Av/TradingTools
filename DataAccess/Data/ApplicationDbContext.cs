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

        public DbSet<Trade> Trades { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<SampleSize> SampleSizes { get; set; }

        public DbSet<UserSettings> UserSettings { get; set; }

        public DbSet<ResearchFirstBarPullback> ResearchFirstBarPullbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trade>()
                .Property(e => e.TriggerPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Trade>()
                .Property(e => e.EntryPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Trade>()
                .Property(e => e.StopPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Trade>()
                .Property(e => e.ExitPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Trade>()
                .Property(e => e.PnL)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Trade>()
                .Property(e => e.ExitPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Trade>()
                .Property(e => e.Fee)
                .HasDefaultValueSql("0");


            modelBuilder.Entity<ResearchFirstBarPullback>()
                .Property(e => e.TriggerPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<ResearchFirstBarPullback>()
                .Property(e => e.EntryPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<ResearchFirstBarPullback>()
                .Property(e => e.StopPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<ResearchFirstBarPullback>()
                .Property(e => e.ExitPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<ResearchFirstBarPullback>()
                .Property(e => e.PnL)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<ResearchFirstBarPullback>()
                .Property(e => e.ExitPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<ResearchFirstBarPullback>()
                .Property(e => e.Fee)
                .HasDefaultValueSql("0");






            modelBuilder.Entity<Trade>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ResearchFirstBarPullback>()
               .Property(e => e.CreatedAt)
               .HasDefaultValueSql("GETDATE()");
        }
    }
}
