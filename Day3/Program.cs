// Part 1 Answer: 549908
// Part 2 Answer: 81166799
using System.Collections.Generic;
using System.IO;

// Read the file and convert it into a 2D character array
var filePath = "resources/input.txt";
var lines = await File.ReadAllLinesAsync(filePath).To2DArrayAsync();

// Sum to hold the total gear ratio sum
int sum = 0;

// Iterate through each character in the 2D array
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        char currentChar = lines[i][j];

        // Check if the current character is not a digit and not a period
        if (!char.IsDigit(currentChar) && currentChar != '.')
        {
            sum += CalculateGearRatio(lines, i, j);
        }
    }
}

Console.WriteLine($"Sum of all part numbers: {sum}");

// Read the complete number in a line around a given position in the 2D array
string ReadNumbersInSurrounding(char[][] array2D, int rowIndex, int colIndex)
{
    if (rowIndex < 0 || rowIndex >= array2D.Length || colIndex < 0 || colIndex >= array2D[rowIndex].Length)
        return "";

    if (!char.IsDigit(array2D[rowIndex][colIndex]))
        return "";

    string number = array2D[rowIndex][colIndex].ToString();
    int left = colIndex - 1;

    // Reading left
    while (left >= 0 && char.IsDigit(array2D[rowIndex][left]))
    {
        number = array2D[rowIndex][left] + number;
        left--;
    }

    int right = colIndex + 1;

    // Reading right
    while (right < array2D[rowIndex].Length && char.IsDigit(array2D[rowIndex][right]))
    {
        number += array2D[rowIndex][right];
        right++;
    }

    return number;
}

// Method to check if a given position in a 2D array is adjacent to a symbol
bool IsAdjacentToSymbol(char[][] array2D, int rowIndex, int colIndex)
{
    // Offset arrays to check all 8 directions around the given position
    // These arrays represent relative positions for neighboring cells
    int[] rowOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
    int[] colOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };

    for (int i = 0; i < 8; i++)
    {
        int newRow = rowIndex + rowOffsets[i];
        int newCol = colIndex + colOffsets[i];

        if (newRow >= 0 && newRow < array2D.Length && newCol >= 0 && newCol < array2D[newRow].Length)
        {
            char c = array2D[newRow][newCol];
            if (!char.IsDigit(c) && c != '.')
            {
                return true;
            }
        }
    }

    return false;
}

// Calculate sum for a given symbol position
int SumAdjacentNumbers(char[][] array2D, int rowIndex, int colIndex)
{
    int sum = 0;
    // Offset arrays to check all 8 directions around the given position
    // These arrays represent relative positions for neighboring cells
    int[] rowOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
    int[] colOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };

    HashSet<string> seenNumbers = new HashSet<string>();

    for (int i = 0; i < 8; i++)
    {
        int newRow = rowIndex + rowOffsets[i];
        int newCol = colIndex + colOffsets[i];

        if (newRow >= 0 && newRow < array2D.Length && newCol >= 0 && newCol < array2D[newRow].Length)
        {
            if (char.IsDigit(array2D[newRow][newCol]))
            {
                string number = ReadNumbersInSurrounding(array2D, newRow, newCol);
                if (seenNumbers.Add(number))
                {
                    sum += int.Parse(number);
                }
            }
        }
    }

    return sum;
}

// Calculate gear ratio for a given symbol position
int CalculateGearRatio(char[][] array2D, int rowIndex, int colIndex)
{
    List<int> adjacentNumbers = new List<int>();
    int[] rowOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
    int[] colOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };
    HashSet<string> seenNumbers = new HashSet<string>();

    // Check all 8 adjacent positions for part numbers      
    for (int i = 0; i < 8; i++)
    {
        int newRow = rowIndex + rowOffsets[i];
        int newCol = colIndex + colOffsets[i];

        if (newRow >= 0 && newRow < array2D.Length && newCol >= 0 && newCol < array2D[newRow].Length)
        {
            if (char.IsDigit(array2D[newRow][newCol]))
            {
                string numberStr = ReadNumbersInSurrounding(array2D, newRow, newCol);

                // Skip if the number is already added
                if (!seenNumbers.Add(numberStr)) continue;

                int number;
                if (int.TryParse(numberStr, out number))
                {

                    adjacentNumbers.Add(number);
                }
            }
        }
    }

    // If exactly two adjacent numbers are found, return their product
    return adjacentNumbers.Count == 2 ? adjacentNumbers[0] * adjacentNumbers[1] : 0;
}

// Extension method to convert string array to 2D char array
public static class FileExtensions
{
    public static async Task<char[][]> To2DArrayAsync(this Task<string[]> linesTask)
    {
        var lines = await linesTask;
        var result = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            result[i] = lines[i].ToCharArray();
        }

        return result;
    }
}