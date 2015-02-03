using System;
using Newtonsoft.Json;

namespace GraphDiffClient.Models
{
	public class DirectoryLinkChange
	{
		[JsonProperty(PropertyName = "objectType")]
		public string ObjectType { get; set; }

		[JsonProperty(PropertyName = "objectId")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "deletionTimestamp", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime DeletionTimestamp { get; set; }

		[JsonProperty(PropertyName = "associationType")]
		public string AssociationType { get; set; }

		[JsonProperty(PropertyName = "sourceObjectId")]
		public string SourceObjectId { get; set; }

		[JsonProperty(PropertyName = "sourceObjectType")]
		public string SourceObjectType { get; set; }

		[JsonProperty(PropertyName = "sourceObjectUri")]
		public string SourceObjectUri { get; set; }

		[JsonProperty(PropertyName = "targetObjectId")]
		public string TargetObjectId { get; set; }

		[JsonProperty(PropertyName = "targetObjectType")]
		public string TargetObjectType { get; set; }

		[JsonProperty(PropertyName = "targetObjectUri")]
		public string TargetObjectUri { get; set; }
	}
}