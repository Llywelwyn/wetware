namespace Wetware.GameSystem;

public static class Dice
{
    private static bool _seeded;
    private static Random _random = new();

    public static void Seed(Random rng)
    {
        _random = rng;
        _seeded = true;
    }

    public static bool IsSeeded() => _seeded;

    public static int Roll(int count, int sides, int bonus = 0)
    {
        var total = 0;
        for (var i = 0; i < count; i++)
        {
            total += Throw(sides);
        }

        return total + bonus;
    }

    private static int Throw(int sides) => _random.Next(1, sides + 1);
}