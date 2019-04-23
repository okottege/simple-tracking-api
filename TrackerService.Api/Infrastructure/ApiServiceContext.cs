using System;
using Microsoft.AspNetCore.Http;
using TrackerService.Common.Contracts;

namespace TrackerService.Api.Infrastructure
{
    public class ApiServiceContext : IServiceContext
    {
        private readonly HttpContext context;

        public ApiServiceContext(IHttpContextAccessor httpAccessor)
        {
            context = httpAccessor.HttpContext;
        }

        public string RequestId
        {
            get
            {
                if (context.Items.TryGetValue(CustomHeaders.RequestId, out var requestId))
                {
                    return requestId.ToString();
                }

                throw new Exception($"Cannot find the {CustomHeaders.RequestId} header.");
            }
        }
    }
}
