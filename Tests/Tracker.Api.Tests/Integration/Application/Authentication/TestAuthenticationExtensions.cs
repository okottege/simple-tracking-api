using System;
using Microsoft.AspNetCore.Authentication;

namespace Tracker.Api.Tests.Integration.Application.Authentication
{
    public static class TestAuthenticationExtensions
    {
        public const string TEST_SCHEME = "Test Scheme";

        public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(TEST_SCHEME, "Test Auth", configureOptions);
        }
    }
}
