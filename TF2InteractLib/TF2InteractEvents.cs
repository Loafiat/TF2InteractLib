using TF2InteractLib.Events;

namespace TF2InteractLib;

public class TF2InteractEvents
{
    public static event Action<string> ConsoleLine = TF2InteractAPI.EventParser;

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