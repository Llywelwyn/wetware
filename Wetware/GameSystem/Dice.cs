using System.Security.Cryptography;

namespace Wetware.GameSystem;

public static class Dice
{
    private static bool _seeded = false;
    private static Random _random = new Random();

    public static void Seed(Random rng)
    {
        _random = rng;
        _seeded = true;
    }

    public static bool IsSeeded() => _seeded;

    public static int Roll(int count, int sides)
    {
        if (_random is null) throw new InvalidOperationException("Dice.Init() must be called first.");

        var total = 0;
        for (var i = 0; i < count; i++)
        {
            total += Throw(sides);
        }

        return total;
    }

    private static int Throw(int sides) => _random.Next(1, sides + 1);
}