using Microsoft.EntityFrameworkCore;
using SmartPostOffice.Models;

namespace SmartPostOffice.Data
{
    public class PostOfficeDbContext : DbContext
    {
        public PostOfficeDbContext(DbContextOptions<PostOfficeDbContext> options) : base(options)
        {
        }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceRequest>()
                .Property(s => s.ServiceType)
                .HasConversion<int>();

            modelBuilder.Entity<ServiceRequest>()
                .Property(s => s.Status)
                .HasConversion<int>();

            modelBuilder.Entity<CashBookEntry>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<OrdinaryLetterEntry>("OrdinaryLetterEntry")
                .HasValue<RegisteredMailEntry>("RegisteredMailEntry")
                .HasValue<SpeedPostEntry>("SpeedPostEntry")
                .HasValue<OrdinaryParcelEntry>("OrdinaryParcelEntry")
                .HasValue<CODEntry>("CODEntry");

            modelBuilder.Entity<DayBalance>()
                .Property(d => d.ServiceType)
                .HasConversion<int>();

        }
        public DbSet<PostOfficeOfficer> Officers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TrackingHistory> TrackingHistory { get; set; }
        public DbSet<CashBookEntry> CashBookEntries { get; set; }
        public DbSet<DayBalance> DayBalances { get; set; }

    }
}
