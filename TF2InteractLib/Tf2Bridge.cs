using System.Collections.Concurrent;
using RconSharp;
using TF2InteractLib.Events;

namespace TF2InteractLib;

public class Tf2Bridge
{
    private static readonly ConcurrentQueue<Action?> ThreadExecuteQueue = new();
    private static Thread? _bridgeThread;
    private static Thread? _logThread;
    private static RconClient? _client;
    public static Tf2BridgeSettings Settings { get; private set; }

    public static event Action<string> OnConsoleOutput = EventManager.ParseLogEvent;

    internal static void ExecuteOnConsoleOutput(string eventLog)
    {
        OnConsoleOutput?.Invoke(eventLog);
    } 
    
    public static async Task<bool> Start(Tf2BridgeSettings settings)
    {
        Settings = settings;
        _client = RconClient.Create(settings.RconHost, settings.RconPort);
        await _client.ConnectAsync();
        if (settings.RconPassword != null)
            if (!await _client.AuthenticateAsync(settings.RconPassword))
                return false;
        _bridgeThread = new Thread(BridgeLoop);
        _bridgeThread.Start();
        if (settings.LogFileName != null)
            await _client.ExecuteCommandAsync("con_logfile " + settings.LogFileName + ".log");
        _logThread = new Thread(LogWatcher.ConsoleWatchLoop)
        {
            IsBackground = true
        };
        _logThread.Start();
        return true;
    }

    public static async Task ExecuteCommand(string command)
    {
        if (_client == null)
            return;
        command = command.Trim();
        await _client.ExecuteCommandAsync(command);
    }

    public static void ExecuteOnBridgeThread(Action action)
    {
        ThreadExecuteQueue.Enqueue(action);
    }

    private static void BridgeLoop()
    {
        while (true)
        {
            while (ThreadExecuteQueue.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }
    }
}