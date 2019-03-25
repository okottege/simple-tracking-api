using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrackerService.Common
{
    public static class HttpResponseExtensions
    {
        public static async Task<T> GetContent<T>(this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
