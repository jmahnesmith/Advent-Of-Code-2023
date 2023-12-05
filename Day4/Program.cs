// Solution for Day 4 part 1: 24733
// Solution for Day 4 part 2: 5422730


// Read the file and convert it into a list;
var filePath = "resources/input.txt";
var lines = await File.ReadAllLinesAsync(filePath);
var listOfLines = lines.ToList();

var game = new Game();
game.ParseData(listOfLines);

Console.WriteLine($"Total Calculated points for all cards: {game.CalculatePoints()}");
public class Game
{
    public List<Card> Cards { get; private set; }
    public Game()
    {
        Cards = new List<Card>();
    }

    public void ParseData(List<string> listOfLines)
    {
        foreach (var line in listOfLines)
        {
            var initialCardData = line.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            // Remove un-needed parsed data in the format of "Card 1:"
            var cardData = initialCardData[1];

            int cardNumber = 1;

            var parts = cardData.Split('|');
            var winningNumbers = parts[0].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
            var cardNumbers = parts[1].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();


            Card card = new Card(cardNumber, winningNumbers, cardNumbers);
            Cards.Add(card);
        }
    }

    // Calculates points specified in part 1
    public int CalculatePoints()
    {
        var points = 0;

        foreach (var card in Cards)
        {
            var pointsForCard = 0;
            var isFirstPoint = true;
            foreach (var number in card.CardNumbers)
            {
                if (card.WinningNumbers.Contains(number))
                {
                    if (isFirstPoint)
                    {
                        pointsForCard += 1;
                        isFirstPoint = false;
                    }
                    else
                    {
                        pointsForCard *= 2;
                    }
                }
            }

            points += pointsForCard;
        }

        return points;
    }

    // Calculates the total number of scratchcards including copies in part 2
    public int CalculateTotalScratchcards()
    {
        // Initialize the counts array
        var counts = Cards.Select(_ => 1).ToArray();

        // Iterate over each card in the Cards list
        for (var i = 0; i < Cards.Count; i++)
        {
            // Retrieve the current card
            var card = Cards[i];

            // For each match that the current card has...
            for (var j = 0; j < card.Matches; j++)
            {
                // Check if the subsequent card index is within bounds
                if (i + j + 1 < Cards.Count)
                {
                    // Add the count of the current card to the subsequent card's count
                    counts[i + j + 1] += counts[i];
                }
            }
        }

        // Return the sum of all counts
        return counts.Sum();
    }

}

public class Card
{
    public Card(int cardNumber, List<int> winningNumbers, List<int> cardNumbers)
    {
        CardNumber = cardNumber;
        WinningNumbers = winningNumbers;
        CardNumbers = cardNumbers;
    }

    public int CardNumber { get; }

    public int Matches
    {
        get
        {
            return WinningNumbers.Intersect(CardNumbers).Count();
        }
    }
    public List<int> WinningNumbers { get; }
    public List<int> CardNumbers { get; }
}