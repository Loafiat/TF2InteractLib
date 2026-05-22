using System.Net;
using CoreRCON;

namespace TF2InteractLib;

public class TF2DirectAPI
{
    public static bool RCONEnabled { get; private set; }
    public static bool Initialized { get; private set; }
    internal static RCON? RConClient = null;
    internal static StreamReader? ConsoleOutputInternal = null;
    public static string ConsoleOutput { get; private set; }
    
    public static async Task<bool> Initialize(bool allowLogging, string tf2Dir = "C:/Program Files (x86)/Steam/steamapps/common/Team Fortress 2", string? rconPassword = null)
    {
        RCONEnabled = true;
        if (rconPassword == null)
        {
            if (allowLogging)
                Console.WriteLine("RCON disabled, functionality will be limited!");
            RCONEnabled = false;
        }
        RConClient = new RCON(IPAddress.Loopback, 27015, rconPassword, sourceMultiPacketSupport:true);
        await RConClient.ConnectAsync();
        await RConClient.AuthenticateAsync();
        if (!RConClient.Authenticated)
            return false;
        await RConClient.SendCommandAsync("con_logfile TF2Output.log");
        Initialized = true;
        bool initializedLog = InitializeLogFile(tf2Dir);
        if (!initializedLog)
        {
            Console.WriteLine("no init log");
            return false;
        }
        Task.Run(OutputMainLoop);
        return true;
    }

    public static async Task OutputMainLoop()
    {
        if (!Initialized)
            return;
        await ConsoleOutputInternal!.ReadToEndAsync();
        string lineBuffer = string.Empty;
        while (true)
        {
            lineBuffer += await ConsoleOutputInternal.ReadToEndAsync();
            if (lineBuffer.Contains("\n"))
            {
                ConsoleOutput += lineBuffer;
                lineBuffer = string.Empty;
            }
        }
    }

    private static bool InitializeLogFile(string tf2Dir)
    {
        if (!Initialized)
            return false;
        ConsoleOutputInternal = File.OpenText(Path.Combine(tf2Dir, "tf", "tf2output.log"));
        return true;
    }

    public static async Task<string> ExecuteCommand(string command)
    {
        if (!Initialized)
            return "Uninitialized";
        try
        {
            string output = await RConClient!.SendCommandAsync(command);
            ConsoleOutput += output;
            return output;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return string.Empty;
        }
    }

    public static async Task<string> GetConVarValue(string cvarName)
    {
        string output = await ExecuteCommand(cvarName);
        while (output == string.Empty)
            output = await ExecuteCommand(cvarName);
        output = output.Replace($"\"{cvarName}\" = ", string.Empty);
        output = output.Substring(output.IndexOf('"') + 1);
        output = output.Substring(0, output.IndexOf('"'));
        return output;
    }

    public static async Task SetConVarValue(string cvarName, string value)
    {
        await ExecuteCommand(cvarName + " " + value);
    }

    public static async Task<string> GetBindValue(string bindKey)
    {
        string output = await ExecuteCommand("bind " + bindKey);
        while (output == string.Empty)
            output = await ExecuteCommand("bind " + bindKey);
        output = output.Replace($"\"{bindKey}\" = \"", string.Empty);
        output = output[..^1];
        return output;
    }

    public static async Task SetBindValue(string bindKey, string value)
    {
        await ExecuteCommand("bind " + bindKey + " \"" + value + '\"');
    }
}