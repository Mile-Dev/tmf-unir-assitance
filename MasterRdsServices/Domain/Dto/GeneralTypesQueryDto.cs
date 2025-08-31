namespace MasterRdsServices.Domain.Dto
{
    public class GeneralTypesQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoriesId { get; set; } = string.Empty;
        public string CategoriesName { get; set; } = string.Empty;

    }
}
