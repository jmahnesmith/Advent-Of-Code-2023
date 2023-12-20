// Day 8 Part 1 answer: 18673
// Day 8 Part 2 answer: 17972669116327

// Holds the mappings for each node.
Dictionary<string, (string left, string right)> _mappings = new();

// Read the mappings and directions, then calculate and display results.
ReadMappingsFromFile("resources/input.txt");
var directions = File.ReadAllLines("resources/input.txt").FirstOrDefault();
Console.WriteLine($"Solution 1 Steps: {CalculateStepsToTarget("AAA", "ZZZ", directions)}");
Console.WriteLine($"Solution 2 LCM: {CalculateLcmForAllAEndings(directions)}");

// Reads node mappings from a file and stores them in the dictionary.
void ReadMappingsFromFile(string filePath)
{
    try
    {
        string[] lines = File.ReadAllLines(filePath);
        var splitter = new[] { " ", "\t", "=", ",", "(", ")" };

        foreach (var line in lines.Skip(2))
        {
            var split = line.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            _mappings.Add(split[0], (split[1], split[2]));
        }
    }
    catch (IOException ex)
    {
        Console.WriteLine($"Error reading file: {ex.Message}");
    }
}

// Calculates the number of steps to traverse from a start node to a target node following given directions.
int CalculateStepsToTarget(string startNode, string targetNode, string directions)
{
    bool endFound = false;
    int steps = 0;
    var currNode = startNode;

    while (!endFound)
    {
        var dir = directions[steps % directions.Length];
        currNode = NextNode(currNode, dir);
        endFound = currNode == targetNode;
        steps++;
    }

    return steps;
}

// Returns the next node based on the current node and the direction.
string NextNode(string currentNode, char direction)
{
    return direction == 'L' ? _mappings[currentNode].left : _mappings[currentNode].right;
}

// Calculates the Least Common Multiple (LCM) of the steps required for all nodes ending with 'A' to reach their respective 'Z' nodes.
long CalculateLcmForAllAEndings(string directions)
{
    var currNodes = _mappings.Keys.Where(key => key.EndsWith("A")).ToList();
    long[] values = new long[currNodes.Count];

    for (int nodeInd = 0; nodeInd < currNodes.Count; nodeInd++)
    {
        bool endFound = false;
        int i = 0;

        while (!endFound)
        {
            var dir = directions[i % directions.Length];
            currNodes[nodeInd] = NextNode(currNodes[nodeInd], dir);

            if (currNodes[nodeInd].EndsWith("Z"))
                endFound = true;
            i++;
        }

        values[nodeInd] = i;
    }

    return Lcm(values);
}

// Helper method to compute the Greatest Common Divisor (GCD) of two numbers.
static long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

// Helper method to compute the Least Common Multiple (LCM) of an array of values.
static long Lcm(long[] values) => values.Aggregate((a, b) => a * b / Gcd(a, b));