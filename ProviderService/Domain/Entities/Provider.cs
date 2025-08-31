using Amazon.DynamoDBv2.DataModel;

namespace ProviderService.Domain.Entities;

// <summary>
/// Map the Book Class to DynamoDb Table
/// To learn more visit https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DeclarativeTagsList.html
/// </summary>
[DynamoDBTable("ProviderData")]
public class Provider
{
    [DynamoDBHashKey]
    [DynamoDBProperty("PK")]
    public required string PartitionKey { get; init; }

    [DynamoDBRangeKey]
    [DynamoDBProperty("SK")]
    public required string ClasificationKey { get; init; }

    [DynamoDBProperty("idProvider")]
    public required string IdProvider { get; init; }

    [DynamoDBProperty("nameProvider")]
    public string NameProvider { get; set; } = string.Empty;

    [DynamoDBProperty("score")]
    public int? Score { get; set; }

    [DynamoDBProperty("typeProvider")]
    public string TypeProvider { get; set; } = string.Empty;

    [DynamoDBProperty("idFiscalProvider")]
    public string? IdFiscal { get; set; }

    [DynamoDBProperty("nit")]
    public string? Nit { get; set; }

    [DynamoDBProperty("idLocation")]
    public string IdLocation { get; set; } = string.Empty;
   
    [DynamoDBProperty("state")]
    public string State { get; set; } = string.Empty;

    [DynamoDBProperty("country")]
    public string Country { get; set; } = string.Empty;

    [DynamoDBProperty("city")]
    public string City { get; set; } = string.Empty;

    [DynamoDBProperty("address")]
    public string Address { get; set; } = string.Empty;

    [DynamoDBProperty("longitude")]
    public string Longitude { get; set; } = string.Empty;

    [DynamoDBProperty("latitude")]
    public string Latitude { get; set; } = string.Empty;

    [DynamoDBProperty("details")]
    public string Details { get; set; } = string.Empty;

    [DynamoDBProperty("idAgreement")]
    public string IdAgreement { get; set; } = string.Empty;

    [DynamoDBProperty("endValidity")]
    public string EndValidity { get; set; } = string.Empty;

    [DynamoDBProperty("startValidity")]
    public string StartValidity { get; set; } = string.Empty;

    [DynamoDBProperty("urlAttach")]
    public List<string> UrlAttach { get; set; } = [];

    [DynamoDBProperty("createdAt")]
    public string CreatedAt { get; set; } = string.Empty;

    [DynamoDBProperty("idPayment")]
    public string IdPayment { get; set; } = string.Empty;

    [DynamoDBProperty("isActivePayment")]
    public bool? IsActivePayment { get; set; } = null;

    [DynamoDBProperty("providerPayment")]
    public string ProviderPayment { get; set; } = string.Empty;

    [DynamoDBProperty("namePaymentMethod")]
    public string NamePaymentMethod { get; set; } = string.Empty;

    [DynamoDBProperty("typePayment")]
    public string TypePayment { get; set; } = string.Empty;

    [DynamoDBProperty("updatedAt")]
    public string UpdatedAt { get; set; } = string.Empty;

    [DynamoDBProperty("idContact")]
    public string IdContact { get; set; } = string.Empty;

    [DynamoDBProperty("listData")]
    public List<ListData>? ListData { get; set; } = null;

}