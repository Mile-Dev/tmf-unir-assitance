using VoucherService.Common.HttpClient.Interface;

namespace VoucherService.Common.HttpClient
{
    public class InvokeClientBase : IInvokeClientServices
    {
        private readonly IInvokeClientServices _iInvokeClientServices;

        public InvokeClientBase(IInvokeClientServices iInvokeClientServices)
        {
            _iInvokeClientServices = iInvokeClientServices;
        }
        public void ApiInvoke(string urlApi)
        {
            this._iInvokeClientServices.ApiInvoke(urlApi);
        }

        public Task<TR> GetAsync<TR>(Dictionary<string, string> paramHeadersInvoke) where TR : class, new()
        {
            return this._iInvokeClientServices.GetAsync<TR>(paramHeadersInvoke);
        }

        public Task DeleteAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest content)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest content)
        {
            throw new NotImplementedException();
        }

        public Task<TR> GetAsync<TR>() where TR : class, new()
        {
            return this._iInvokeClientServices.GetAsync<TR>();
        }

        public Task<TR> PostAsync<T, TR>(T objectTransferencia)
            where T : class, new()
            where TR : class, new()
        {
            return this._iInvokeClientServices.PostAsync<T, TR>(objectTransferencia);
        }
    }
}
