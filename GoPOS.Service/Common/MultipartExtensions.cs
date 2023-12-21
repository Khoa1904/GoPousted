using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Content;
using System;

namespace GoPOS.Service.Common
{
    internal static class MultipartExtensions
    {
        internal static Task<IFlurlResponse> PatchMultipartAsync(this IFlurlRequest request, Action<CapturedMultipartContent> buildContent, CancellationToken cancellationToken = default(CancellationToken))
        {
            var cmc = new CapturedMultipartContent(request.Settings);
            buildContent(cmc);
            return request.SendAsync(new HttpMethod("PATCH"), cmc, cancellationToken);
        }
    }
}
