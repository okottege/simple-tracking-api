using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrackerService.Data
{
    public static class ServiceAccessExtensions
    {
        public static async Task<T> GetContent<T>(this HttpResponseMessage response)
        {
            var strContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(strContent);
        }

        public static HttpContent GetJsonContent(this object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
