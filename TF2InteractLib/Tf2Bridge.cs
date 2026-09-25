using System.Collections.Concurrent;
using RconSharp;
using TF2InteractLib.Events;
using static TF2InteractLib.Tf2BridgeInternals;

namespace TF2InteractLib;

public class Tf2Bridge
{
    private static readonly ConcurrentQueue<Action?> ThreadExecuteQueue = new();
    public static Tf2BridgeSettings Settings { get; private set; }
    public static event Action<string> OnConsoleOutput = EventManager.ParseLogEvent;

    internal static void ExecuteOnConsoleOutput(string eventLog)
    {
        OnConsoleOutput?.Invoke(eventLog);
    } 
    
    public static async Task<bool> Start(Tf2BridgeSettings settings)
    {
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
        _bridgeThread = new Thread(() => BridgeLoop(_bridgeThreadCancellationToken.Token));
        _bridgeThread.Start();
        if (settings.LogFileName != null)
            await Rcon.ExecuteCommandAsync("con_logfile " + settings.LogFileName + ".log");
        _logThread = new Thread(() => LogWatcher.ConsoleWatchLoop(_logThreadCancellationToken.Token))
        {
            IsBackground = true
        };
        _logThread.Start();
        return true;
    }

    public static async Task ExecuteCommand(string command)
    {
        if (Rcon == null)
            return;
        command = command.Trim();
        await Rcon.ExecuteCommandAsync(command);
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
                return;
            while (ThreadExecuteQueue.TryDequeue(out var action))
                action?.Invoke();
        }
    }
}