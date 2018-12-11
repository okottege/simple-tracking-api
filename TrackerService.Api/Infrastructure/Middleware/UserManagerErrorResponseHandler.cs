using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrackerService.Api.CustomExceptions;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public class UserManagerErrorResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new UserManagementException(response.StatusCode, responseContent, ex);
            }

            return response;
        }
    }
}
