using System.Collections.Generic;

namespace Tracker.Api.IntegrationTests.Configuration
{
    public class AuthenticationInfo
    {
        public AuthenticationInfo(IDictionary<string, object> properties)
        {
            ClientId = properties["clientId"].ToString();
            Audience = properties["audience"].ToString();
            ClientSecret = properties["clientSecret"].ToString();
            AuthenticationBaseUrl = properties["authBaseUrl"].ToString();
        }

        public string ClientId { get; }
        public string ClientSecret { get; }
        public string AuthenticationBaseUrl { get; }
        public string Audience { get; }
    }
}
