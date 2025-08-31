using Amazon.DynamoDBv2.DataModel;

namespace MasterRdsServices.Domain.Entities.Dynamodb
{
    [DynamoDBTable("CountriesAndCities")]
    public class Country
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("PK")]
        public required string PartitionKey { get; init; }

        [DynamoDBRangeKey]
        [DynamoDBProperty("SK")]
        public required string ClasificationKey { get; init; }

        [DynamoDBProperty("Ambulancias")]
        public string Ambulancia { get; set; } = string.Empty;

        [DynamoDBProperty("Bomberos")]
        public string Bombero { get; set; } = string.Empty;

        [DynamoDBProperty("Capital")]
        public string Capital { get; set; } = string.Empty;

        [DynamoDBProperty("Continent")]
        public string Continent { get; set; } = string.Empty;

        [DynamoDBProperty("Divisa")]
        public string Divisa { get; set; } = string.Empty;

        [DynamoDBProperty("Indicativo")]
        public string Indicativo { get; set; } = string.Empty;

        [DynamoDBProperty("IsoDos")]
        public string IsoDos { get; set; } = string.Empty;

        [DynamoDBProperty("MDxkhab")]
        public string MDHabitante { get; set; } = string.Empty;

        [DynamoDBProperty("Name")]
        public string Name { get; set; } = string.Empty;

        [DynamoDBProperty("NameSpanish")]
        public string NameSpanish { get; set; } = string.Empty;

        [DynamoDBProperty("Policia")]
        public string Policia { get; set; } = string.Empty;

        [DynamoDBProperty("Simbolo")]
        public string Simbolo { get; set; } = string.Empty;

        [DynamoDBProperty("UsoHorario")]
        public string UsoHorario { get; set; } = string.Empty;
    }
}
