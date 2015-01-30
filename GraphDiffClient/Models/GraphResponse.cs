using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GraphDiffClient.Models
{
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
}