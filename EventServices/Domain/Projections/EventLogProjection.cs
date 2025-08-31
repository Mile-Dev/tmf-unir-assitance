namespace EventServices.Domain.Projections
{
    /// <summary>
    /// Representa una proyección de los datos de un evento para el registro de logs.
    /// </summary>
    public class EventLogProjection
    {
        /// <summary>
        /// Identificador único del registro de evento.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del voucher asociado al evento.
        /// </summary>
        public string? VoucherName { get; set; }

        /// <summary>
        /// Código del cliente relacionado con el evento.
        /// </summary>
        public string? ClientCode { get; set; }

        /// <summary>
        /// Identificador único del cliente relacionado con el evento.
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Nombre del estado del evento.
        /// </summary>
        public string? EventStatusName { get; set; }

        /// <summary>
        /// Identificador del estado del evento.
        /// </summary>
        public int? EventStatusId { get; set; }
    }
}
