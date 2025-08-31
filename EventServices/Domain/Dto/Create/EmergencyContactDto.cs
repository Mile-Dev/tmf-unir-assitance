namespace EventServices.Domain.Dto.Create
{
    public class EmergencyContactDto
    {
        public string? NameEmergencyContact { get; set; } = null;

        public string? LastNameEmergencyContact { get; set; } = null;

        public string? PhoneEmergencyContact { get; set; } = null;

        public string? EmailEmergencyContact { get; set; } = null;

        public bool? MainPersonEmergencyContact { get; set; } = null;
    }
}
