namespace AdventOfCode.Tests;

public class Day3
{
    private char[][] schematic;
    
    [Test]
    public void Test()
    {
        int answer = 539590;
        
        var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day3\\Input.txt");
        
        var lines = content.Split('\n').ToList();
        schematic = new char[lines.Count][];
        for (int i = 0; i < lines.Count; i++)
        {
            schematic[i] = lines[i].ToCharArray();
        }

        // Get coordinates of all numbers
        var numbers = new List<(int startX, int startY, int count, int number)>();
        for (int y = 0; y < schematic.Length; y++)
        {
            for (int x = 0; x < schematic[y].Length; x++)
            {
                if (char.IsDigit(schematic[y][x]))
                {
                    int count = 1;
                    while (x + count < schematic[y].Length && char.IsDigit(schematic[y][x + count]))
                    {
                        count++;
                    }

                    numbers.Add((x, y, count, int.Parse(new string(schematic[y][x..(x + count)]))));
                    x += count;
                }
            }
        }

        int sum = 0;
        foreach ((int startX, int startY, int count, int number) number in numbers)
        {
            var adjacent = GetAdjacent(number.startX, number.startY, number.count);
            if (adjacent.Any(c => c == '&' || c == '/' || c == '=' || c == '*' || c == '+' || c == '-' || c == '@' || c == '#' || c == '$' || c == '%'))
            {
                sum += number.number;
            }
        }
        
        Console.WriteLine(sum);
        
        Assert.That(answer, Is.EqualTo(sum));
    }

    List<char> GetAdjacent(int x, int y, int size)
    {
        var adjacent = new List<char>();
        for (int y1 = -1; y1 <= 1; y1++)
        {
            for (int x1 = -1; x1 <= size; x1++)
            {
                if (x1 == 0 && y1 == 0) continue;
                if (x + x1 < 0 || x + x1 >= schematic.Length) continue;
                if (y + y1 < 0 || y + y1 >= schematic[y].Length) continue;
                adjacent.Add(schematic[y + y1][x + x1]);
            }
        }

        return adjacent;
    }
}