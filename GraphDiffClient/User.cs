using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GraphDiffClient
{
	public class User
	{
		[JsonProperty(PropertyName = "objectType")]
		public string ObjectType { get; set; }

		[JsonProperty(PropertyName = "objectId")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "displayName")]
		public string DisplayName { get; set; }

		[JsonProperty(PropertyName = "givenName")]
		public string GivenName { get; set; }

		[JsonProperty(PropertyName = "surname")]
		public string Surname { get; set; }

		[JsonProperty(PropertyName = "userPrincipalName")]
		public string Upn { get; set; }

		[JsonProperty(PropertyName = "otherMails")]
		public List<string> OtherMails { get; set; }

		[JsonProperty(PropertyName = "userType")]
		public string UserType { get; set; }

		[JsonProperty(PropertyName = "accountEnabled")]
		public bool AccountEnabled { get; set; }

		[JsonProperty(PropertyName = "deletionTimestamp", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime DeletionTimestamp { get; set; }

		[DefaultValue(false)]
		[JsonProperty(PropertyName = "aad.isDeleted", NullValueHandling = NullValueHandling.Ignore)]
		public bool IsDeleted { get; set; }

		[JsonProperty(PropertyName = "aad.originalUserPrincipalName", NullValueHandling = NullValueHandling.Ignore)]
		public string OriginalUpn { get; set; }
	}
}