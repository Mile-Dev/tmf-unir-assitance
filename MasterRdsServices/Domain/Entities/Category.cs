namespace MasterRdsServices.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = null;
        public bool? IsConfigurationField { get; set; } = null;

        // Relación con GeneralTypes
        public ICollection<GeneralType> GeneralTypes { get; set; }
    }
}
