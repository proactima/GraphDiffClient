using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphDiffClient
{
	public static class UriExtensions
	{
		/// <summary>
		/// Adds the query parameters defined in the input dictionary to the Uri.
		/// This can be called several times to add more parameters
		/// </summary>
		/// <param name="uri">The URI to act on</param>
		/// <param name="parameters">Parameters to add</param>
		/// <returns>A new URI with the parameters added</returns>
		public static Uri AddQueryParameters(this Uri uri, Dictionary<string, string> parameters)
		{
			if (parameters.Count == 0)
				return uri;

			var existingQuery = uri.Query;

			var builder = new StringBuilder();
			parameters.Aggregate(builder,
				(sb, kvp) =>
				{
					if (existingQuery.Contains("?") || (builder.Length > 0 && builder[0].Equals('?')))
						return sb.AppendFormat("&{0}={1}", kvp.Key, Uri.EscapeDataString(kvp.Value));

					sb.Append('?');
					return sb.AppendFormat("{0}={1}", kvp.Key, Uri.EscapeDataString(kvp.Value));
				});

			if (builder[builder.Length - 1] == '&')
				builder.Remove(builder.Length - 1, 1);

			return new Uri(uri + builder.ToString());
		}

		/// <summary>
		/// Extracts all query parameters
		/// </summary>
		/// <param name="uri">The URI to act on</param>
		/// <returns>A dictionary of all query names/values</returns>
		public static Dictionary<string, string> ExtractQueryParams(this Uri uri)
		{
			var result = new Dictionary<string, string>();

			var query = uri.Query;
			if (string.IsNullOrEmpty(query))
				return result;

			var paramStart = query.IndexOf('?');
			query = query.Remove(paramStart, 1);

			var queryPairs = query.Split('&').ToList();
			queryPairs.ForEach(q =>
			{
				var singleQuery = q.Split('=').ToList();
				result.Add(singleQuery.First(), Uri.UnescapeDataString(singleQuery.Last()));
			});

			return result;
		}

		/// <summary>
		/// Extracts the value of a singe query parameter
		/// </summary>
		/// <param name="uri">The URI to act on</param>
		/// <param name="parameterName">The parameter to get the value for</param>
		/// <param name="ignoreCase">If true, ignores case on parameterName</param>
		/// <returns>The unescaped value, or null if not found</returns>
		public static string ExtractNamedQueryParameter(this Uri uri, string parameterName, bool ignoreCase = true)
		{
			if (string.IsNullOrEmpty(parameterName))
				throw new ArgumentNullException("parameterName");

			parameterName = ignoreCase
				? parameterName.Trim().ToLower()
				: parameterName.Trim();

			var queryParams = uri.ExtractQueryParams();

			return queryParams.ContainsKey(parameterName)
				? Uri.UnescapeDataString(queryParams[parameterName])
				: null;
		}
	}
}