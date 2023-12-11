namespace AdventOfCode.Tests;

public class Day3
{
    private class Schematic
    {
        private char[][] _grid;
        private List<(int startX, int startY, int count, int number)> _numbers;
        private List<(int x, int y)> _gears;

        public Schematic()
        {
            var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day3\\Input.txt");

            var lines = content.Split('\n').ToList();
            _grid = new char[lines.Count][];
            for (int i = 0; i < lines.Count; i++)
            {
                _grid[i] = lines[i].ToCharArray();
            }
            
            PopuplateNumbers();
            PopulateGears();
        }

        public int Calculate1()
        {
            int sum = 0;
            foreach ((int startX, int startY, int count, int number) number in _numbers)
            {
                var adjacent = GetAdjacent(number.startX, number.startY, number.count);
                if (adjacent.Any(c => c == '&' || c == '/' || c == '=' || c == '*' || c == '+' || c == '-' || c == '@' || c == '#' || c == '$' || c == '%'))
                {
                    sum += number.number;
                }
            }

            return sum;
        }

        public int Calculate2()
        {
            int sum = 0;
            foreach ((int x, int y) gear in _gears)
            { 
                var adjacent = GetAdjacent(gear.x, gear.y);
                
                // Check all numbers for each gear
                List<int> matchedNumber = new List<int>();
                foreach ((int startX, int startY, int count, int number) number in _numbers)
                {
                    // Does any part of the number overlap with the adjacent of the gear?
                    int startX = number.startX;
                    int endX = number.startX + number.count;
                    int y = number.startY;

                    // Move over x to check if it matches adjacent
                    for (int x = startX; x < endX; x++)
                    {
                        if (adjacent.Exists(tuple => tuple.x == x && tuple.y == y))
                        {
                            matchedNumber.Add(number.number);
                            break;
                        }
                    }
                }

                if (matchedNumber.Count == 2)
                {
                    sum += matchedNumber[0] * matchedNumber[1];
                }
            }

            return sum;
        }

        private void PopuplateNumbers()
        {
            _numbers = new List<(int startX, int startY, int count, int number)>();
            for (int y = 0; y < _grid.Length; y++)
            {
                for (int x = 0; x < _grid[y].Length; x++)
                {
                    if (char.IsDigit(_grid[y][x]))
                    {
                        int count = 1;
                        while (x + count < _grid[y].Length && char.IsDigit(_grid[y][x + count]))
                        {
                            count++;
                        }

                        _numbers.Add((x, y, count, int.Parse(new string(_grid[y][x..(x + count)]))));
                        x += count;
                    }
                }
            }
        }

        private void PopulateGears()
        {
            _gears = new List<(int x, int y)>();
            for (int y = 0; y < _grid.Length; y++)
            {
                for (int x = 0; x < _grid[y].Length; x++)
                {
                    if (_grid[y][x] == '*')
                    {
                        _gears.Add(new (x,y));
                    }
                }
            }
        }

        List<(int x, int y)> GetAdjacent(int x, int y)
        {
            return new List<(int x, int y)>()
            {
                new(x - 1, y + 1),
                new(x, y + 1),
                new(x + 1, y + 1),

                new(x - 1, y),
                new(x + 1, y),

                new(x - 1, y - 1),
                new(x, y - 1),
                new(x + 1, y - 1),
            };
        }
        
        List<char> GetAdjacent(int x, int y, int size)
        {
            var adjacent = new List<char>();
            for (int y1 = -1; y1 <= 1; y1++)
            {
                for (int x1 = -1; x1 <= size; x1++)
                {
                    if (x1 == 0 && y1 == 0) continue;
                    if (x + x1 < 0 || x + x1 >= _grid.Length) continue;
                    if (y + y1 < 0 || y + y1 >= _grid[y].Length) continue;
                    adjacent.Add(_grid[y + y1][x + x1]);
                }
            }

            return adjacent;
        }
    }
    
    [Test]
    public void Part1()
    {
        int answer = 539590;
        var schematic = new Schematic();

        int x = schematic.Calculate1();
        
        Assert.That(answer, Is.EqualTo(x));
    }

    [Test]
    public void Part2()
    {
        int answer = 539590;
        var schematic = new Schematic();

        int x = schematic.Calculate2();
        
        Console.WriteLine(x);
        
        Assert.Pass();
    }
}