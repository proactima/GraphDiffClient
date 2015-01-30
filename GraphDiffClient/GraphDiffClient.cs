using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GraphDiffClient
{
	public class GraphDiffClient
	{
		private readonly Uri _baseUrl = new Uri("https://graph.windows.net/");
		private readonly HttpClient _httpClient;
		private readonly string _tenantId;
		private readonly Func<Task<string>> _tokenRetriever;
		private string accessToken;

		public GraphDiffClient(Func<Task<string>> tokenRetriever, string tenantId)
		{
			_httpClient = new HttpClient(); // {BaseAddress = _baseUrl};
			_tenantId = tenantId;
			_tokenRetriever = tokenRetriever;
		}

		public async Task<GraphResponse> GetUsersAsync()
		{
			if (string.IsNullOrEmpty(accessToken))
				accessToken = await _tokenRetriever().ConfigureAwait(false);

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

	public class DiffRequest
	{
		private string _apiVersion = "1.5";

		public string ApiVersion
		{
			get { return _apiVersion; }
			set { _apiVersion = value; }
		}

		public string ResourceSet { get; set; }
		public string DeltaLink { get; set; }
	}

	public class GraphResponse
	{
		[JsonProperty(PropertyName = "odata.metadata")]
		public string Metadata { get; set; }

		[JsonProperty(PropertyName = "aad.deltaLink")]
		private Uri DeltaLink { get; set; }

		public string DeltaToken
		{
			get
			{
				if (DeltaLink == null)
					return string.Empty;

				return DeltaLink.ExtractQueryParams().FirstOrDefault().Value;
			}
		}

		[JsonProperty(PropertyName = "aad.nextPage")]
		public string NextPage { get; set; }

		[JsonProperty(PropertyName = "value")]
		public IEnumerable<User> Users { get; set; }

		[JsonIgnore]
		public bool HasMorePages
		{
			get { return !string.IsNullOrEmpty(NextPage); }
		}
	}

	public class User
	{
		[JsonProperty(PropertyName = "objectType")]
		public string ObjectType { get; set; }

		[JsonProperty(PropertyName = "objectId")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "displayName")]
		public string DisplayName { get; set; }

		[JsonProperty(PropertyName = "givenName")]
		public string GivenName { get; set; }

		[JsonProperty(PropertyName = "surname")]
		public string Surname { get; set; }

		[JsonProperty(PropertyName = "userPrincipalName")]
		public string Upn { get; set; }

		[JsonProperty(PropertyName = "otherMails")]
		public List<string> OtherMails { get; set; }

		[JsonProperty(PropertyName = "userType")]
		public string UserType { get; set; }

		[JsonProperty(PropertyName = "accountEnabled")]
		public bool AccountEnabled { get; set; }

		[JsonProperty(PropertyName = "deletionTimestamp", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime DeletionTimestamp { get; set; }

		[DefaultValue(false)]
		[JsonProperty(PropertyName = "aad.isDeleted", NullValueHandling = NullValueHandling.Ignore)]
		public bool IsDeleted { get; set; }

		[JsonProperty(PropertyName = "aad.originalUserPrincipalName", NullValueHandling = NullValueHandling.Ignore)]
		public string OriginalUpn { get; set; }
	}
}