using Amazon.DynamoDBv2.DataModel;

namespace EventServices.EventFirstContact.Domain.Entities
{
    public class Information
    {
        [DynamoDBProperty("id")]
        public int Id { get; set; }

        [DynamoDBProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
