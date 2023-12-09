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

public record Card(string input) : IComparable<Card>
{
    private static Dictionary<char, int> Values = new Dictionary<char, int>()
    {
        {'2', 2},
        {'3', 3},
        {'4', 4},
        {'5', 5},
        {'6', 6},
        {'7', 7},
        {'8', 8},
        {'9', 9},
        {'T', 10},
        {'J', 11},
        {'Q', 12},
        {'K', 13},
        {'A', 14}
    };

    public (CardScore, List<int>) Score()
    {
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

        var numerics = input.Select(c => Values[c]).ToList();
        // numerics.Sort();
        // numerics.Reverse();

        var counts = map.Values.ToList();

        if (counts.Contains(5))
        {
            return (CardScore.FiveOfAKind, numerics);
        }
        if (counts.Contains(4))
        {
            return (CardScore.FourOfAKind, numerics);
        }
        if (counts.Contains(3) && counts.Contains(2))
        {
            return (CardScore.FullHouse, numerics);
        }
        if (counts.Contains(3))
        {
            return (CardScore.ThreeOfAKind, numerics);
        }
        if (counts.Count(n => n == 2) == 2)
        {
            return (CardScore.TwoPair, numerics);
        }
        if (counts.Contains(2))
        {
            return (CardScore.OnePair, numerics);
        }

        return (CardScore.HighCard, numerics);
    }


    public int CompareTo(Card other)
    {
        var (score, values) = Score();
        var (oscore, ovalues) = other.Score();

        if (score != oscore)
        {
            return score - oscore;
        }

        for (int i = 0; i < 5; i++)
        {
            if (values[i] != ovalues[i])
            {
                return values[i] - ovalues[i];
            } 
        }

        return 0;
    }
}

public class Day7
{
    public static void Part1()
    {
        // AKJJ2 OnePair
        // ATQQ5 OnePair
        var cardsBids = File.ReadAllLines("7.txt")
            .Select(line =>
            {
                var ps = line.Split(" ");
                var bid = Int64.Parse(ps[1]);
                var card = new Card(ps[0]);
                return (card, bid);
            }).ToDictionary();

        var cards = cardsBids.Keys.ToList();
        cards.Sort();


        long sum = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            sum += cardsBids[cards[i]] * (i + 1);
        }
        Console.WriteLine(sum);

        // foreach (var card in cardsBids)
        // {
        //     var score = string.Join(" ", card.Key.Score().Item2);
        //     Console.WriteLine($"{card} {score}");
        // }
    }
}
