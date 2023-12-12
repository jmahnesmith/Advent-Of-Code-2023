// Day 6 part 1 answer: 4568778
// Day 6 part 2 answer: 28973936

// Read the file and convert it into a list
var filePath = "resources/input.txt";
var lines = await File.ReadAllLinesAsync(filePath);

// Processing for Part 1
var racesPart1 = ParseRacesPart1(lines);
var part1Total = CalculateTotal(racesPart1);

// Processing for Part 2
var racePart2 = ParseRacesPart2(lines);
var part2Total = CalculateTotal(racePart2);

// Output results
Console.WriteLine($"Part1 total: {part1Total}");
Console.WriteLine($"Part2 total: {part2Total}");

// Calculates the total based on a list of races
long CalculateTotal(List<Race> races)
{
    long total = 1;
    races.ForEach(race =>
    {
        long recordBroken = 0;
        for (int i = 0; i <= race.Time; i++)
        {
            var distance = CalculateDistance(i, race.Time);
            if (distance > race.Distance)
            {
                recordBroken++;
            }
        }
        total *= recordBroken;
    });

    return total;
}

// Calculates the distance for a given hold time and total time
long CalculateDistance(long holdTime, long totalTime)
{
    return holdTime * (totalTime - holdTime);
}

// Parses the races for Part 1 format
List<Race> ParseRacesPart1(string[] lines)
{
    var times = lines[0].Substring("Time:".Length).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
    var distances = lines[1].Substring("Distance:".Length).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

    var races = new List<Race>();
    for (int i = 0; i < times.Length; i++)
    {
        races.Add(new Race(times[i], distances[i]));
    }

    return races;
}

// Parses the races for Part 2 format
List<Race> ParseRacesPart2(string[] lines)
{
    List<Race> races = new();
    var time = lines[0].Substring("Time:".Length).Replace(" ", "");
    var distance = lines[1].Substring("Distance:".Length).Replace(" ", "");

    var race = new Race(long.Parse(time), long.Parse(distance));
    races.Add(race);

    return races;
}

// Record to represent a race with time and distance
public record Race(long Time, long Distance);
