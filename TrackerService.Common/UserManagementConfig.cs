namespace TrackerService.Common
{
    public class UserManagementConfig : BaseAuthenticationOptions
    {
        public string BaseUrl { get; set; }
        public string ConnectionID { get; set; }
    }
}
