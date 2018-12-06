namespace TrackerService.Api.Infrastructure.Authentication.Models
{
    public class BaseAuthenticationConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string Audience { get; set; }
        public string AuthBaseUrl { get; set; }
    }
}
