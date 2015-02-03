using System;

namespace Proactima.GraphDiff.Sample
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var graph = new GraphService();

			graph.GetUsers().Wait();
			Console.ReadLine();
		}
	}
}