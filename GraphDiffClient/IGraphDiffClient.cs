using System.Threading.Tasks;
using Proactima.GraphDiff.Models;

namespace Proactima.GraphDiff
{
	public interface IGraphDiffClient
	{
		Task<DiffResponse> GetObjectsAsync(string deltaLink = "");
		Task<DiffResponse> GetNextPageAsync();
	}
}