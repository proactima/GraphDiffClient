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
            var actual = DiffHelpers.GenerateQueryParams(null);

            // t
            actual.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void ItGeneratesSelectFilterParams()
        {
            // g
            var filter = new List<string>
            {
                "filterA",
                "filterB"
            };

            // w
            var actual = DiffHelpers.GenerateQueryParams(filter);

            // t
            actual["$select"].Should().Be("filterA,filterB");
        }
    }
}