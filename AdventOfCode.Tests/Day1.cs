using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Tests;

public class Tests
{
	[SetUp]
	public void Setup()
	{
		
	}

	[Test]
	public void Test()
	{
		var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day1\\Input.txt");
		var lines = content.Split('\n').ToList();

		int sum = 0;
		foreach (string line in lines)
		{
			Regex regex = new Regex(@"(-?\d+)");
			MatchCollection matches = regex.Matches(line);
			if(matches.Count == 0) continue;
			var first = matches[0].Value.Substring(0,1);
			var last = matches[^1].Value.Substring(matches[^1].Value.Length - 1);
			int amount = int.Parse(first + last);
			sum += amount;
		}
		
		Console.WriteLine(sum);
		
		Assert.Pass();
	}
}