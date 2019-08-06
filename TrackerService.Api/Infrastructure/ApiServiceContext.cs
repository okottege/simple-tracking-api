using System;
using Microsoft.AspNetCore.Http;
using TrackerService.Core.CoreDomain;

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
                if (httpAccessor.HttpContext.Request.Headers.TryGetValue(CustomHeaders.RequestId, out var requestId))
                {
                    return requestId.ToString();
                }

                return null;
            }
        }

        public string TenantId => httpAccessor.HttpContext.Request.Headers[CustomHeaders.TenantId];
    }
}
