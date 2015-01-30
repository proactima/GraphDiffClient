using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GraphDiffClient.Sample
{
	public class GraphService
	{
		private const string ClientId = "";
		private const string Secret = "";
		private const string TenantId = "";

		public GraphService()
		{
		}

		public async Task GetUsers()
		{
			var client = new GraphDiffClient(AquireTokenForApplicationAsync, TenantId);
			var result = await client.GetUsersAsync().ConfigureAwait(false);

			foreach (var user in result.Users)
			{
				Console.WriteLine("UserObjectId: {0}  UPN: {1}  Name: {2}  E-Mail: {3}", user.Id, user.Upn,
					user.DisplayName, user.OtherMails.FirstOrDefault());
			}
		}

		private static async Task<string> AquireTokenForApplicationAsync()
		{
			var authContext = new AuthenticationContext(string.Format("https://login.windows.net/{0}", TenantId));
			var credential = new ClientCredential(ClientId, Secret);

			var result =
				await authContext.AcquireTokenAsync("https://graph.windows.net", credential).ConfigureAwait(false);

			return result.AccessToken;
		}
	}
}