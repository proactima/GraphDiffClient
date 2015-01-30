using System.Collections.Generic;
using Newtonsoft.Json;

namespace GraphDiffClient.Models
{
	public class UserGraphResponse : BaseGraphResponse
	{
		[JsonProperty(PropertyName = "value")]
		public IEnumerable<User> Users { get; set; }
	}
}