using EventServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Common
{
    public class MainContext(DbContextOptions<MainContext> options) : DbContext(options)
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<EventStatus> EventStatus { get; set; }
        public DbSet<EventProviderStatus> EventProviderStatus { get; set; }
        public DbSet<GeneralType> GeneralType { get; set; }
        public DbSet<VoucherStatus> VoucherStatus { get; set; }
        public DbSet<CustomerTrip> CustomerTrip { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<ContactInformation> ContactInformation { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventCoverage> EventCoverage { get; set; }
        public DbSet<ContactEmergency> ContactEmergency { get; set; }
        public DbSet<EventProvider> EventProvider { get; set; }
        public DbSet<EventNote> EventNote { get; set; }
        public DbSet<ViewEvent> VwEvent { get; set; }
        public DbSet<ViewEventDetail> VwEventDetail { get; set; }
        public DbSet<ViewPhoneConsultationEvent> VwPhoneConsultationEvent { get; set; }
        public DbSet<GuaranteePayment> GuaranteePayment { get; set; }
        public DbSet<ViewGuaranteesPaymentEventProvider> VwGuaranteesPaymentEventProvider { get; set; }
        public DbSet<GuaranteePaymentStatus> GuaranteePaymentStatus { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<PhoneConsultation> PhoneConsultation { get; set; }

        
        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyUtcDateTimeKind();
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<EventStatus>();
            modelBuilder.Entity<EventProviderStatus>();
            modelBuilder.Entity<GeneralType>();
            modelBuilder.Entity<VoucherStatus>();
            modelBuilder.Entity<CustomerTrip>();
            modelBuilder.Entity<Voucher>();
            modelBuilder.Entity<ContactInformation>();
            modelBuilder.Entity<Event>()
                        .Property(e => e.TravelPurpose)
                        .HasConversion<string>();
            modelBuilder.Entity<EventCoverage>();
            modelBuilder.Entity<ContactEmergency>();
            modelBuilder.Entity<EventProvider>();
            modelBuilder.Entity<EventNote>();
            modelBuilder.Entity<ViewEvent>();
            modelBuilder.Entity<ViewEventDetail>();
            modelBuilder.Entity<ViewPhoneConsultationEvent>();
            modelBuilder.Entity<GuaranteePayment>();
            modelBuilder.Entity<ViewGuaranteesPaymentEventProvider>();
            modelBuilder.Entity<GuaranteePaymentStatus>();
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<PhoneConsultation>();

        }
        #endregion
    }

}
