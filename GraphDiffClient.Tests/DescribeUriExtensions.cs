using System;
using System.Collections.Generic;
using FluentAssertions;
using Proactima.GraphDiff;
using Xunit;

namespace GraphDiffClient.Tests
{
	public class DescribeUriExtensions
	{
		[Fact]
		public void ItCanAddAQueryParameter()
		{
			// g
			var baseUri = new Uri("http://www.example.com/");

			// w
			var actual = baseUri.AddQueryParameters(new Dictionary<string, string> {{"query", "param"}});

			// t
			actual.ToString().Should().Be("http://www.example.com/?query=param");
		}

		[Fact]
		public void ItCanAddAQueryParameterMultipleTimes()
		{
			// g
			var baseUri = new Uri("http://www.example.com/");

			// w
			var temp = baseUri.AddQueryParameters(new Dictionary<string, string> {{"query", "param"}});
			var actual = temp.AddQueryParameters(new Dictionary<string, string> {{"foo", "bar"}});

			// t
			actual.ToString().Should().Be("http://www.example.com/?query=param&foo=bar");
		}

		[Fact]
		public void ItCanAddAQueryParameterWithoutValue()
		{
			// g
			var baseUri = new Uri("http://www.example.com/");

			// w
			var actual = baseUri.AddQueryParameters(new Dictionary<string, string> {{"query", ""}});

			// t
			actual.ToString().Should().Be("http://www.example.com/?query=");
		}

		[Fact]
		public void ItCanAddSeveralQueryParameters()
		{
			// g
			var baseUri = new Uri("http://www.example.com/");

			// w
			var actual = baseUri.AddQueryParameters(new Dictionary<string, string> {{"query", "param"}, {"second", "param"}});

			// t
			actual.ToString().Should().Be("http://www.example.com/?query=param&second=param");
		}

		[Fact]
		public void ItCanEscapeParameters()
		{
			// g
			var baseUri = new Uri("http://www.example.com/");

			// w
			var actual = baseUri.AddQueryParameters(new Dictionary<string, string> {{"query", "; DELETE * FROM Users"}});

			// t
			actual.ToString().Should().Be("http://www.example.com/?query=%3B DELETE * FROM Users");
		}

		[Fact]
		public void ItDoesNotDoAnythingWhenNotGivenParameters()
		{
			// g
			var baseUri = new Uri("http://www.example.com/");

			// w
			var actual = baseUri.AddQueryParameters(new Dictionary<string, string>());

			// t
			actual.ToString().Should().Be("http://www.example.com/");
		}

		[Fact]
		public void ItCanExtractQueryParams()
		{
			// g
			var uriUnderTest = new Uri("http://www.example.com/?query=param");

			// w
			var actual = uriUnderTest.ExtractQueryParams();

			// t
			actual.Should().ContainKey("query");
			actual["query"].Should().Be("param");
		}

		[Fact]
		public void ItCanExtractSeveralQueryParams()
		{
			// g
			var uriUnderTest = new Uri("http://www.example.com/?query=param&foo=bar");

			// w
			var actual = uriUnderTest.ExtractQueryParams();

			// t
			actual.Should().ContainKey("query");
			actual["query"].Should().Be("param");
			actual.Should().ContainKey("foo");
			actual["foo"].Should().Be("bar");
		}

		[Fact]
		public void ItCanExtractQueryParamsOnSubRoute()
		{
			// g
			var uriUnderTest = new Uri("http://www.example.com/users?query=param");

			// w
			var actual = uriUnderTest.ExtractQueryParams();

			// t
			actual.Should().ContainKey("query");
			actual["query"].Should().Be("param");
		}

		[Fact]
		public void ItReturnsNoQueryParamsWhenNoneGiven()
		{
			// g
			var uriUnderTest = new Uri("http://www.example.com/");

			// w
			var actual = uriUnderTest.ExtractQueryParams();

			// t
			actual.Count.Should().Be(0);
		}

		[Fact]
		public void ItReturnsDecodedQueryParams()
		{
			// g
			var uriUnderTest = new Uri("http://www.example.com/?query=%3B DELETE * FROM Users");

			// w
			var actual = uriUnderTest.ExtractQueryParams();

			// t
			actual.Should().ContainKey("query");
			actual["query"].Should().Be("; DELETE * FROM Users");
		}

		[Fact]
		public void ItBlowsUpWhenNoNameGivenToExtractNamed()
		{
			// g
			var uriUnderTest = new Uri("http://www.example.com/?query=param&foo=bar");

			// w

			// t
			Assert.Throws<ArgumentNullException>(() => uriUnderTest.ExtractNamedQueryParameter(""));
		}

		[Fact]
		public void ItCanExtractANamedParameter()
		{
			// g
			var uriUnderTest = new Uri("http://www.example.com/?query=param&foo=bar");

			// w
			var actual = uriUnderTest.ExtractNamedQueryParameter("query");

			// t
			actual.Should().Be("param");
		}

		[Fact]
		public void ItReturnsNullIfParameterDoesNotExist()
		{
			// g
			var uriUnderTest = new Uri("http://www.example.com/?query=param&foo=bar");

			// w
			var actual = uriUnderTest.ExtractNamedQueryParameter("name");

			// t
			actual.Should().BeNull();
		}

		[Fact]
		public void ItCanExtractANamedParameterWithoutValue()
		{
			// g
			var uriUnderTest = new Uri("http://www.example.com/?foo=");

			// w
			var actual = uriUnderTest.ExtractNamedQueryParameter("foo");

			// t
			actual.Should().Be("");
		}
	}
}