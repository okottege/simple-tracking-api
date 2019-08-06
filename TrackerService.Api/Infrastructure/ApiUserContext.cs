using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using TrackerService.Core.CoreDomain;

namespace TrackerService.Api.Infrastructure
{
    public class ApiUserContext : IUserContext
    {
        private readonly IHttpContextAccessor contextAccessor;

        public ApiUserContext(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public string UserId => IsUserAuthenticated() ? GetClaim(UserClaimTypes.USER_ID) : null;
        public string Email => IsUserAuthenticated() ? GetClaim(UserClaimTypes.EMAIL) : null;

        public async Task<string> GetAccessToken()
        {
            return await contextAccessor.HttpContext.GetTokenAsync("access_token");
        }

        public bool IsAdmin => GetClaim(UserClaimTypes.ROLES).Contains("admin");

        private bool IsUserAuthenticated()
        {
            return contextAccessor.HttpContext.User?.Identity?.IsAuthenticated == true;
        }

        private string GetClaim(string type)
        {
            return GetClaims(type).FirstOrDefault();
        }

        private string[] GetClaims(string type)
        {
            return contextAccessor.HttpContext.User?.Claims.Where(c => c.Type == type)
                .Select(c => c.Value)
                .ToArray() ?? new string []{};
        }
    }
}
