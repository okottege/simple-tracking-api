using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tracker.Api.IntegrationTests.Logging
{
    public class TestTraceTraceLogger : ITestTraceLogger
    {
        private readonly TestContext context;

        public TestTraceTraceLogger(TestContext context)
        {
            this.context = context;
        }

        public void WriteLine(string message)
        {
            context.WriteLine(message);
        }
    }
}
