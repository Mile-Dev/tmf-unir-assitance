using System.Security.Cryptography;

namespace EventServices.Domain.Entities
{
    public class Category
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Relación con GeneralTypes
        public ICollection<GeneralType> GeneralTypes { get; set; }
    }
}
