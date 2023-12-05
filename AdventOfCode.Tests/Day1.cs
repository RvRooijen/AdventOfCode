using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Tests;

public class Day1
{
	[Test]
	public void PartOne()
	{
		int answer = 54953;
		
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

		Assert.That(answer, Is.EqualTo(sum));
	}
	
	Dictionary<string, int> numberToValue = new Dictionary<string, int>
	{
		{"one", 1},
		{"two", 2},
		{"three", 3},
		{"four", 4},
		{"five", 5},
		{"six", 6},
		{"seven", 7},
		{"eight", 8},
		{"nine", 9}
	};
	
	[Test]
	public void PartTwo()
	{
		int answer = 53868;
		
		var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day1\\Input.txt");
		var lines = content.Split('\n').ToList();

		int sum = 0;
		foreach (string line in lines)
		{
			// .NET doesn't support the possessive quantifier :( d{1}+
			// So we need to limit the captured numerics to 1 digit ourselves
			Regex regex = new Regex(@"(?=(\d+|one|two|three|four|five|six|seven|eight|nine))");
			MatchCollection matches = regex.Matches(line);
			if(matches.Count == 0) continue;
			var first = matches[0].Groups[1].Value;
			var last = matches[^1].Groups[1].Value;

			int a = StringToInt(first);
			int b = StringToInt(last);

			string fullNumber = a.ToString() + b.ToString();
			
			sum += int.Parse(fullNumber);
		}
		
		Assert.That(answer, Is.EqualTo(sum));
	}
	
	int StringToInt(string number)
	{
		if (int.TryParse(number, out int result))
		{
			// Only return the first digit because of regex limitations in .net
			if (number.Length > 1)
				return int.Parse(number[0].ToString());

			return result;
		}

		return numberToValue[number];
	}
}