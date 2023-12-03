// Part 1 Answer: 2176
// Part 2 Anser: 63700
using System.Buffers;
using System.Net;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.Serialization;

var filePath = "resources/input.txt";
var lines = await File.ReadAllLinesAsync(filePath);
var listOfLines = lines.ToList();

int gameCount = 1;
List<int> validGameIds = new List<int>();
// Process each line to create a game and calculate its power
listOfLines.ForEach(line =>
{
    var game = new Game(gameCount, line);
    gameCount++;
    // Uncomment the following line to filter out invalid games
    //if (!game.IsGameInvalid())
    //{
    validGameIds.Add(game.GetPowerOfRolls());
    //}
});

// Output the sum of the powers of all valid games
Console.WriteLine(validGameIds.Sum());

class Game
{
    public int Id;
    public List<Dice> Rolls = new List<Dice>();
    public Game(int gameId, string line)
    {
        this.Id = gameId;
        ParseLine(line);
    }

    // Checks if the game is invalid based on dice roll criteria    
    public bool IsGameInvalid()
    {
        return Rolls.Any(roll =>
        {
            return roll.Blue > roll.MaxBlueLength ||
           roll.Green > roll.MaxGreenLength ||
           roll.Red > roll.MaxRedLength;
        });
    }

    // Calculates the power of rolls in the game
    public int GetPowerOfRolls()
    {
        if (Rolls == null || Rolls.Count == 0)
        {
            return 0;
        }

        var largestBlue = Rolls.Max(roll => roll.Blue);
        var largestGreen = Rolls.Max(roll => roll.Green);
        var largestRed = Rolls.Max(roll => roll.Red);

        return largestBlue * largestGreen * largestRed;
    }

    // Parses a line from the input file to create rolls    
    private void ParseLine(string line)
    {
        line = RemoveGameText(line);
        var rollDescriptions = line.Split(';');
        foreach (var roll in rollDescriptions)
        {
            var trimmedRoll = roll.Trim();
            if (!string.IsNullOrEmpty(trimmedRoll))
            {
                Rolls.Add(ParseRoll(trimmedRoll));
            }
        }
    }

    // Removes the game text (everything before ':') from the line
    private static string RemoveGameText(string line)
    {
        int colonIndex = line.IndexOf(':');
        line = line.Substring(colonIndex + 1).Trim();
        return line;
    }

    // Parses a single roll from the input line
    private Dice ParseRoll(string roll)
    {
        var dice = new Dice();
        var colorCounts = roll.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var colorCount in colorCounts)
        {
            var parts = colorCount.Trim().Split(' ');
            if (parts.Length == 2 && int.TryParse(parts[0], out int number))
            {
                switch (parts[1].ToLower())
                {
                    case "green":
                        dice.Green += number;
                        break;
                    case "red":
                        dice.Red += number;
                        break;
                    case "blue":
                        dice.Blue += number;
                        break;
                }
            }
        }

        return dice;
    }
}

class Dice
{
    public int Green { get; set; } = 0;
    public int Red { get; set; } = 0;
    public int Blue { get; set; } = 0;

    // Maximum allowed length for each color
    public int MaxGreenLength { get; private set; } = 13;
    public int MaxRedLength { get; private set; } = 12;
    public int MaxBlueLength { get; private set; } = 14;
}