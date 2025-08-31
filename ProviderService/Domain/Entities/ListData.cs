using Amazon.DynamoDBv2.DataModel;

namespace ProviderService.Domain.Entities
{
    public class ListData
    {
        [DynamoDBProperty("key")]
        public string Key { get; set; } = string.Empty;

        [DynamoDBProperty("value")]
        public string Value { get; set; } = string.Empty;
    }
}
