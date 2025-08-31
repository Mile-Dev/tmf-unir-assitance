using System.Security.Cryptography;

namespace EventServices.Domain.Entities
{
    public class Client
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

    }
}
