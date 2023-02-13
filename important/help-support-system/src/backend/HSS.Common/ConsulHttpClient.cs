using Consul;
using HSS.Common.Exceptions;
using Newtonsoft.Json;
using System.Text;

namespace HSS.Common
{
    public class ConsulHttpClient : IConsulHttpClient
    {
        private readonly HttpClient _client;
        private readonly IConsulClient _consulclient;

        public ConsulHttpClient(HttpClient client, IConsulClient consulclient)
        {
            _client = client;
            _consulclient = consulclient;
        }

        public async Task<T> GetAsync<T>(string serviceName, string requestUri)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);

            var response = await _client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }
        public async Task<Tout> PostAsync<Tout, Tin>(string serviceName, string requestUri, Tin data)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);

            var payload = JsonConvert.SerializeObject(data);
            HttpContent strContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(uri, strContent);

            if (!response.IsSuccessStatusCode)
            {
                return default(Tout);
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Tout>(content);
        }

        private async Task<Uri> GetRequestUriAsync(string serviceName, string uri)
        {
            //Get all services registered on Consul
            var allRegisteredServices = await _consulclient.Agent.Services();

            //Get all instance of the service went to send a request to
            var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();

            //Get a random instance of the service
            var service = GetRandomInstance(registeredServices);

            if (service == null)
            {
                throw new HSSConsulServiceNotFoundException($"Consul service: '{serviceName}' was not found.");
            }

            var uriBuilder = new UriBuilder(new Uri(service.Address))
            {
                Port = service.Port,
                Path = uri
            };

            return uriBuilder.Uri;
        }

        private static AgentService GetRandomInstance(IList<AgentService> services)
        {
            Random _random = new();
            AgentService servToUse = services[_random.Next(0, services.Count)];
            return servToUse;
        }
    }
}
