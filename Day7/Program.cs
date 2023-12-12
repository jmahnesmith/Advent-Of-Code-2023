// Day 7 Part 1 answer: 254494947
// Day 7 Part 2 answer: 254494947

var input = File.ReadAllLines("resources/input.txt");

var parseLine = (string line, string cardOrder, string jokers) =>
{
    var parts = line.Split(' ');
    var hand = parts[0];
    var bid = int.Parse(parts[1]);

    HandType handType = HandType.FiveOfAKind;
    var handWithoutJokers = jokers != "" ? hand.Replace(jokers, "") : hand;
    var numJokers = hand.Length - handWithoutJokers.Length;
    var groups =
        handWithoutJokers
        .GroupBy(x => x)
        .Select(x => x.Count())
        .OrderByDescending(x => x)
        .ToList();
    // Add a zero to handle hands with less than 5 different cards
    groups.Add(0);
    groups[0] += numJokers;

    if (groups[0] == 5)
        handType = HandType.FiveOfAKind;
    else if (groups[0] == 4)
        handType = HandType.FourOfAKind;
    else if (groups[0] == 3 && groups[1] == 2)
        handType = HandType.FullHouse;
    else if (groups[0] == 3)
        handType = HandType.ThreeOfAKind;
    else if (groups[0] == 2 && groups[1] == 2)
        handType = HandType.TwoPair;
    else if (groups[0] == 2)
        handType = HandType.OnePair;
    else
        handType = HandType.HighCard;

    var weight = hand.Select((card, index) => cardOrder.IndexOf(card) << (4 * (5 - index))).Sum();

    return (hand, handType, weight, bid);
};

var solve = (string cardOrder, string jokers) =>
{
    var hands = input.Select(line => parseLine(line, cardOrder, jokers));
    var orderedHands = hands.OrderBy(x => x.handType).ThenBy(x => x.weight);
    var result = orderedHands.Select((hand, index) => hand.bid * (index + 1)).Sum();
    return result;
};

var result1 = solve("23456789TJQKA", "");
Console.WriteLine($"Result1 = {result1}");

var result2 = solve("J23456789TQKA", "J");
Console.WriteLine($"Result2 = {result2}");

enum HandType
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind,
};
