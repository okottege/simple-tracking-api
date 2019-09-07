using System;
using System.Collections.Generic;
using TrackerService.Core.CoreDomain.Definitions;

namespace TrackerService.Services.Users
{
    internal class UserProfileCache : IUserProfileCache
    {
        public List<IDomainUser> GetUsers()
        {
            throw new NotImplementedException();
        }

        public IDomainUser GetUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
