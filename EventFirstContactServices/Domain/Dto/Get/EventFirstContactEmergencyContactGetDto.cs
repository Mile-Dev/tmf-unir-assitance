
namespace EventFirstContactServices.Domain.Dto.Get
{
    public class EventFirstContactEmergencyContactGetDto
    {
        public string? Id { get; set; } = null;

        public string Screen { get; set; } = string.Empty;

        public List<EmergencyContactDto>? ListEmergencyContactEvent { get; set; } = null;

    }
}
