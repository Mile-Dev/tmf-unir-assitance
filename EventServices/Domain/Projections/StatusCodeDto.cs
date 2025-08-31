namespace EventServices.Domain.Projections
{
    /// <summary>
    /// DTO que representa un código de estado con su identificador.
    /// </summary>
    public class StatusCodeDto
    {
        /// <summary>
        /// Identificador único del código de estado.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código de estado en formato texto.
        /// </summary>
        public string Code { get; set; } = null!;
    }
}
