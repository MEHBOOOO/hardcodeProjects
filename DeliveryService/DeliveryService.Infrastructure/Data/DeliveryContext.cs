using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Models;

namespace DeliveryService.Infrastructure.Data
{
    public class DeliveryContext : DbContext
    {
        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options) { }

        public DbSet<DeliveryRequest> DeliveryRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryRequest>().Property(d => d.Address).IsRequired();
        }
    }
}