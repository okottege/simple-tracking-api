using TrackerService.Core.CoreDomain.Definitions;

namespace TrackerService.Services.DataObjects
{
    public class ApplicationUser : IDomainUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
