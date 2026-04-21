using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.App_Start
{
    public class RequestBodyBufferingHandler : DelegatingHandler
    {
        private const string BodyKey = "RequestBodyBuffer";

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content != null)
            {
                await request.Content.LoadIntoBufferAsync();
                var body = await request.Content.ReadAsStringAsync();

                // Store it for later retrieval in BaseController
                request.Properties[BodyKey] = body;
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}