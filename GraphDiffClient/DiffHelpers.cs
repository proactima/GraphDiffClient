using System.Collections.Generic;
using System.Linq;

namespace GraphDiffClient
{
    internal static class DiffHelpers
    {
        internal static Dictionary<string, string> GenerateQueryParams(List<string> selectList)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"api-version", "1.5"},
                {"deltaLink", ""},
            };

            if (selectList == null || !selectList.Any()) return queryParams;

            var selectFilter = selectList.Aggregate((s1, s2) => s1 + "," + s2);
            queryParams["$select"] = selectFilter;

            return queryParams;
        }
    }
}