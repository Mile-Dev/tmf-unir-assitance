using PhoneConsultationService.Domain.Enum;
using System.Text.Json.Serialization;

namespace PhoneConsultationService.Domain.Dto;
public class PhoneConsultationDto
{
    public required string IdEvent { get; set; }
    public string PhoneRecordId { get; set; } = string.Empty;
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public DateTime DateBirth { get; set; }
    public required string Doctor { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string CurrentIllness { get; set; } = string.Empty;
    public string MedicalHistory { get; set; } = string.Empty;
    public string SystemReview { get; set; } = string.Empty;
    public string Analysis { get; set; } = string.Empty;
    public string CodeDiagnosis { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string FormulationTmo { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string Recommendations { get; set; } = string.Empty;
    public string MedicalSummary { get; set; } = string.Empty;

    public string AssignTriage { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DecisionType DecisionType { get; set; }

    public List<AttachmentDto> Attachments { get; set; } = [];
}

public class PhoneConsultationGetDto
{
    public required string IdEvent { get; set; }
    public required string HistoryClinica { get; set; }
    public required string PhoneRecordId { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Doctor { get; set; }
}

public class AttachmentDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}