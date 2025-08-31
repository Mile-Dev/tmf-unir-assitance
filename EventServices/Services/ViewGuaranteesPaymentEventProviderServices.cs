using AutoMapper;
using EventServices.Domain.Dto.Query;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Services.Interfaces;

namespace EventServices.Services
{
    /// <summary>
    /// Servicio para la gestión de la consulta de pagos de garantías asociados a proveedores de eventos.
    /// Utiliza un UnitOfWork para acceder a los repositorios y AutoMapper para la conversión de entidades a DTOs.
    /// </summary>
    public class ViewGuaranteesPaymentEventProviderServices(IUnitOfWork unitOfWork, ILogger<ViewGuaranteesPaymentEventProviderServices> logger, IMapper mapper) : IViewGuaranteesPaymentEventProviderServices
    {
        /// <summary>
        /// Unidad de trabajo para acceder a los repositorios de datos.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Logger para el registro de información y errores en el servicio.
        /// </summary>
        private readonly ILogger<ViewGuaranteesPaymentEventProviderServices> _logger = logger;

        /// <summary>
        /// Mapeador para la conversión de entidades a DTOs y viceversa.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Obtiene de forma asíncrona la lista de pagos de garantías asociados a un proveedor de evento específico.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>Lista de DTOs con la información de los pagos de garantías.</returns>
        public async Task<List<ViewGuaranteesPaymentEventProviderGetDto>> GetGuaranteesPaymentByIdEventProviderAsync(int id)
        {
            _logger.LogInformation("GetGuaranteesPaymentByIdEventProviderAsync");
            var ListGuaranteesPaymentByIdEvent = await _unitOfWork.ViewGuaranteesPaymentEventProviderRepository.GetGuaranteesPaymentByIdEventProviderAsync(id);
            var listResultGuaranteesPayment = _mapper.Map<List<ViewGuaranteesPaymentEventProviderGetDto>>(ListGuaranteesPaymentByIdEvent);

            return listResultGuaranteesPayment;
        }
    }
}
