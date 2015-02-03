using System.Collections.Generic;

namespace GraphDiffClient
{
	internal static class DiffHelpers
	{
		internal static Dictionary<string, string> GenerateQueryParams()
		{
			var queryParams = new Dictionary<string, string>
			{
				{"api-version", "1.5"},
				{"deltaLink", ""},
			};

			return queryParams;
		}
	}
}