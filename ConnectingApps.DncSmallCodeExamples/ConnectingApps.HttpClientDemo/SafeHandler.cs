using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConnectingApps.HttpClientDemo
{
    public class SafeHandler : DelegatingHandler
    {
        public SafeHandler()
        {
            InnerHandler = new HttpClientHandler();
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = base.SendAsync(request, cancellationToken);

            if (request.Headers.Authorization != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(request.Headers.Authorization.Scheme, "***");
            }

            var toLog = JsonConvert.SerializeObject(request);
            Console.WriteLine(toLog);
            return result;
        }
    }
}
