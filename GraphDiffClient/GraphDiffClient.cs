using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Proactima.GraphDiff.Models;

namespace Proactima.GraphDiff
{
    public class GraphDiffClient : IGraphDiffClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _tenantId;
        private readonly Func<Task<string>> _tokenRetriever;
        private string _accessToken;

        public GraphDiffClient(Func<Task<string>> tokenRetriever, string tenantId)
        {
            _httpClient = new HttpClient();
            _tenantId = tenantId;
            _tokenRetriever = tokenRetriever;
        }

        public async Task<DiffResponse> GetObjectsAsync(string deltaLink = "")
        {
            var request = new DiffRequest {DeltaLink = deltaLink};
            var queryParams = DiffHelpers.GenerateQueryParams(request);
            var requestUri =
                new Uri(string.Format("https://graph.windows.net/{0}/directoryObjects", _tenantId)).AddQueryParameters(
                    queryParams);

            var data = await CallAdAsync(requestUri).ConfigureAwait(false);
            return data;
        }

        private async Task<DiffResponse> CallAdAsync(Uri requestUri)
        {
            if (string.IsNullOrEmpty(_accessToken))
                _accessToken = await _tokenRetriever().ConfigureAwait(false);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            var result = await ExecuteHttpRequestAsync(requestUri).ConfigureAwait(false);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                _accessToken = await _tokenRetriever().ConfigureAwait(false);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                result = await ExecuteHttpRequestAsync(requestUri).ConfigureAwait(false);
            }

            var data = await DiffHelpers.ParseResponseAsync(result).ConfigureAwait(false);

            return data;
        }

        private async Task<HttpResponseMessage> ExecuteHttpRequestAsync(Uri requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));

            var result = await _httpClient.SendAsync(request).ConfigureAwait(false);
            return result;
        }
    }
}