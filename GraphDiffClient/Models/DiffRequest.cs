namespace GraphDiffClient.Models
{
	public class DiffRequest
	{
		private string _apiVersion = "1.5";

		public string ApiVersion
		{
			get { return _apiVersion; }
			set { _apiVersion = value; }
		}

		public string ResourceSet { get; set; }
		public string DeltaLink { get; set; }
	}
}