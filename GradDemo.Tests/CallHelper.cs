using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GradDemo.Tests
{
    public static class CallHelper
    {
        public static async Task<(T? content, HttpResponseMessage httpResponse)> GetAndDeserialize<T>(this HttpClient client, string path) where T : class
        {
            var result = await client.GetAsync(path);
            var resultContent = await ParseContent<T>(result.Content);
            return (resultContent, result);
        }

        public static async Task<(T? content, HttpResponseMessage httpResponse)> PostAndDeserialize<T>(this HttpClient client, string path, object input) where T : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(path, content);
            var resultContent = await ParseContent<T>(result.Content);
            return (resultContent, result);
        }

        private static async Task<T?> ParseContent<T>(HttpContent content) where T : class
        {
            try
            {
                var res = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(res);
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}
