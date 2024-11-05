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

        public DbSet<ResearchFirstBarPullback> ResearchFirstBarPullbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaperTrade>()
                .Property(e => e.TradeDurationInCandles)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<PaperTrade>()
                .Property(e => e.TriggerPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<PaperTrade>()
                .Property(e => e.EntryPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<PaperTrade>()
                .Property(e => e.StopPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<PaperTrade>()
                .Property(e => e.ExitPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<PaperTrade>()
                .Property(e => e.PnL)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<PaperTrade>()
                .Property(e => e.ExitPrice)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<PaperTrade>()
                .Property(e => e.Fee)
                .HasDefaultValueSql("0");




            modelBuilder.Entity<ResearchFirstBarPullback>()
                .Property(e => e.TradeDurationInCandles)
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






            modelBuilder.Entity<PaperTrade>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ResearchFirstBarPullback>()
               .Property(e => e.CreatedAt)
               .HasDefaultValueSql("GETDATE()");
        }
    }
}
