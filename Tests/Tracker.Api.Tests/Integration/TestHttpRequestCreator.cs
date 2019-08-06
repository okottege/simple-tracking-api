using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Tracker.Api.Tests.Integration
{
    public static class TestHttpRequestCreator
    {
        const string JsonMediaType = "application/json";

        public static HttpContent ToJsonContent(this object payload) =>
            new StringContent(payload == null ? string.Empty : JsonConvert.SerializeObject(payload),
                Encoding.UTF8, JsonMediaType);
    }
}
