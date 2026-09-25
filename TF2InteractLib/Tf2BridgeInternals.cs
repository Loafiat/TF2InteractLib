using RconSharp;

namespace TF2InteractLib;

public static class Tf2BridgeInternals
{
    public static Thread? _bridgeThread;
    public static CancellationTokenSource _bridgeThreadCancellationToken = new();
    public static Thread? _logThread;
    public static CancellationTokenSource _logThreadCancellationToken = new();
    public static RconClient? Rcon;
}