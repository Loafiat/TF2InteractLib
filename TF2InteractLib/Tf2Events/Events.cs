using TF2InteractLib.Tf2Events.Types.ParserTypes;

namespace TF2InteractLib.Tf2Events;

public static class Events
{
    public static event Action<string> OnConsoleOutput = EventManager.ParseLogEvent;

    internal static void ExecuteOnConsoleOutput(string eventLog)
    {
        OnConsoleOutput?.Invoke(eventLog);
    }
    
    public static event Action<ChatMessage> OnMessageSent = delegate { };

    internal static void ExecuteOnMessageSent(ChatMessage message)
    {
        OnMessageSent?.Invoke(message);
    }
    
    public static event Action<DeathParams> OnPlayerDied = delegate { };

    internal static void ExecuteOnPlayerDied(DeathParams message)
    {
        OnPlayerDied?.Invoke(message);
    }
    
    public static event Action<DefenseParams> OnPointDefended = delegate { };

    internal static void ExecuteOnPointDefended(DefenseParams message)
    {
        OnPointDefended?.Invoke(message);
    }
    
    public static event Action<CaptureParams> OnPointCaptured = delegate { };

    internal static void ExecuteOnPointCaptured(CaptureParams message)
    {
        OnPointCaptured?.Invoke(message);
    }
    
    public static event Action<string> OnCaptionSoundPlayed = delegate { };

    internal static void ExecuteOnCaptionSoundPlayed(string soundpath)
    {
        OnCaptionSoundPlayed?.Invoke(soundpath);
    }
    
    public static event Action<JoinedServerInfo> OnServerJoined = delegate { };

    internal static void ExecuteOnServerJoined(JoinedServerInfo serverInfo)
    {
        OnServerJoined?.Invoke(serverInfo);
    }
}