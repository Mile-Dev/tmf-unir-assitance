namespace EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb
{
    public class ResponseEventFirstContactEmergencyContactDto
    {
        public string? Id { get; set; } = null;

        public string Screen { get; set; } = string.Empty;

        public List<EmergencyContactQueryDto>? ListEmergencyContactEvent { get; set; } = null;
    }
}
