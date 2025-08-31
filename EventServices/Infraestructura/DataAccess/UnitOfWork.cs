using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Dao;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;

namespace EventServices.Infraestructura.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly MainContext _context;

        public UnitOfWork(MainContext context)
        {
            _context = context;
            CategoriesRepository = new CategoriesRepository(_context);
            ContactEmergencyRepository = new ContactEmergencyRepository(_context);
            ContactInformationRepository = new ContactInformationRepository(_context);
            CustomerTripRepository = new CustomerTripRepository(_context);
            EventProviderRepository = new EventProviderRepository(_context);
            EventProviderStatusRepository = new EventProviderStatusRepository(_context);
            EventsRepository = new EventsRepository(_context);
            EventStatusRepository = new EventStatusRepository(_context);
            GeneralTypesRepository = new GeneralTypesRepository(_context);
            NotesRepository = new NotesRepository(_context);
            VouchersRepository = new VouchersRepository(_context);
            VoucherStatusRepository = new VoucherStatusRepository(_context);
            EventCoveragesRepository = new EventCoveragesRepository(_context);
            ViewEventsRepository = new ViewEventsRepository(_context);
            ViewEventDetailsRepository = new ViewEventDetailsRepository(_context);
            ViewPhoneConsultationEventsRepository = new ViewPhoneConsultationEventsRepository(_context);
            GuaranteePaymentRepository = new GuaranteePaymentRepository(_context);
            ViewGuaranteesPaymentEventProviderRepository = new ViewGuaranteesPaymentEventProviderRepository(_context);
            GuaranteePaymentStatusRepository = new GuaranteePaymentStatusRepository(_context);
            ClientRepository = new ClientRepository(_context);
            PhoneConsultationRepository = new PhoneConsultationRepository(_context);
            DocumentsRepository = new DocumentsRepository(_context);
        }

        public ICategoriesRepository CategoriesRepository { get; private set; }

        public IContactEmergencyRepository ContactEmergencyRepository { get; private set; }

        public IContactInformationRepository ContactInformationRepository { get; private set; }

        public ICustomerTripRepository CustomerTripRepository { get; private set; }

        public IEventProviderRepository EventProviderRepository { get; private set; }

        public IEventProviderStatusRepository EventProviderStatusRepository { get; private set; }

        public IEventsRepository EventsRepository { get; private set; }

        public IEventStatusRepository EventStatusRepository { get; private set; }

        public IGeneralTypesRepository GeneralTypesRepository { get; private set; }

        public INotesRepository NotesRepository { get; private set; }

        public IVouchersRepository VouchersRepository { get; private set; }

        public IVoucherStatusRepository VoucherStatusRepository { get; private set; }

        public IEventCoveragesRepository EventCoveragesRepository { get; private set; }

        public IViewEventsRepository ViewEventsRepository { get; private set; }

        public IViewEventDetailsRepository ViewEventDetailsRepository { get; private set; }

        public IViewPhoneConsultationEventsRepository ViewPhoneConsultationEventsRepository { get; private set; }

        public IGuaranteePaymentRepository GuaranteePaymentRepository { get; private set; }

        public IViewGuaranteesPaymentEventProviderRepository ViewGuaranteesPaymentEventProviderRepository { get; private set; }

        public IGuaranteePaymentStatusRepository GuaranteePaymentStatusRepository { get; private set; }

        public IClientRepository ClientRepository { get; private set; }

        public IPhoneConsultationRepository PhoneConsultationRepository { get; private set; }

        public IDocumentsRepository DocumentsRepository { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
