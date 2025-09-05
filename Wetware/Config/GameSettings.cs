namespace Wetware.Config;

public static class GameSettings
{
   private const string Title = "Wetware";
   private const string Version = "0.0.1";
   private const string Author = "Lewis Wynne";

   public static string Attribution() => $"{Title} {Version}, by {Author}.";
}