using System;
using Microsoft.AspNetCore.Http;
using TrackerService.Common.Contracts;

namespace TrackerService.Api.Infrastructure
{
    public class ApiServiceContext : IServiceContext
    {
        private readonly IHttpContextAccessor httpAccessor;

        public ApiServiceContext(IHttpContextAccessor httpAccessor)
        {
            this.httpAccessor = httpAccessor;
        }

        public string RequestId
        {
            get
            {
                if (httpAccessor.HttpContext.Items.TryGetValue(CustomHeaders.RequestId, out var requestId))
                {
                    return requestId.ToString();
                }

                throw new Exception($"Cannot find the {CustomHeaders.RequestId} header.");
            }
        }
    }
}
