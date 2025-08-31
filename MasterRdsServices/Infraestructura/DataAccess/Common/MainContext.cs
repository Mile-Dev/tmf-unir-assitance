using MasterRdsServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MasterRdsServices.Infraestructura.DataAccess.Common
{
    public class MainContext(DbContextOptions<MainContext> options) : DbContext(options)
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<EventStatus> EventStatus { get; set; }
        public DbSet<EventProviderStatus> EventProviderStatus { get; set; }
        public DbSet<GeneralType> GeneralType { get; set; }
        public DbSet<VoucherStatus> VoucherStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<EventStatus>();
            modelBuilder.Entity<EventProviderStatus>();
            modelBuilder.Entity<GeneralType>();
            modelBuilder.Entity<VoucherStatus>();            
        }
    }
}
