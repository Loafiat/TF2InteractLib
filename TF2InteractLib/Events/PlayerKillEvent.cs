using TF2InteractLib.Players;

namespace TF2InteractLib.Events;

public struct PlayerKillArgs
{
    public TF2Player? Killer { get; set; }
    public TF2Player Victim { get; set; }
    public string? WeaponName { get; set; }
    public bool CriticalKill { get; set; }
}