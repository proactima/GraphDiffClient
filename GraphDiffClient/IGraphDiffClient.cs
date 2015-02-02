using System.Threading.Tasks;
using GraphDiffClient.Models;

namespace GraphDiffClient
{
    public interface IGraphDiffClient
    {
        Task<DiffResponse> GetObjectsAsync();
        Task<DiffResponse> GetNextPageAsync();
    }
}