using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace GraphDiffClient.Tests
{
    public class DescribeDiffHelpers
    {
        [Fact]
        public void ItGeneratesBasicParams()
        {
            // g
            var expected = new Dictionary<string, string>
            {
                {"api-version", "1.5"},
                {"deltaLink", ""}
            };

            // w
            var actual = DiffHelpers.GenerateQueryParams();

            // t
            actual.ShouldBeEquivalentTo(expected);
        }
    }
}