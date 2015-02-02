using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraphDiffClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphDiffClient
{
	public class GraphDiffClient
	{
		private readonly HttpClient _httpClient;
		private readonly string _tenantId;
		private readonly Func<Task<string>> _tokenRetriever;
		private string _accessToken;
	    private string _nextLink;

		public GraphDiffClient(Func<Task<string>> tokenRetriever, string tenantId)
		{
			_httpClient = new HttpClient();
			_tenantId = tenantId;
			_tokenRetriever = tokenRetriever;
		}

	    public async Task<DiffResponse> GetObjectsAsync()
	    {
            if (string.IsNullOrEmpty(_accessToken))
                _accessToken = await _tokenRetriever().ConfigureAwait(false);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var queryParams = GenerateQueryParams(null);
            var requestUri = new Uri(string.Format("https://graph.windows.net/{0}/directoryObjects", _tenantId)).AddQueryParameters(queryParams);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));

            var result = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var stringResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            var data = JsonConvert.DeserializeObject<DiffResponse>(stringResult);

	        var asJobject = JObject.Parse(stringResult);
	        foreach (var change in asJobject["value"])
	        {
                switch (change["odata.type"].ToString())
	            {
                    case "Microsoft.DirectoryServices.User":
                        data.Users.Add(change.ToObject<User>());
	                    break;
                    case "Microsoft.DirectoryServices.Group":
                        data.Groups.Add(change.ToObject<Group>());
	                    break;
                    case "Microsoft.DirectoryServices.Contact":
	                    break;
                    case "Microsoft.DirectoryServices.DirectoryLinkChange":
                        data.DirectoryLinkChanges.Add(change.ToObject<DirectoryLinkChange>());
	                    break;
	            }
	        }

	        if (data.HasMorePages)
	            _nextLink = data.NextPage;

            return data;
	    }

		private static Dictionary<string, string> GenerateQueryParams(List<string> selectList)
		{
			var queryParams = new Dictionary<string, string>
			{
				{"api-version", "1.5"},
				{"deltaLink", ""},
			};

			if (selectList == null || !selectList.Any()) return queryParams;

			var selectFilter = selectList.Aggregate((s1, s2) => s1 + "," + s2);
			queryParams["$select"] = selectFilter;

			return queryParams;
		}
	}
}