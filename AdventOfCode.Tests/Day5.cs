using FluentAssertions;

namespace AdventOfCode.Tests;

public class Day5
{
    private List<MapGroup> MapGroups = new();

    private class MapGroup
    {
        private List<Mapping> _mappings { get; }

        public MapGroup(List<Mapping> mappings)
        {
            _mappings = mappings;
        }

        public uint Convert(uint x)
        {
            // Check if x is between any source range start and source range start + range length
            var find = _mappings.FirstOrDefault(mapping => mapping.IsInRange(x));
            return find?.MapValue(x) ?? x;
        }
    }
    
    private class Mapping
    {
        public uint DestinationRangeStart { get; }
        public uint SourceRangeStart { get; }
        public uint RangeLength { get; }
        private uint SourceRangeEnd => SourceRangeStart + RangeLength;
        public uint DestinationRangeEnd => DestinationRangeStart + RangeLength;

        public bool IsInRange(uint x) => x >= SourceRangeStart && x <= SourceRangeEnd;

        public Mapping(uint destinationRangeStart, uint sourceRangeStart, uint rangeLength)
        {
            DestinationRangeStart = destinationRangeStart;
            SourceRangeStart = sourceRangeStart;
            RangeLength = rangeLength;
        }

        public uint MapValue(uint x)
        {
            var diff = x - SourceRangeStart;
            return DestinationRangeStart + diff;
        }
    }
    
    private MapGroup CreateMapping(string readAllText)
    {
        var lines = readAllText.Split('\n');
        var mappings = lines
            .Select(line => line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(uint.Parse)
                .ToList())
            .Select(items => new Mapping(items[0], items[1], items[2]))
            .ToList();

        return new MapGroup(mappings);
    }

    [Test]
    public void Example()
    {
        var group = new MapGroup(new List<Mapping>()
        {
            new Mapping(50, 98, 2),
            new Mapping(52, 50, 48),
        });
        
        // Examples page
        /*
            seed  soil
            0     0
            1     1
            ...   ...
            48    48
            49    49
            50    52
            51    53
            ...   ...
            96    98
            97    99
            98    50
            99    51
        */
        
        group.Convert(0).Should().Be(0);
        group.Convert(1).Should().Be(1);
        
        group.Convert(48).Should().Be(48);
        group.Convert(49).Should().Be(49);
        
        group.Convert(50).Should().Be(52);
        group.Convert(51).Should().Be(53);
        
        group.Convert(96).Should().Be(98);
        group.Convert(97).Should().Be(99);
        group.Convert(98).Should().Be(50);
        group.Convert(99).Should().Be(51);
    }
    
    [Test]
    public void Test()
    {
        int answer = 3374647;

        var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\Seeds.txt");
        
        List<uint> initialSeeds = content
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(uint.Parse)
            .ToList();
        
        MapGroups.Add(CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\SeedToSoil.txt")));
        MapGroups.Add(CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\SoilToFertilizer.txt")));
        MapGroups.Add(CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\FertilizerToWater.txt")));
        MapGroups.Add(CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\WaterToLight.txt")));
        MapGroups.Add(CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\LightToTemp.txt")));
        MapGroups.Add(CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\TempToHumid.txt")));
        MapGroups.Add(CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\HumidToLocation.txt")));

        var lowest = initialSeeds
            .Select(seed => MapGroups
                .Aggregate(seed, (current, group) => group
                    .Convert(current)))
            .Min();
        lowest.Should().Be(3374647);
    }
}