namespace EventServices.Domain.Entities
{
    public class VoucherStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Relación con Vouchers
        public ICollection<Voucher> Vouchers { get; set; }
    }
}
