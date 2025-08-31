namespace MasterRdsServices.Domain.Dto.Provider.Query
{
    public class ProviderTypeGetDto
    {
        public int Id { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;
    }
}
