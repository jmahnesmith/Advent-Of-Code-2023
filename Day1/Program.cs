using System.Net;
using System.Runtime.CompilerServices;
using System.IO;

// Function to replace words in a line
string ReplaceNumberWords(string line, Dictionary<string, char> numberWords)
{
    foreach (var numberWord in numberWords)
    {
        if (line.Contains(numberWord.Key))
        {
            string replacement = numberWord.Key.First() + numberWord.Value.ToString() + numberWord.Key.Last();
            line = line.Replace(numberWord.Key, replacement);
        }
    }
    return line;
}

// Function to extract and combine the first and last numerical characters in a line
int? ExtractAndCombineNumbers(string line)
{
    var firstNumericalCharacter = line.FirstOrDefault(char.IsDigit);
    var lastNumericalCharacter = line.LastOrDefault(char.IsDigit);

    if (firstNumericalCharacter != default(char) && lastNumericalCharacter != default(char))
    {
        if (int.TryParse(firstNumericalCharacter.ToString() + lastNumericalCharacter.ToString(), out int combinedNumbers))
        {
            return combinedNumbers;
        }
    }
    return null;
}

// Main processing function
async Task<int> ProcessFileAsync(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);
    var numberWords = new Dictionary<string, char>
    {
        {"one", '1'}, {"two", '2'}, {"three", '3'}, {"four", '4'},
        {"five", '5'}, {"six", '6'}, {"seven", '7'}, {"eight", '8'}, {"nine", '9'}
    };

    var listOfCombinedCharacters = new List<int>();

    foreach (var line in lines)
    {
        var updatedLine = ReplaceNumberWords(line, numberWords);
        var combinedNumber = ExtractAndCombineNumbers(updatedLine);
        if (combinedNumber.HasValue)
        {
            listOfCombinedCharacters.Add(combinedNumber.Value);
        }
    }

    return listOfCombinedCharacters.Sum();
}

// Usage
var path = "resources/input.txt";
var totalSum = await ProcessFileAsync(path);
Console.WriteLine(totalSum);

// Result for day 1 part 2: 54719