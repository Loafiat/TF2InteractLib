using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using RconSharp;
using TF2InteractLib.NetProps;
using static TF2InteractLib.Tf2BridgeInternals;

namespace TF2InteractLib;

public partial class Tf2Bridge
{
    public static Tf2BridgeSettings Settings { get; private set; }
    private static readonly ConcurrentQueue<Action?> ThreadExecuteQueue = new();
    public static event Action? OnBridgeThreadStop;
    
    public static async Task<bool> Start(Tf2BridgeSettings settings)
    {
        NetPropsManager.Init();
        if (settings.RconPassword == null)
        {
            Console.WriteLine("Password required for client rcon to work!");
            return false;
        }
        Settings = settings;
        Rcon = RconClient.Create(settings.RconHost, settings.RconPort);
        await Rcon.ConnectAsync();
        if (!await Rcon.AuthenticateAsync(settings.RconPassword))
            return false;
        _bridgeThread = new Thread(() => BridgeLoop(_bridgeCancellationToken.Token));
        _bridgeThread.Start();
        if (settings.LogFileName != null)
            await Rcon.ExecuteCommandAsync("con_logfile " + settings.LogFileName + ".log");
        _logThread = new Thread(() => LogWatcher.ConsoleWatchLoop(_bridgeCancellationToken.Token))
        {
            IsBackground = true
        };
        _logThread.Start();
        return true;
    }

    public static async Task<string> ExecuteCommand(string command, bool multipacket = false)
    {
        if (Rcon == null)
            return string.Empty;
        command = command.Trim();
        return await Rcon.ExecuteCommandAsync(command, multipacket);
    }
    
    public static async Task<string?> GetConvar(string key)
    {
        if (Rcon == null)
            return string.Empty;
        var result = await Rcon.ExecuteCommandAsync(key);
        var match = ConvarRegex().Match(result);
        if (!match.Success)
            return null;
        return match.Groups["key"].Value == key ? match.Groups["value"].Value : null;
    }

    public static void ExecuteOnBridgeThread(Action action)
    {
        ThreadExecuteQueue.Enqueue(action);
    }

    private static void BridgeLoop(CancellationToken token)
    {
        while (true)
        {
            if (token.IsCancellationRequested)
            {
                OnBridgeThreadStop?.Invoke();
                return;
            }
            while (ThreadExecuteQueue.TryDequeue(out var action))
                action?.Invoke();
        }
    }

    [GeneratedRegex("""(?<key>[^"]+)"\s*=\s*"(?<value>[^"]*)""", RegexOptions.Multiline)]
    private static partial Regex ConvarRegex();
}