namespace TrackerService.Common
{
    public class AuthenticationConfig : BaseAuthenticationOptions
    {
        public string Authority { get; set; }
        public string Realm { get; set; }
    }
}
