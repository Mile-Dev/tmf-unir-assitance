using Amazon.DynamoDBv2.DataModel;
using PhoneConsultationService.Domain.Enum;

namespace PhoneConsultationService.Domain.Entities;


[DynamoDBTable("PhoneConsultations")]
public class PhoneConsultation
{
    [DynamoDBHashKey]
    [DynamoDBProperty("PK")]
    public required string PartitionKey { get; set; }

    [DynamoDBRangeKey]
    [DynamoDBProperty("SK")]
    public required string ClasificationKey { get; set; }

    public required string HistoryClinica { get; set; }

    public required string PhoneRecordId { get; set; }

    public DateTime CreatedAt { get; set; }
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
    public AssignTriage AssignTriage { get; set; }
    public DecisionType DecisionType { get; set; }
}

[DynamoDBTable("PhoneConsultations")]
public class Attachment
{
    [DynamoDBHashKey]
    [DynamoDBProperty("PK")]
    public required string PartitionKey { get; set; } ////idevent

    [DynamoDBRangeKey]
    [DynamoDBProperty("SK")]
    public required string ClasificationKey { get; set; } //idPhoneRecord attachment#20240812101349#Id
   
    public string PhoneRecordId { get; set; } = string.Empty;

    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

}
