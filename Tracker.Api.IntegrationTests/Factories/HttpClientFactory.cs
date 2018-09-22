using System.Net.Http;

namespace Tracker.Api.IntegrationTests.Factories
{
    public class HttpClientFactory
    {
        private static HttpClient client;

        public HttpClient CreateNewClient()
        {
            if (client == null)
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                client = new HttpClient(httpClientHandler);
            }

            return client;
        }
    }
}
