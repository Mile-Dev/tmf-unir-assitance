using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using PhoneConsultationService.Domain.Entities;
using PhoneConsultationService.Domain.Interfaces;

namespace PhoneConsultationService.DataAccess;

public class PhoneConsultationRepository(IAmazonDynamoDB amazonDynamoDb) : IPhoneConsultationRepository
{
    private readonly DynamoDBContext _context = new(amazonDynamoDb);

    public async Task<PhoneConsultation> CreateAsync(PhoneConsultation phoneConsultation)
    {
        try
        {
            await _context.SaveAsync(phoneConsultation);
            return phoneConsultation;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saving PhoneConsultation: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<PhoneConsultation>> GetListByIdAsync(string idEvent, string field, string value)
    {
        var conditions = new DynamoDBOperationConfig
        {
            ConditionalOperator = ConditionalOperatorValues.And,
            QueryFilter =
                [
                    new ScanCondition(field, ScanOperator.BeginsWith, value)
                ]
        };

        return await _context.QueryAsync<PhoneConsultation>(idEvent, conditions).GetRemainingAsync();
    }

    public async Task<PhoneConsultation> GetIdPhoneRecordByIdEventAsync(string idEvent, string idPhoneRecord)
    {
        return await _context.LoadAsync<PhoneConsultation>(idEvent, idPhoneRecord);
    }

    public async Task<bool> BatchWriteUpdateAsync(List<Attachment> attachment)
    {
        if (attachment == null) return false;

        try
        {
            var prividerBatch = _context.CreateBatchWrite<Attachment>();
            prividerBatch.AddPutItems(attachment);
            await prividerBatch.ExecuteAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return true;
    }

    public async Task<IEnumerable<Attachment>> GetListAttachmentByIdAsync(string idEvent, string field, string value)
    {
        var conditions = new DynamoDBOperationConfig
        {
            ConditionalOperator = ConditionalOperatorValues.And,
            QueryFilter =
                [
                    new ScanCondition(field, ScanOperator.BeginsWith, value)
                ]
        };

        return await _context.QueryAsync<Attachment>(idEvent, conditions).GetRemainingAsync();
    }
}
