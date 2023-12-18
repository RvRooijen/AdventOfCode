using FluentAssertions;

namespace AdventOfCode.Tests;

public class Day4
{
    private class Card
    {
        private List<int> _numbers;

        public Card(List<int> numbers)
        {
            _numbers = numbers;
        }

        public int NumberOfMatches(List<int> extractNumbers)
        {
            return extractNumbers.Count(i => _numbers.Contains(i));
        }

        public int CalculateRewards(List<int> extractNumbers)
        {
            var numberOfMatches = NumberOfMatches(extractNumbers);
            int points = 0;
            if (numberOfMatches > 0)
            {
                points = (int) Math.Pow(2, numberOfMatches - 1);
            }
            return points;
        }
    }
    
    private List<int> ExtractNumbers(string s) => s
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToList();
    
    [Test]
    public void Test()
    {
        int answer = 21138;
        
        var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day4\\Input.txt");
        var lines = content.Split('\n').ToList();

        int points = 0;
        
        foreach (string line in lines)
        {
            var splitted = line.Split('|');
            
            var winningNumbers = splitted[0].Split(':')[1];
            var card = splitted[1];

            Card myCard = new Card(ExtractNumbers(card));
            points += myCard.CalculateRewards(ExtractNumbers(winningNumbers));
        }
        
        Assert.That(points, Is.EqualTo(answer));
    }

    [Test]
    public void Part2()
    {
        int answer = 0;
        
        var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day4\\Input.txt");
        var lines = content.Split('\n').ToList();

        Dictionary<int, int> earnedCards = new Dictionary<int, int>();
        int sumEarned = 0;
        // For each game
        foreach (string line in lines)
        {
            var splitted = line.Split('|');

            var winSide = splitted[0].Split(':');
            var id = int.Parse(winSide[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
            var winningNumbers = winSide[1];
            var card = splitted[1];

            Card myCard = new Card(ExtractNumbers(card));
            earnedCards.TryGetValue(id, out var cardAmount);
            var matches = myCard.NumberOfMatches(ExtractNumbers(winningNumbers));
            // For each earned card + 1 initial
            for (int c = 0; c < cardAmount+1; c++)
            {
                int start = id + 1;
                // For each point earned by this card
                for (int i = start; i < start + matches; i++)
                {
                    if (earnedCards.TryGetValue(i, out var earned))
                    {
                        earnedCards[i] = ++earned;
                    }
                    else
                    {
                        earnedCards.Add(i, 1);
                    }
                }
                
                sumEarned++;
            }
        }
        
        Console.WriteLine(sumEarned);
        Assert.Pass();
    }

    [Test]
    public void Example()
    {
        int answer = 30;
        
        var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day4\\ExampleInput.txt");
        var lines = content.Split('\n').ToList();

        Dictionary<int, int> earnedCards = new Dictionary<int, int>();
        int sumEarned = 0;
        // For each game
        foreach (string line in lines)
        {
            var splitted = line.Split('|');

            var winSide = splitted[0].Split(':');
            var id = int.Parse(winSide[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
            var winningNumbers = winSide[1];
            var card = splitted[1];

            Card myCard = new Card(ExtractNumbers(card));
            earnedCards.TryGetValue(id, out var cardAmount);
            var matches = myCard.NumberOfMatches(ExtractNumbers(winningNumbers));
            // For each earned card + 1 initial
            for (int c = 0; c < cardAmount+1; c++)
            {
                int start = id + 1;
                // For each point earned by this card
                for (int i = start; i < start + matches; i++)
                {
                    if (earnedCards.TryGetValue(i, out var earned))
                    {
                        earnedCards[i] = ++earned;
                    }
                    else
                    {
                        earnedCards.Add(i, 1);
                    }
                }
                
                sumEarned++;
            }
        }
        
        Console.WriteLine(sumEarned);
        sumEarned.Should().Be(30);
    }
}