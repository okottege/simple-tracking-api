using System;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TrackerService.Api.Infrastructure.Contracts;
using TrackerService.Common;
using TrackerService.Data.ServiceAccessResponses;

namespace TrackerService.Api.Infrastructure.Authentication
{
    public class ServiceToServiceAuthenticator : IServiceAuthenticator
    {
        private readonly HttpClient client;
        private readonly UserManagementConfig config;
        private readonly IDistributedCache cache;
        private const string AUTH_TOKEN_KEY = "AUTH_SERVICE_TOKEN";

        public ServiceToServiceAuthenticator(IHttpClientFactory factory, UserManagementConfig config, IDistributedCache cache)
        {
            client = factory.CreateClient(HttpClientNames.AUTHENTICATION_CLIENT);
            this.config = config;
            this.cache = cache;
        }

        public async Task<string> AuthenticateAsync()
        {
            var cachedToken = cache.GetString(AUTH_TOKEN_KEY);

            if (!string.IsNullOrEmpty(cachedToken))
            {
                return cachedToken;
            }

            dynamic reqContent = new ExpandoObject();
            reqContent.grant_type = config.GrantType;
            reqContent.client_id = config.ClientID;
            reqContent.client_secret = config.ClientSecret;
            reqContent.audience = config.Audience;

            var content = new StringContent(JsonConvert.SerializeObject(reqContent), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("oauth/token", content);
            var tokenResponse = await response.GetContent<GetTokenResponse>();
            
            cache.SetString(
                AUTH_TOKEN_KEY, 
                tokenResponse.access_token, 
                new DistributedCacheEntryOptions { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(2))});

            return tokenResponse.access_token;
        }
    }
}
