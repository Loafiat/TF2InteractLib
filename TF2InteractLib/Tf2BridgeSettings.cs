namespace TF2InteractLib;

public struct Tf2BridgeSettings
{
    public string? RconPassword;
    public string RconHost = "127.0.0.1";
    public int RconPort = 27015;
    public string? Tf2Path = null;

    private string? _logFileName;
    public string? LogFileName
    {
        get => _logFileName;
        set => _logFileName = value?.ToLower();
    }

    public Tf2BridgeSettings() { }
}