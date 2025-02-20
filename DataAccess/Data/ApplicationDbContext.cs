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

            // Configuring TPT (Table Per Type) inheritance strategy
            modelBuilder.Entity<BaseTrade>()
                .ToTable("BaseTrades"); // Base class table

            modelBuilder.Entity<Trade>()
                .ToTable("Trades") // Table for Trade
                .HasBaseType<BaseTrade>(); // Inherit from BaseTrade

            modelBuilder.Entity<ResearchFirstBarPullback>()
                .ToTable("ResearchFirstBarPullbacks") // Table for ResearchTypeA
                .HasBaseType<BaseTrade>(); // Inherit from BaseTrade
        }
    }
}
