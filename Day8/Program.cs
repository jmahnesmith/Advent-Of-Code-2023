// Day 8 Part 1 answer: 18673

var input = File.ReadAllLines("resources/input.txt").ToList();
var network = ParseInput(input);

var part1HopCount = network.SolvePart1("ZZZ");
Console.WriteLine($"Part1 Hop Count: {part1HopCount}");


var part2HopCount = network.SolvePart2();
Console.WriteLine($"Part2 Hop Count: {part2HopCount}");

Network ParseInput(List<string> input)
{
    ArgumentNullException.ThrowIfNull(input);

    var directions = input.FirstOrDefault().ToCharArray();

    var start = "AAA";

    /*input.Skip(1)
        .Where(x => !string.IsNullOrEmpty(x))
        .Select(x => x.Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim())
        .FirstOrDefault().Replace("=", "").Trim();*/

    var map = input.Skip(1)
    .Where(x => !string.IsNullOrEmpty(x))
    .SelectMany(x =>
    {
        string[] parts = x.Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
        return new Dictionary<string, (string Left, string Right)>
        {
            {parts[0].Replace("=", "").Trim(), (parts[1].Trim(), parts[2].Trim())}
        };
    }).ToDictionary(kv => kv.Key, kv => kv.Value);

    return new Network(map, directions, start);
}

public class Network
{
    public string Start { get; private set; }
    public Dictionary<string, (string Left, string Right)> Map { get; private set; }
    public char[] Directions { get; private set; }

    public Network(Dictionary<string, (string Left, string Right)> map, char[] directions, string start)
    {
        Map = map;
        Directions = directions;
        Start = start;
    }

    public int SolvePart1(string destination)
    {
        var currentLocation = Start;
        // Initialize the hop counter
        var hopCount = 0;

        for (int i = 0; i < Directions.Length; i++)
        {
            var options = Map[currentLocation];
            switch (Directions[i])
            {
                case 'R':
                    currentLocation = options.Right;
                    break;
                case 'L':
                    currentLocation = options.Left;
                    break;
            }

            // Increment hop count at each move
            hopCount++;

            if (currentLocation == destination)
            {
                return hopCount;
            }
            // Check if index has reached the last element if so reset loop.
            if (i == Directions.Length - 1)
            {
                // Reset to -1 as it will be incremented to 0 at the end of the loop
                i = -1;
            }
        }


        return hopCount;
    }

    private List<string> FindPath(string start, Func<(string Left, string Right), bool> condition)
    {
        var path = new List<string>();
        var current = Map[start];
        var lrCnt = 0;

        while (!condition(current))
        {
            path.Add(start);
            start = Directions[lrCnt % Directions.Length] == 'L' ? current.Left : current.Right;
            current = Map[start];
            lrCnt++;
        }

        return path;
    }

    private long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            a %= b;
            (a, b) = (b, a);
        }
        return a;
    }

    private long FindLeastCommonMultiple(IEnumerable<long> numbers) =>
        numbers.Aggregate((long)1, (current, number) => current / GreatestCommonDivisor(current, number) * number);

    public long SolvePart2()
    {
        var pathLengths = Map.Keys.Where(key => key.EndsWith("A"))
                                  .Select(start => FindPath(start, node => node.Right.EndsWith("Z") || node.Left.EndsWith("Z")))
                                  .Select(path => (long)path.Count);

        return FindLeastCommonMultiple(pathLengths);
    }

}