using TF2InteractLib.Events;

namespace TF2InteractLib;

public class TF2InteractEvents
{
    public static event Action<string> ConsoleLine;

    // non-functional. See the commented chunk of TF2InteractAPI
    public static event Action<PlayerKillArgs> PlayerKilled;

    public static void ExecuteConsoleLine(string newLine)
    {
        ConsoleLine?.Invoke(newLine);
    }

    public static void ExecutePlayerKilled(PlayerKillArgs args)
    {
        PlayerKilled?.Invoke(args);
    }
}