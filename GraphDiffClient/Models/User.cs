﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GraphDiffClient.Models
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

		[JsonIgnore]
		public List<string> OtherMails
		{
			get { return _otherMails ?? new List<string>(); }
		}

		[JsonProperty(PropertyName = "otherMails")]
		private List<string> _otherMails { get; set; }

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