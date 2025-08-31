using System.Threading.Tasks;

namespace SQSProducerServices.Common.Interfaces
{
    public interface ISqsService
    {
        Task SendMessageAsync<T>(T message, string queueUrl);
    }
}
