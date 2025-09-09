namespace Wetware.Config;

public enum DebugFlag
{
    SystemBoundary,
    AdditionalJsonSerialization,
}

public static class Debug
{
    private const bool DebugAllOverride = true;
    private static readonly Dictionary<DebugFlag, bool> Flags = new()
    {
        [DebugFlag.SystemBoundary] = false,
        [DebugFlag.AdditionalJsonSerialization] = false,
    };

    public static bool IsEnabled(DebugFlag name)
    {
        if (!Flags.TryGetValue(name, out var value))
            throw new ArgumentException($"Unknown flag: {name}.");
        return value || DebugAllOverride;
    }

    public static void Print(string message) => Console.WriteLine(message);

    public static void SystemBoundaryStart(string name)
    {
        if (IsEnabled(DebugFlag.SystemBoundary))
            Console.WriteLine($"=== STARTING: {name}");
    }

    public static void SystemBoundaryEnd(string name)
    {
        if (IsEnabled(DebugFlag.SystemBoundary))
            Console.WriteLine($"~~~ ENDING: {name}");
    }
}
