using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using TrackerService.Api.Infrastructure.Contracts;
using TrackerService.Common.Contracts;

namespace TrackerService.Api.Infrastructure.Filters
{
    public class RequireServiceToken : IAsyncActionFilter
    {
        private readonly IServiceAuthenticator authenticator;

        public RequireServiceToken(IServiceAuthenticator authenticator)
        {
            this.authenticator = authenticator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = await authenticator.AuthenticateAsync();
            var viewModelsRequireToken = context.ActionArguments
                .Select(a => a.Value as IUserManagementAction)
                .Where(a => a != null);

            foreach (var vm in viewModelsRequireToken)
            {
                vm.ServiceToken = token;
            }

            await next();
        }
    }
}
