using System;
using Newtonsoft.Json;

namespace GraphDiffClient.Models
{
	public class Group
	{
		[JsonProperty(PropertyName = "objectType")]
		public string ObjectType { get; set; }

		[JsonProperty(PropertyName = "objectId")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "deletionTimestamp", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime DeletionTimestamp { get; set; }

		[JsonProperty(PropertyName = "displayName")]
		public string DisplayName { get; set; }

		[JsonProperty(PropertyName = "mailNickname")]
		public string MailNickname { get; set; }

		[JsonProperty(PropertyName = "mailEnabled")]
		public bool MailEnabled { get; set; }

		[JsonProperty(PropertyName = "securityEnabled")]
		public bool SecurityEnabled { get; set; }
	}
}