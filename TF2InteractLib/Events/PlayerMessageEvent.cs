using TF2InteractLib.Players;

namespace TF2InteractLib.Events;

public struct PlayerMessageArgs
{
    public bool Dead { get; set; }
    
    public bool Team { get; set; }
    
    public TF2Player Sender { get; set; }
    
    public string Message { get; set; }
}