using TrackerService.Common.Contracts;

namespace TrackerService.Data.DataObjects.UserManagement
{
    public class UserAuthenticationResult : IUserAuthenticationResult
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string Scope { get; set; }
    }
}
