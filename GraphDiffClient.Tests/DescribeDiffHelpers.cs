using System.Collections.Generic;
using FluentAssertions;
using Proactima.GraphDiff;
using Proactima.GraphDiff.Models;
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

			var request = new DiffRequest();

			// w
			var actual = DiffHelpers.GenerateQueryParams(request);

			// t
			actual.ShouldBeEquivalentTo(expected);
		}

		[Fact]
		public void ItGeneratesBasicParamsWithDeltaLink()
		{
			// g
			var expected = new Dictionary<string, string>
			{
				{"api-version", "1.5"},
				{"deltaLink", "1234"}
			};

			var request = new DiffRequest {DeltaLink = "1234"};

			// w
			var actual = DiffHelpers.GenerateQueryParams(request);

			// t
			actual.ShouldBeEquivalentTo(expected);
		}
	}
}