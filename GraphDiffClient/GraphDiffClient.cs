using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraphDiffClient.Models;
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
			_httpClient = new HttpClient();
			_tenantId = tenantId;
			_tokenRetriever = tokenRetriever;
		}

		public async Task<GraphResponse> GetUsersAsync(List<string> select = null)
		{
			if (string.IsNullOrEmpty(_accessToken))
				_accessToken = await _tokenRetriever().ConfigureAwait(false);

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

			var queryParams = GenerateQueryParams(select);
			var requestUri =
				new Uri(string.Format("https://graph.windows.net/{0}/users", _tenantId)).AddQueryParameters(queryParams);
			var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));

			var result = await _httpClient.SendAsync(request).ConfigureAwait(false);
			var stringResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			var data = JsonConvert.DeserializeObject<GraphResponse>(stringResult);

			return data;
		}

		private static Dictionary<string, string> GenerateQueryParams(List<string> selectList)
		{
			var selectFilter = string.Empty;
			if (selectList != null && selectList.Any())
				selectFilter = selectList.Aggregate((s1, s2) => s1 + "," + s2);

			var queryParams = new Dictionary<string, string>
			{
				{"api-version", "1.5"},
				{"deltaLink", ""},
			};

			if (selectList != null && selectList.Any())
				queryParams["$select"] = selectFilter;

			return queryParams;
		}
	}
}