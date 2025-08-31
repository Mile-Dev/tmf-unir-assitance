using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;

namespace EventServices.Infraestructura.DataAccess.Interface
{
    /// <summary>
    /// Define la unidad de trabajo para el acceso a datos, agrupando todos los repositorios y el método para guardar los cambios.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositorio para operaciones sobre categorías.
        /// </summary>
        ICategoriesRepository CategoriesRepository { get; }

        /// <summary>
        /// Repositorio para contactos de emergencia.
        /// </summary>
        IContactEmergencyRepository ContactEmergencyRepository { get; }

        /// <summary>
        /// Repositorio para información de contacto.
        /// </summary>
        IContactInformationRepository ContactInformationRepository { get; }

        /// <summary>
        /// Repositorio para viajes de clientes.
        /// </summary>
        ICustomerTripRepository CustomerTripRepository { get; }

        /// <summary>
        /// Repositorio para proveedores de eventos.
        /// </summary>
        IEventProviderRepository EventProviderRepository { get; }

        /// <summary>
        /// Repositorio para estados de proveedores de eventos.
        /// </summary>
        IEventProviderStatusRepository EventProviderStatusRepository { get; }

        /// <summary>
        /// Repositorio para eventos.
        /// </summary>
        IEventsRepository EventsRepository { get; }

        /// <summary>
        /// Repositorio para estados de eventos.
        /// </summary>
        IEventStatusRepository EventStatusRepository { get; }

        /// <summary>
        /// Repositorio para tipos generales.
        /// </summary>
        IGeneralTypesRepository GeneralTypesRepository { get; }

        /// <summary>
        /// Repositorio para notas de eventos.
        /// </summary>
        INotesRepository NotesRepository { get; }

        /// <summary>
        /// Repositorio para vales.
        /// </summary>
        IVouchersRepository VouchersRepository { get; }

        /// <summary>
        /// Repositorio para estados de vales.
        /// </summary>
        IVoucherStatusRepository VoucherStatusRepository { get; }

        /// <summary>
        /// Repositorio para coberturas de eventos.
        /// </summary>
        IEventCoveragesRepository EventCoveragesRepository { get; }

        /// <summary>
        /// Repositorio para la vista de eventos.
        /// </summary>
        IViewEventsRepository ViewEventsRepository { get; }

        /// <summary>
        /// Repositorio para la vista de detalles de eventos.
        /// </summary>
        IViewEventDetailsRepository ViewEventDetailsRepository { get; }

        /// <summary>
        /// Repositorio para la vista de eventos de consulta telefónica.
        /// </summary>
        IViewPhoneConsultationEventsRepository ViewPhoneConsultationEventsRepository { get; }

        /// <summary>
        /// Repositorio para pagos de garantía.
        /// </summary>
        IGuaranteePaymentRepository GuaranteePaymentRepository { get; }

        /// <summary>
        /// Repositorio para la vista de pagos de garantía por proveedor de evento.
        /// </summary>
        IViewGuaranteesPaymentEventProviderRepository ViewGuaranteesPaymentEventProviderRepository { get; }

        /// <summary>
        /// Repositorio para clientes.
        /// </summary>
        IClientRepository ClientRepository { get; }

        /// <summary>
        /// Repositorio para consultas telefónicas.
        /// </summary>
        IPhoneConsultationRepository PhoneConsultationRepository { get; }

        /// <summary>
        /// Repositorio para operaciones sobre categorías.
        /// </summary>
        IDocumentsRepository DocumentsRepository { get; }

        /// <summary>
        /// Guarda los cambios realizados en la unidad de trabajo de manera asíncrona.
        /// </summary>
        /// <returns>Número de registros afectados.</returns>
        Task<int> CompleteAsync();
    }
}
