namespace TrackerService.Api.Infrastructure.Authentication.Models
{
    public class UserAuthenticationConfiguration : BaseAuthenticationConfiguration
    {
        public string Realm { get; set; }
    }
}
