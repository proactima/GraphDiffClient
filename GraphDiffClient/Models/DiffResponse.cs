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
		internal Uri DeltaLink { get; set; }

        [JsonProperty(PropertyName = "aad.nextLink")]
        internal Uri NextLink { get; set; }

        [JsonIgnore]
        public string DeltaToken { get; set; }

		[JsonIgnore]
		public bool HasMorePages
		{
            get { return NextLink != null; }
		}

		public List<User> Users { get; set; }
		public List<Group> Groups { get; set; }
		public List<DirectoryLinkChange> DirectoryLinkChanges { get; set; }
	}
}