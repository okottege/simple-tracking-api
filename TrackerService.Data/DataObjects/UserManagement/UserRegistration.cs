using TrackerService.Common.Contracts;

namespace TrackerService.Data.DataObjects
{
    public class UserRegistration : IUserManagementAction
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ServiceToken { get; set; }
    }
}
