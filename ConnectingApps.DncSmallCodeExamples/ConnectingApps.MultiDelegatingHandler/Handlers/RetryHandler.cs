using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectingApps.MultiDelegatingHandler.Handlers
{
    public class RetryHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage result = null;
            for (int i = 0; i < 3; i++)
            {
                result = await base.SendAsync(request, cancellationToken);
                if (result.StatusCode >= HttpStatusCode.InternalServerError)
                {
                    continue;
                }
                return result;
            }
            return result;
        }
    }
}
