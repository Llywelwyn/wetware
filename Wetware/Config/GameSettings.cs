namespace Wetware.Config;

public static class GameSettings
{
   private const string m_title = "Wetware";
   private const string m_version = "0.0.1";
   private const string m_author = "Lewis Wynne";

   public static string Attribution() => $"{m_title} {m_version}, by {m_author}.";
}
