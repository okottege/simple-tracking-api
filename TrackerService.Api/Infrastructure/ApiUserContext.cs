using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using TrackerService.Api.Infrastructure.Contracts;

namespace TrackerService.Api.Infrastructure
{
    public class ApiUserContext : IUserContext
    {
        private readonly HttpContext context;

        public ApiUserContext(IHttpContextAccessor contextAccessor)
        {
            context = contextAccessor.HttpContext;
        }

        public string UserId => IsUserAuthenticated() ? GetClaim(UserClaimTypes.USER_ID) : null;
        public string Email => IsUserAuthenticated() ? GetClaim(UserClaimTypes.EMAIL) : null;

        public async Task<string> GetAccessToken()
        {
            return await context.GetTokenAsync("access_token");
        }

        private bool IsUserAuthenticated()
        {
            return context.User?.Identity?.IsAuthenticated == true;
        }

        private string GetClaim(string type)
        {
            return context.User?.Claims.FirstOrDefault(c => c.Type == type)?.Value;
        }
    }
}
