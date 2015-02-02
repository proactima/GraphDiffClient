using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GraphDiffClient.Models
{
    public class DiffResponse
    {
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
    }
}