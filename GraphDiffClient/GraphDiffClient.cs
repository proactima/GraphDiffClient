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
        private readonly Action<string, string> _errorLogger;
        private readonly Action<string, string> _infoLogger; 

        public GraphDiffClient(Func<Task<string>> tokenRetriever, string tenantId, Action<string, string> errorLogger, Action<string, string> infoLogger)
        {
            _httpClient = new HttpClient();
            _tenantId = tenantId;
            _errorLogger = errorLogger ?? NullLogger;
            _infoLogger = infoLogger ?? NullLogger;
            _tokenRetriever = tokenRetriever;
        }

        public async Task<GraphResponse> GetObjectsAsync(string deltaLink = "")
        {
            try
            {
                var request = new DiffRequest { DeltaLink = deltaLink };
                var queryParams = DiffHelpers.GenerateQueryParams(request);
                var requestUri =
                    new Uri($"https://graph.windows.net/{_tenantId}/directoryObjects").AddQueryParameters(
                        queryParams);

                var data = await CallAdAsync(requestUri).ConfigureAwait(false);
                return data == null
                    ? GraphResponse.CreateFailedResponse("Something went wrong")
                    : GraphResponse.Create(data);
            }
            catch (Exception ex)
            {
                _errorLogger("Something went wrong", ex.ToString());
                return GraphResponse.CreateFailedResponse(ex.ToString());
            }
        }

        private async Task<DiffResponse> CallAdAsync(Uri requestUri)
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                _infoLogger("CallAdAsync", "Missing token - trying to retrieve...");
                _accessToken = await _tokenRetriever().ConfigureAwait(false);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            var result = await ExecuteHttpRequestAsync(requestUri).ConfigureAwait(false);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                _infoLogger("CallAdAsync", "Unauthorized, trying to get new token");
                _accessToken = await _tokenRetriever().ConfigureAwait(false);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                result = await ExecuteHttpRequestAsync(requestUri).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                    return null;
            }

            var data = await DiffHelpers.ParseResponseAsync(result, _infoLogger).ConfigureAwait(false);

            return data;
        }

        private async Task<HttpResponseMessage> ExecuteHttpRequestAsync(Uri requestUri)
        {
            _infoLogger("CallAdAsync", $"Calling Graph with following URI: {requestUri}");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));

            var result = await _httpClient.SendAsync(request).ConfigureAwait(false);
            return result;
        }

        private static void NullLogger(string step, string message)
        {
        }
    }
}