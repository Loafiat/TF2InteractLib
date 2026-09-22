using TF2InteractLib.Events.Types.ParserTypes;

namespace TF2InteractLib.Events;

public class Events
{
    public static event Action<ChatMessage> OnMessageSent = delegate { };

    internal static void ExecuteOnMessageSent(ChatMessage message)
    {
        OnMessageSent?.Invoke(message);
    }
}