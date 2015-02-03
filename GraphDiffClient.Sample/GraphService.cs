using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Proactima.GraphDiff.Sample
{
	public class GraphService
	{
		private readonly string _clientId = "";
		private readonly string _secret = "";
		private readonly string _tenantId = "";

		public GraphService()
		{
			_clientId = ConfigurationManager.AppSettings["ClientId"];
			_secret = ConfigurationManager.AppSettings["Secret"];
			_tenantId = ConfigurationManager.AppSettings["TenantId"];
		}

		public async Task GetUsers()
		{
			var client = new GraphDiffClient(AquireTokenForApplicationAsync, _tenantId);
			var result = await client.GetObjectsAsync().ConfigureAwait(false);

			foreach (var user in result.Users)
			{
				Console.WriteLine("UserObjectId: {0}  UPN: {1}  Name: {2}  E-Mail: {3}", user.Id, user.Upn,
					user.DisplayName, user.OtherMails.FirstOrDefault());
			}
		}

		private async Task<string> AquireTokenForApplicationAsync()
		{
			var authContext = new AuthenticationContext(string.Format("https://login.windows.net/{0}", _tenantId));
			var credential = new ClientCredential(_clientId, _secret);

			var result =
				await authContext.AcquireTokenAsync("https://graph.windows.net", credential).ConfigureAwait(false);

			return result.AccessToken;
		}
	}
}