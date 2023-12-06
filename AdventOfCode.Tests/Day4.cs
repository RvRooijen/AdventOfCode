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

        public int CalculateRewards(List<int> extractNumbers)
        {
            int numberOfMatches = extractNumbers.Count(i => _numbers.Contains(i));
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
}