using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Nacos.Extensions.DiscoveryHandler
{
    public class PollyContextInjectingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var context = new Context();
            request.SetPolicyExecutionContext(context);
            return base.SendAsync(request, cancellationToken);
        }
    }
}