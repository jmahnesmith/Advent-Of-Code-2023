Console.WriteLine("Day 05:");
const string FILE = "resources/input.txt";

// Read lines from the input file
string[] lines = File.ReadAllLines(FILE);

// Extract seed inputs from the first line of the file
string[] seedInputs = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

// Initialize a list to store seed values for Part 1
List<long> seeds = new List<long>();
for (int i = 0; i < seedInputs.Length; i++)
{
    seeds.Add(long.Parse(seedInputs[i]));
}

// Initialize a variable to keep track of the current element index and a dictionary to map elements for Part 2
int eleIndex = -1;
var elementMap = new Dictionary<(int, int), List<(long, long, long)>>();

// Process the remaining lines of the file to populate the elementMap for Part 2
for (int i = 1; i < lines.Length; i++)
{
    string line = lines[i];
    if (string.IsNullOrWhiteSpace(line))
    {
        eleIndex++; // Increment element index when encountering a blank line
        continue;
    }
    if (line.EndsWith("map:"))
    {
        continue; // Skip lines ending with "map:"
    }

    // Parse each line into destination, source, and length components
    long[] parts = line.Split(' ').Select(long.Parse).ToArray();
    long dest = parts[0], source = parts[1], length = parts[2];

    // Use element index as key and add parsed data to the elementMap
    var key = (eleIndex, eleIndex + 1);
    if (!elementMap.ContainsKey(key))
    {
        elementMap[key] = new List<(long, long, long)>();
    }
    elementMap[key].Add((dest, source, length));
}

// Execute Part 1 Logic: Adjust seed values based on the elementMap
for (int i = 0; i <= eleIndex; i++)
{
    for (int j = 0; j < seeds.Count; j++)
    {
        long loc = seeds[j];
        foreach (var (destg, sourceg, leng) in elementMap[(i, i + 1)])
        {
            // Check and update seed value if it falls within a mapped range
            if (sourceg <= loc && loc < sourceg + leng)
            {
                seeds[j] = destg - sourceg + loc;
                break;
            }
        }
    }
}
long minLocation1 = seeds.Min(); // Find the minimum value among the updated seeds

// Initialize seed ranges for Part 2
List<(long, long)> seedRanges = new List<(long, long)>();
for (int i = 0; i < seedInputs.Length; i += 2)
{
    long start = long.Parse(seedInputs[i]);
    long length = long.Parse(seedInputs[i + 1]);
    seedRanges.Add((start, start + length));
}

// Execute Part 2 Logic: Process each data block in the elementMap
foreach (var dataBlock in elementMap)
{
    var newSeedRanges = new List<(long, long)>();
    while (seedRanges.Count > 0)
    {
        var (start, end) = seedRanges.Last();
        seedRanges.RemoveAt(seedRanges.Count - 1);

        bool isOverlapped = false;
        foreach (var (destination, source, length) in dataBlock.Value)
        {
            long overlapStart = Math.Max(start, source);
            long overlapEnd = Math.Min(end, source + length);

            // Check for overlap and update ranges accordingly
            if (overlapStart < overlapEnd)
            {
                isOverlapped = true;
                newSeedRanges.Add((overlapStart - source + destination, overlapEnd - source + destination));
                // Add remaining parts of the range if they exist
                if (overlapStart > start)
                {
                    seedRanges.Add((start, overlapStart));
                }
                if (overlapEnd < end)
                {
                    seedRanges.Add((overlapEnd, end));
                }
                break;
            }
        }

        // Add original range if no overlap is found
        if (!isOverlapped)
        {
            newSeedRanges.Add((start, end));
        }
    }

    seedRanges = newSeedRanges.Distinct().ToList(); // Update seedRanges with the new ranges, removing duplicates
}

long answerPart2 = seedRanges.Min(sr => sr.Item1); // Find the minimum value among the final seed ranges

// Display the results for Part 1 and Part 2
Console.WriteLine("Part 1: " + minLocation1);
Console.WriteLine("Part 2: " + answerPart2);
