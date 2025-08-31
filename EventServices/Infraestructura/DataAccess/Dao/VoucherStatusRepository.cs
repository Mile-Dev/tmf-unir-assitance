using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio específico para la entidad VoucherStatus.
    /// Hereda de Repository<VoucherStatus> y expone las operaciones definidas en IVoucherStatusRepository.
    /// </summary>
    public class VoucherStatusRepository : Repository<VoucherStatus>, IVoucherStatusRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="VoucherStatusRepository"/>.
        /// </summary>
        /// <param name="context">Contexto de base de datos principal.</param>
        public VoucherStatusRepository(MainContext context) : base(context)
        {
        }
    }
}
