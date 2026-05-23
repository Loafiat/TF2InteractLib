using TF2InteractLib.Events;
using TF2InteractLib.Players;

namespace TF2InteractLib;

public class TF2InteractEvents
{
    public static event EventHandler<string> ConsoleLine = TF2InteractAPI.EventParser;

    public static event EventHandler<PlayerKillArgs> PlayerKilled;

    public static event EventHandler<PlayerMessageArgs> PlayerMessage;

    public static void ExecuteConsoleLine(string newLine)
    {
        ConsoleLine?.Invoke(null, newLine);
    }

    public static void ExecutePlayerKilled(PlayerKillArgs args)
    {
        PlayerKilled?.Invoke(null, args);
    }

    public static void ExecutePlayerTalk(PlayerMessageArgs args)
    {
        PlayerMessage?.Invoke(null, args);
    }
}