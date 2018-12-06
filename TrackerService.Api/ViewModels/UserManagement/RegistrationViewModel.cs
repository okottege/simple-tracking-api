using TrackerService.Common.Contracts;

namespace TrackerService.Api.ViewModels.UserManagement
{
    public class RegistrationViewModel : IUserManagementAction
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string ServiceToken { get; set; }
    }
}
