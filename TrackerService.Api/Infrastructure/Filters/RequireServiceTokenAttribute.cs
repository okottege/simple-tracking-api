using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TrackerService.Api.Infrastructure.Filters
{
    public class RequireServiceTokenAttribute : Attribute, IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }
}
