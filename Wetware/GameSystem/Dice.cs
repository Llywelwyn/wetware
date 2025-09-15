namespace Wetware.GameSystem;

public static class Dice
{
    private static bool m_seeded;
    private static Random m_random = new();

    public static void Seed(Random rng)
    {
        m_random = rng;
        m_seeded = true;
    }

    public static bool IsSeeded() => m_seeded;

    public static int Roll(int count, int sides, int bonus = 0)
    {
        var total = 0;
        for (var i = 0; i < count; i++)
        {
            total += Throw(sides);
        }

        return total + bonus;
    }

    private static int Throw(int sides) => m_random.Next(1, sides + 1);
}
