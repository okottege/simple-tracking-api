namespace Tracker.Api.IntegrationTests.Logging
{
    public interface ITestTraceLogger
    {
        void WriteLine(string message);
    }
}