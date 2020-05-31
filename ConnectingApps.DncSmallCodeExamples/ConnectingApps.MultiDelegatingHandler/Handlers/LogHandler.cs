using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ConnectingApps.MultiDelegatingHandler.Handlers
{
    public class LogHandler : DelegatingHandler
    {
        private readonly ILogger<LogHandler> _logger;

        public LogHandler(ILogger<LogHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            _logger.LogInformation("{response}", response);
            return response;
        }
    }
}
