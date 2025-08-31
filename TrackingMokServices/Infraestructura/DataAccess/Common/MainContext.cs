using Microsoft.EntityFrameworkCore;
using TrackingMokServices.Domain.Entities;

namespace TrackingMokServices.Infraestructura.DataAccess.Common
{
    public class MainContext(DbContextOptions<MainContext> options) : DbContext(options)
    {
        public DbSet<EventMok> VwSummaryEventMOK { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventMok>();
        }
    }
}
