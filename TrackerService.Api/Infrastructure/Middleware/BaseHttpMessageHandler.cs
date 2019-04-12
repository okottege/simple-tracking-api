using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrackerService.Api.CustomExceptions;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public class BaseHttpMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
        {
            var response = await base.SendAsync(request, token);
            if (response.IsSuccessStatusCode) return response;

            throw new ServiceAccessException(response);
        }
    }
}
