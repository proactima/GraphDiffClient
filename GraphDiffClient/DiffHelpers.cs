using System.Collections.Generic;
using GraphDiffClient.Models;

namespace GraphDiffClient
{
	internal static class DiffHelpers
	{
		internal static Dictionary<string, string> GenerateQueryParams(DiffRequest request)
		{
			var queryParams = new Dictionary<string, string>
			{
				{"api-version", request.ApiVersion},
				{"deltaLink", request.DeltaLink},
			};

			return queryParams;
		}
	}
}