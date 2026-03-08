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
        }
        public DbSet<PostOfficeOfficer> Officers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
