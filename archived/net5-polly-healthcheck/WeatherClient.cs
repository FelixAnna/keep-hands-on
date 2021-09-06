using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProductApi
{
    public class WeatherClient
    {
        private HttpClient client;
        private ServiceSettings settings;

        public WeatherClient(HttpClient client, IOptions<ServiceSettings> options)
        {
            this.client = client;
            this.settings = options.Value;
        }

        public record Weather(string description);
        public record Main(decimal temp);
        public record Forecast(Weather[] weathers, Main main, long dt);

        public async Task<Forecast> GetForecatAsync(string city)
        {
            var result = new Forecast(new[] { new Weather("this is description") }, new Main(3), 123344555 );//await client.GetFromJsonAsync<Forecast>($"http://{settings.Host}/{city}?key={settings.ApiKey}");

            return result;
        }
    }
}
