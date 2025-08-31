using EventStatusSwitchTempServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventStatusSwitchTempServices.Infraestructura.DataAccess
{
    public class MainContext(DbContextOptions<MainContext> options) : DbContext(options)
    {
        public DbSet<EventStatus> EventStatus { get; set; }

        public DbSet<Events> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventStatus>();
            modelBuilder.Entity<Events>();
        }
    }
}
