using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConnectingApps.HttpClientDemo
{
    class Program
    {
        static async Task Main()
        {
            using (var httpClient = new HttpClient(new SafeHandler()))
            {
                httpClient.BaseAddress = new Uri("https://www.google.com/");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "DummyToken");
                var response = await httpClient.GetAsync("");
                Console.WriteLine($"Returned status code: {response.StatusCode}");
                Console.WriteLine("END");
            }
        }
    }
}
