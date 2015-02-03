using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Proactima.GraphDiff.Models
{
	public class DiffResponse
	{
		public DiffResponse()
		{
			Users = new List<User>();
			Groups = new List<Group>();
			DirectoryLinkChanges = new List<DirectoryLinkChange>();
		}

		[JsonProperty(PropertyName = "odata.metadata")]
		public string Metadata { get; set; }

		[JsonProperty(PropertyName = "aad.deltaLink")]
		private Uri DeltaLink { get; set; }

		public string DeltaToken
		{
			get
			{
				return DeltaLink == null
					? string.Empty
					: DeltaLink.ExtractNamedQueryParameter("deltaLink", false);
			}
		}

		[JsonProperty(PropertyName = "aad.nextPage")]
		public string NextPage { get; set; }

		[JsonIgnore]
		public bool HasMorePages
		{
			get { return !string.IsNullOrEmpty(NextPage); }
		}

		public List<User> Users { get; set; }
		public List<Group> Groups { get; set; }
		public List<DirectoryLinkChange> DirectoryLinkChanges { get; set; }
	}
}