using System.Text.RegularExpressions;

namespace AdventOfCode.Tests;

public class Day2
{
	enum Colors
	{
		Red,
		Green,
		Blue
	}

	Dictionary<Colors, int> maxColors = new Dictionary<Colors, int> { {Colors.Red, 12}, {Colors.Green, 13}, {Colors.Blue, 14} };
	Regex numbersRegex = new Regex(@"(-?\d+)");
	
	[Test]
	public void Test()
	{
		int answer = 2265;
		
		var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day2\\Input.txt");
		var games = content.Split('\n').ToList();

		var sum = games.Select(IsGamePossible)
			.Where(tuple => tuple.isPossible)
			.Sum(tuple => tuple.id);
		
		Assert.That(answer, Is.EqualTo(sum));
	}

	(bool isPossible, int id) IsGamePossible(string game)
	{
		var splitted = game.Split(new[] { ",", ";", ":" }, StringSplitOptions.TrimEntries);
		var id = int.Parse(numbersRegex.Match(splitted[0]).Value);
		
		var possible = splitted
			.Skip(1)
			.Select(GetColor)
			.ToList()
			.TrueForAll(tuple => tuple.n <= maxColors[tuple.c]);
		
		return (possible, id);
	}
	
	(Colors c, int n) GetColor(string input) =>
		input switch
		{
			_ when input.Contains("red") => (Colors.Red, GetNumber(input)),
			_ when input.Contains("green") => (Colors.Green, GetNumber(input)),
			_ when input.Contains("blue") => (Colors.Blue, GetNumber(input)),
			_ => throw new Exception("Unknown color")
		};

	int GetNumber(string input)
	{
		var match = numbersRegex.Match(input);
		return int.Parse(match.Value);
	}
}