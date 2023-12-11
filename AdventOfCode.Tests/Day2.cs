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
	public void Part1()
	{
		int answer = 2265;
		
		var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day2\\Input.txt");
		var games = content.Split('\n').ToList();

		var sum = games.Select(GetGameInfo)
			.Where(tuple => tuple.IsPossible)
			.Sum(tuple => tuple.Id);
		
		Assert.That(answer, Is.EqualTo(sum));
	}

	private class ColorValue
	{
		public Colors Color;
		public int Number;

		public ColorValue(Colors color, int number)
		{
			Color = color;
			Number = number;
		}
	}
	
	private class GameInfo
	{
		public bool IsPossible;
		public int Id;
		public List<ColorValue> ColorValues;

		public GameInfo(bool isPossible, int id, List<ColorValue> colorValues)
		{
			IsPossible = isPossible;
			Id = id;
			ColorValues = colorValues;
		}
	}
	
	GameInfo GetGameInfo(string game)
	{
		var splitted = game.Split(new[] { ",", ";", ":" }, StringSplitOptions.TrimEntries);
		var id = int.Parse(numbersRegex.Match(splitted[0]).Value);

		var colors = splitted
			.Skip(1)
			.Select(GetColor)
			.Select(tuple => new ColorValue(tuple.c, tuple.n))
			.ToList();

		var possible = colors
			.TrueForAll(tuple => tuple.Number <= maxColors[tuple.Color]);

		return new GameInfo(possible, id, colors);
	}

	[Test]
	public void Part2()
	{
		int answer = 2265;
		
		var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day2\\Input.txt");
		var games = content.Split('\n').ToList();

		var gameInfo = games
			.Select(GetGameInfo);

		int sum = 0;
		foreach (GameInfo info in gameInfo)
		{
			int power = 0;
			var by = info.ColorValues.GroupBy(value => value.Color);
			foreach (IGrouping<Colors,ColorValue> grouping in by)
			{
				var max = grouping.OrderByDescending(value => value.Number).First().Number;
				if (power == 0)
				{
					power = max;
					continue;
				}

				power *= max;
			}

			sum += power;
		}
		
		Console.WriteLine(sum);
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