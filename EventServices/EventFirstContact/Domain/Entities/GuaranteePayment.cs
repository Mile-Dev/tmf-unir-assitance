using Amazon.DynamoDBv2.DataModel;

namespace EventServices.EventFirstContact.Domain.Entities
{
    public class GuaranteePayment
    {
        [DynamoDBProperty("amountLocal")]
        public decimal? AmountLocal { get; set; } = null;

        [DynamoDBProperty("guaranteePaymentStatus")]
        public Information GuaranteePaymentStatus { get; set; } = new Information();

        [DynamoDBProperty("typeMoney")]
        public string? TypeMoney { get; set; } = null;

        [DynamoDBProperty("amountUsd")]
        public decimal? AmountUsd { get; set; } = null;

        [DynamoDBProperty("exchangeRate")]
        public decimal? ExchangeRate { get; set; } = null;

        [DynamoDBProperty("deductibleAmountLocal")]
        public decimal? DeductibleAmountLocal { get; set; } = null;

        [DynamoDBProperty("deductibleAmountUsd")]
        public decimal? DeductibleAmountUsd { get; set; } = null;

    }
}
