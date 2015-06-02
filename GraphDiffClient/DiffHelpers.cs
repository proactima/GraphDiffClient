using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proactima.GraphDiff.Models;

namespace Proactima.GraphDiff
{
    internal static class DiffHelpers
    {
        internal static Dictionary<string, string> GenerateQueryParams(DiffRequest request)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"api-version", request.ApiVersion},
                {"deltaLink", request.DeltaLink}
            };

            return queryParams;
        }

        internal static string GetDeltaToken(DiffResponse data)
        {
            if (data.NextLink != null)
            {
                var delta = data.NextLink.ExtractNamedQueryParameter("deltaLink", false);
                return delta;
            }

            if(data.DeltaLink != null)
            {
                var delta = data.DeltaLink.ExtractNamedQueryParameter("deltaLink", false);
                return delta;
            }

            return string.Empty;
        }

        internal static async Task<DiffResponse> ParseResponseAsync(HttpResponseMessage result, Action<string, string> infoLogger)
        {
            var stringResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var data = JsonConvert.DeserializeObject<DiffResponse>(stringResult);
            var asJobject = JObject.Parse(stringResult);

            data.DeltaToken = GetDeltaToken(data);
            infoLogger("ParseResponseAsync", $"Got DeltaToken {data.DeltaToken}");

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
            return data;
        }
    }
}