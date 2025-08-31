namespace VoucherService.Common.HttpClient.Interface
{
    public interface IInvokeClientServices
    {
        /// <summary>
        /// Modificar la api para la petición.
        /// </summary>
        /// <param name="urlApi">Url de la api a invocar</param>
        void ApiInvoke(string urlApi);

        Task<TR> GetAsync<TR>(Dictionary<string, string> paramHeadersInvoke)
            where TR : class, new();

        Task<TR> GetAsync<TR>()
          where TR : class, new();

        Task<TR> PostAsync<T, TR>(T objectTransferencia)
            where T : class, new()
            where TR : class, new();
        Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest content);
        Task DeleteAsync(string url);
    }
}
