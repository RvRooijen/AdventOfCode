using FluentAssertions;

namespace AdventOfCode.Tests;

public class Day5
{
    //private Dictionary<uint, uint> calculatedSeeds = new();

    [SetUp]
    public void Setup()
    {
        //calculatedSeeds = new Dictionary<uint, uint>();
    }

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
        private uint SourceRangeEnd => SourceRangeStart + RangeLength -1;
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

    private List<uint> GenerateOriginalSeeds()
    {
        var content = File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\Seeds.txt");

        List<uint> initialSeeds = content
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(uint.Parse)
            .ToList();
        return initialSeeds;
    }

    private List<MapGroup> CreateMapGroups()
    {
        List<MapGroup> mapGroups = new()
        {
            CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\SeedToSoil.txt")),
            CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\SoilToFertilizer.txt")),
            CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\FertilizerToWater.txt")),
            CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\WaterToLight.txt")),
            CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\LightToTemp.txt")),
            CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\TempToHumid.txt")),
            CreateMapping(File.ReadAllText("D:\\Projects\\AdventOfCode\\Input\\Day5\\HumidToLocation.txt"))
        };
        return mapGroups;
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
    public void SeedTest()
    {
        List<uint> initialSeeds = new() { 50, 51, 96, 97 };
        List<MapGroup> mapGroups = new();
        List<Mapping> mappings = new List<Mapping>()
        {
            new(50, 98, 2),
            new(52, 50, 48)
        };
        mapGroups.Add(new MapGroup(mappings));

        var min = GetSmallestLocationFromSeeds(initialSeeds, mapGroups);
        min.Should().Be(52);
    }
    
    [Test]
    public void MultiSeedTest()
    {
        List<List<uint>> holder = new List<List<uint>>
        {
            new()
            {
                50,51
            },
            new()
            {
                96,97
            }
        };

        List<MapGroup> mapGroups = new();
        List<Mapping> mappings = new List<Mapping>()
        {
            new(52, 50, 48),
            new(50, 98, 2),
        };
        mapGroups.Add(new MapGroup(mappings));
        
        var globalMin = GetMinimumFromHolder(holder, mapGroups);

        globalMin.Should().Be(52);
    }

    [Test]
    public void OverlappingTest()
    {
        List<List<uint>> holder = new List<List<uint>>
        {
            new()
            {
                50,51
            },
            new()
            {
                96,97
            },
            new()
            {
                96,97
            },
            new()
            {
                96,97
            },
            new()
            {
                96,97
            },
        };

        List<MapGroup> mapGroups = new();
        List<Mapping> mappings = new List<Mapping>()
        {
            new(50, 98, 2),
            new(52, 50, 48)
        };
        mapGroups.Add(new MapGroup(mappings));
        
        var globalMin = GetMinimumFromHolder(holder, mapGroups);

        globalMin.Should().Be(52);
    }
    
    [Test]
    public void MultimapTest()
    {
        List<List<uint>> holder = new List<List<uint>>
        {
            new()
            {
                10, 5, 2
            },
            new()
            {
                15, 20, 25
            }
        };

        List<MapGroup> mapGroups = new();
        List<Mapping> mappings1 = new List<Mapping>()
        {
            new(0, 0, 5)
        };
        
        List<Mapping> mappings2 = new List<Mapping>()
        {
            new(50, 15, 10)
        };
        
        mapGroups.Add(new MapGroup(mappings1));
        mapGroups.Add(new MapGroup(mappings2));
        
        var globalMin = GetMinimumFromHolder(holder, mapGroups);

        globalMin.Should().Be(2);
    }

    private uint GetMinimumFromHolder(List<List<uint>> holder, List<MapGroup> mapGroups)
    {
        uint globalMin = uint.MaxValue;
        // Per seed range
        foreach (var seeds in holder)
        {
            var check = GetSmallestLocationFromSeeds(seeds, mapGroups);
            Console.WriteLine(check);
            if (check < globalMin)
                globalMin = check;
        }

        return globalMin;
    }

    private uint GetSmallestLocationFromSeeds(List<uint> initialSeeds, List<MapGroup> mapGroups)
    {
        return initialSeeds
            .AsParallel()
            .Min(seed =>
            {
                var check = GetLocationFromSeed(seed, mapGroups);
                return check;
            });
    }

    private uint GetLocationFromSeed(uint seed, List<MapGroup> MapGroups)
    {
        return MapGroups
            .AsParallel()
            .Aggregate(seed, (currentSeed, group) => group
                .Convert(currentSeed));
    }

    [Test]
    public void Part1()
    {
        int answer = 3374647;

        var initialSeeds = GenerateOriginalSeeds();

        var mapGroups = CreateMapGroups();

        var min = GetSmallestLocationFromSeeds(initialSeeds, mapGroups);
        min.Should().Be(3374647);
    }
    
    [Test]
    [Ignore(" :D")]
    public void Part2()
    {
        List<List<uint>> GenerateSeedRange(List<uint> uints)
        {
            List<List<uint>> list = new List<List<uint>>();
            for (var i = 0; i < uints.Count; i++)
            {
                if (i % 2 == 1)
                {
                    var newList = new List<uint>();
                    for (uint j = uints[i - 1]; j < uints[i - 1] + uints[i]; j++)
                    {
                        newList.Add(j);
                    }

                    list.Add(newList);
                }
            }

            return list;
        }

        int answer = 3374647;

        var initialSeeds = GenerateOriginalSeeds();

        var seeds = GenerateSeedRange(initialSeeds);

        var mapGroups = CreateMapGroups();

        var lowest = GetMinimumFromHolder(seeds, mapGroups);

        Console.WriteLine(lowest);
    }
}