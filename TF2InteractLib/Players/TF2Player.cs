namespace TF2InteractLib.Players;

public struct TF2Player
{
    // Steam User Info
    public string SteamName { get; set; }
    public int SteamID { get; set; }
    public int UserID { get; set; }
    
    // Scoreboard info
    public int Score { get; set; }
    public int Deaths { get; set; }
    public int Ping { get; set; }
    
    // Connection Status
    public bool IsConnected { get; set; }
    public bool IsValid  { get; set; }
    
    // Player info
    public TF2Team Team { get; set; }
    public int Health { get; set; }
    public bool Alive { get; set; }
}