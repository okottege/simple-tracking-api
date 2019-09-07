using System.Collections.Generic;
using TrackerService.Core.CoreDomain.Definitions;

namespace TrackerService.Services.Users
{
    public interface IUserProfileCache
    {
        List<IDomainUser> GetUsers();
        IDomainUser GetUser(string userId);
    }
}