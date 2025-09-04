namespace Wetware;

public static class Debug
{
    public static bool DebugAll = true;
    private static bool _logSystemBoundaries = true;

    public static bool LogSystemBoundaries => DebugAll || _logSystemBoundaries;
   
    public static void Print(string message) => Console.WriteLine(message);
    public static void SystemBoundaryStart(string name) => Console.WriteLine($"=== STARTING: {name}");
    public static void SystemBoundaryEnd(string name) => Console.WriteLine($"~~~ ENDING: {name}");
}