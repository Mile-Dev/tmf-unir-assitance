namespace EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb
{
    public class EmergencyContactQueryDto
    {
        public string? IdEmergencyContact { get; set; } = null;

        public string NameEmergencyContact { get; set; } = string.Empty;

        public string LastNameEmergencyContact { get; set; } = string.Empty;

        public string PhoneEmergencyContact { get; set; } = string.Empty;

        public string EmailEmergencyContact { get; set; } = string.Empty;

        public bool MainPersonEmergencyContact { get; set; } = false;
    }
}
