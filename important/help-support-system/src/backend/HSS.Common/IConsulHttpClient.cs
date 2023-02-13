namespace HSS.Common
{
    public interface IConsulHttpClient
    {
        Task<T> GetAsync<T>(string serviceName, string requestUri);
        Task<Tout> PostAsync<Tout, Tin>(string serviceName, string requestUri, Tin data);
    }
}