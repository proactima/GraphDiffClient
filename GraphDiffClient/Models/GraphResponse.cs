namespace Proactima.GraphDiff.Models
{
    public class GraphResponse
    {
        public static GraphResponse Create(DiffResponse data)
        {
            return new GraphResponse {Data = data};
        }

        public static GraphResponse CreateFailedResponse(string message)
        {
            return new GraphResponse {HasError = true, ErrorMessage = message};
        }

        public bool HasError { get; private set; }
        public DiffResponse Data { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}
