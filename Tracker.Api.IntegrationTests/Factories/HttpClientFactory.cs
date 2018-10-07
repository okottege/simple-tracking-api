using System;
using System.Net.Http;

namespace Tracker.Api.IntegrationTests.Factories
{
    public class HttpClientFactory
    {
        private static HttpClient serviceClient;
        private static HttpClient authenticationClient;

        private readonly string serviceBaseUrl;
        private readonly string authBaseUrl;

        public HttpClientFactory(string serviceBaseUrl, string authBaseUrl)
        {
            this.serviceBaseUrl = serviceBaseUrl;
            this.authBaseUrl = authBaseUrl;
        }

        public HttpClient CreateNewServiceClient()
        {
            return serviceClient ?? (serviceClient = CreateClient(serviceBaseUrl));
        }

        public HttpClient CreateNewAuthenticationClient()
        {
            return authenticationClient ?? (authenticationClient = CreateClient(authBaseUrl));
        }

        private HttpClient CreateClient(string baseAddress)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var client = new HttpClient(httpClientHandler) {BaseAddress = new Uri(baseAddress)};

            return client;
        }
    }
}
