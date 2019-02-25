using TrackerService.Common.Contracts;

namespace TrackerService.Api.Configuration
{
    internal class UserManagementOptions : IUserManagementOptions
    {
        public string Audience { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string BaseUrl { get; set; }
        public string ConnectionID { get; set; }
    }
}
