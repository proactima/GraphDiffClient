using System.Collections.Generic;
using Newtonsoft.Json;

namespace GraphDiffClient.Models
{
	public class GroupGraphResponse : BaseGraphResponse
	{
		[JsonProperty(PropertyName = "value")]
		public IEnumerable<Group> Groups { get; set; }
	}
}