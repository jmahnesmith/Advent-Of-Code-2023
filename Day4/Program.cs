
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
}

public class Card
{
    public Card(int cardNumber, List<int> winningNumbers, List<int> cardNumbers)
    {
        WinningNumbers = winningNumbers;
        CardNumbers = cardNumbers;
    }

    public List<int> WinningNumbers { get; }
    public List<int> CardNumbers { get; }
}