
namespace EventServices.Domain.Entities
{
    public class GuaranteePaymentStatus
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Relación con GeneralTypes
        public ICollection<GuaranteePayment> GuaranteePayment { get; set; }
    }
}
