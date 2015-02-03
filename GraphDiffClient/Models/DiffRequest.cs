namespace Proactima.GraphDiff.Models
{
	public class DiffRequest
	{
		private string _apiVersion = "1.5";
		private string _deltaLink = "";

		public string ApiVersion
		{
			get { return _apiVersion; }
			set { _apiVersion = value; }
		}

		public string DeltaLink
		{
			get { return _deltaLink; }
			set { _deltaLink = value; }
		}
	}
}