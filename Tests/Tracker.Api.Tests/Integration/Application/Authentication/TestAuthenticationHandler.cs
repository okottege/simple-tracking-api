using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tracker.Api.Tests.Integration.Application.Authentication
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authTicket = new AuthenticationTicket(new ClaimsPrincipal(Options.Identity), new AuthenticationProperties(), TestAuthenticationExtensions.TEST_SCHEME);
            return Task.FromResult(AuthenticateResult.Success(authTicket));
        }
    }
}
