using System.ComponentModel.DataAnnotations.Schema;

namespace MasterRdsServices.Domain.Entities
{
    public class GeneralType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }  

        // Llave foránea a Categories
        public int CategoriesId { get; set; }

        [ForeignKey(nameof(CategoriesId))]
        [InverseProperty(nameof(Category.GeneralTypes))]
        public Category? Category { get; set; } = null;
    }
}
