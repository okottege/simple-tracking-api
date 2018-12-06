using TrackerService.Common.Contracts;

namespace TrackerService.Api.Configuration
{
    public class UserRepositoryConfig : IUserRepositoryConfig
    {
        public string ConnectionName { get; set; }
        public string UserManagementBaseUrl { get; set; }
    }
}
