using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GraphDiffClient
{
	public class GraphDiffClient
	{
		private readonly HttpClient _httpClient;
		private readonly string _tenantId;
		private readonly Func<Task<string>> _tokenRetriever;
		private string _accessToken;

		public GraphDiffClient(Func<Task<string>> tokenRetriever, string tenantId)
		{
			_httpClient = new HttpClient(); // {BaseAddress = _baseUrl};
			_tenantId = tenantId;
			_tokenRetriever = tokenRetriever;
		}

		public async Task<GraphResponse> GetUsersAsync()
		{
			if (string.IsNullOrEmpty(_accessToken))
				_accessToken = await _tokenRetriever().ConfigureAwait(false);

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

			var queryParams = GenerateQueryParams();
			var requestUri =
				new Uri(string.Format("https://graph.windows.net/{0}/users", _tenantId)).AddQueryParameters(queryParams);
			var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));

			var result = await _httpClient.SendAsync(request).ConfigureAwait(false);
			var stringResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			var jsonSettings = new JsonSerializerSettings {ContractResolver = new NullToEmptyListResolver()};

			var data = JsonConvert.DeserializeObject<GraphResponse>(stringResult, jsonSettings);

			return data;
		}

		private static Dictionary<string, string> GenerateQueryParams()
		{
			return new Dictionary<string, string>
			{
				{"api-version", "1.5"},
				{"deltaLink", ""}
			};
		}
	}
}