using System.Threading.Tasks;
using Proactima.GraphDiff.Models;

namespace Proactima.GraphDiff
{
	public interface IGraphDiffClient
	{
		Task<GraphResponse> GetObjectsAsync(string deltaLink = "");
	}
}