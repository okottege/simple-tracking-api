namespace TrackerService.Common
{
    public class BaseAuthenticationOptions
    {
        public string Audience { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
    }
}
