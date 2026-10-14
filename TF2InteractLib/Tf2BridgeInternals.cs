using RconSharp;

namespace TF2InteractLib;

public static class Tf2BridgeInternals
{
    public static CancellationTokenSource _bridgeCancellationToken = new();
    public static Thread? _bridgeThread;
    public static Thread? _logThread;
    public static RconClient? Rcon;
}