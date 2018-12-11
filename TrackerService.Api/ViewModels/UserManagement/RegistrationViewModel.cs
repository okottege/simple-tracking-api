using System.ComponentModel.DataAnnotations;
using TrackerService.Common.Contracts;

namespace TrackerService.Api.ViewModels.UserManagement
{
    public class RegistrationViewModel : IUserManagementAction
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string ServiceToken { get; set; }
    }
}
