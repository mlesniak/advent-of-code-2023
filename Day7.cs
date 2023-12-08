namespace Lesniak.AoC2023;

public enum CardScore
{
    FiveOfAKind  = 7,
    FourOfAKind  = 6,
    FullHouse    = 5,
    ThreeOfAKind = 4,
    TwoPair      = 3,
    OnePair      = 2,
    HighCard     = 1
}

public record Card(string input)
{
    // 1 High card
    // 2 One pair
    // 3 Two pair
    // 4 Three of a kind
    // 5 Full house
    // 6 Four of a kind
    // 7 Five of a kind
    public CardScore Score()
    {
        // Move card values into a map <char, count>
        var map = new Dictionary<char, int>();
        foreach (var c in input)
        {
            int cur = 0;
            if (map.TryGetValue(c, out int count))
            {
                cur = count;
            }
            map[c] = cur + 1;
        }

        // foreach (var pair in map)
        // {
        //     Console.WriteLine($"{pair.Key} -> {pair.Value}");    
        // }

        var counts = map.Values.ToList();
        Console.WriteLine(string.Join(",", counts));

        if (counts.Contains(5))
        {
            return CardScore.FiveOfAKind;
        }
        if (counts.Contains(4))
        {
            return CardScore.FourOfAKind;
        }
        if (counts.Contains(3) && counts.Contains(2))
        {
            return CardScore.FullHouse;
        }
        if (counts.Contains(3))
        {
            return CardScore.ThreeOfAKind;
        }
        if (counts.Count(n => n == 2) == 2)
        {
            return CardScore.TwoPair;
        }
        if (counts.Contains(2))
        {
            return CardScore.OnePair;
        }

        return CardScore.HighCard;
    }
}

public class Day7
{
    public static void Part1()
    {
        var cards = File.ReadAllLines("7.txt")
            .Select(line =>
            {
                var ps = line.Split(" ");
                var bid = Int32.Parse(ps[1]);
                var card = new Card(ps[0]);
                return (card, bid);
            }).ToDictionary();

        foreach (var pair in cards)
        {
            Console.WriteLine(pair.Key);
            Console.WriteLine(pair.Key.Score());
        }
    }
}
