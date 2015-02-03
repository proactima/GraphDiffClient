using System.Collections.Generic;
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
				{"deltaLink", request.DeltaLink},
			};

			return queryParams;
		}
	}
}