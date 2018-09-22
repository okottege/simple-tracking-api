using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tracker.Api.IntegrationTests.Configuration
{
    public class TestConfiguration
    {
        public TestConfiguration(TestContext context)
        {
            WebApiUrl = context.Properties["webUrl"].ToString();
            Authentication = new AuthenticationInfo(context.Properties);
        }

        public string WebApiUrl { get; }
        public AuthenticationInfo Authentication { get; }
    }
}
