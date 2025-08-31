namespace EventServices.Domain.Dto.Query
{
    public class EmergencyContactGetDto
    {
        public string NameEmergencyContact { get; set; } = string.Empty;

        public string LastNameEmergencyContact { get; set; } = string.Empty;

        public string PhoneEmergencyContact { get; set; } = string.Empty;

        public string EmailEmergencyContact { get; set; } = string.Empty;

        public bool MainPersonEmergencyContact { get; set; } = false;
    }
}
