// Day 9 Part 1 Answer: 1882395907

// Read all lines from the input file
var input = File.ReadAllLines("resources/input.txt").ToList();

// Iterate through each line and create Pyramid objects
List<Pyramid> pyramids = new();
input.ForEach(line =>
{
    // Split each line into long integers and initialize Pyramid
    var initialArray = line.Split(' ').Select(long.Parse).ToList();
    pyramids.Add(new Pyramid(initialArray));
});

// Calculate the sum of predicted values from all pyramids
long part1Answer = pyramids.Sum(pyramid => pyramid.GetPredictedValue());

// Output the result
Console.WriteLine($"Part 1 answer: {part1Answer}");

class Pyramid
{
    public List<List<long>> Levels { get; private set; }

    // Constructor: Initializes the pyramid with the top level and generates the full pyramid
    public Pyramid(IEnumerable<long> topLevel)
    {
        Levels = new List<List<long>> { topLevel.ToList() };
        GeneratePyramid();
        SetPredictedValues();
    }

    // Generates the entire pyramid levels
    private void GeneratePyramid()
    {
        while (true)
        {
            var nextLevel = GenerateNextLevel(Levels.Last());

            // Add the generated level to the pyramid
            Levels.Add(nextLevel);

            // Stop if all elements in the level are zero
            if (nextLevel.All(n => n == 0))
                break;
        }
    }

    // Generates the next level in the pyramid based on differences between elements
    private List<long> GenerateNextLevel(List<long> currentLevel)
    {
        // Return [0] if the current level has only one element
        if (currentLevel.Count == 1)
        {
            return new List<long> { 0 };
        }

        // Calculate the differences between adjacent elements
        return currentLevel
            .Zip(currentLevel.Skip(1), (a, b) => b - a)
            .ToList();
    }

    // Sets predicted values for each level in the pyramid
    private void SetPredictedValues()
    {
        // Add 0 to the last level as a starting point
        Levels.Last().Add(0);

        // Work our way up the pyramid, setting predicted values
        for (int level = Levels.Count - 1; level >= 0; level--)
        {
            // Ensure we're within range before calculating the next value
            if (level + 1 < Levels.Count)
            {
                long nextValue = Levels[level].Last() + Levels[level + 1][^1];
                Levels[level].Add(nextValue);
            }
        }
    }

    // Retrieves the predicted value from the top level of the pyramid
    public long GetPredictedValue()
    {
        return Levels.First().Last();
    }
}