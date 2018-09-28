using System.Collections.Generic;

namespace Tracker.Api.IntegrationTests.Configuration
{
    public class AuthenticationInfo
    {
        public AuthenticationInfo(IDictionary<string, object> properties)
        {
            ClientId = properties["clientId"].ToString();
            Resource = properties["resource"].ToString();
            Username = properties["username"].ToString();
            Password = properties["password"].ToString();
            Audience = properties["audience"].ToString();
            ClientSecret = properties["clientSecret"].ToString();
            TenantId = properties["tenantId"].ToString();
            AuthenticationBaseUrl = properties["authBaseUrl"].ToString();
        }

        public string ClientId { get; }
        public string Resource { get; }
        public string Username { get; }
        public string Password { get; }
        public string ClientSecret { get; }
        public string TenantId { get; }
        public string AuthenticationBaseUrl { get; }
        public string Audience { get; }
    }
}
