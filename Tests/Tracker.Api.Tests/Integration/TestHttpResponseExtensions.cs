using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tracker.Api.Tests.Integration
{
    public static class TestHttpResponseExtensions
    {
        public static async Task<T> GetContent<T>(this HttpRequestMessage response)
        {
            var strContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(strContent);
        }
    }
}
