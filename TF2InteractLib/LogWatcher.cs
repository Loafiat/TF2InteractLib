using TF2InteractLib.Tf2Events;

namespace TF2InteractLib;

public class LogWatcher
{
    public static async void ConsoleWatchLoop(CancellationToken token)
    {
        try
        {
            if (Tf2Bridge.Settings.Tf2Path == null)
                return;
            string filePath = Path.Combine(Tf2Bridge.Settings.Tf2Path, "tf", Tf2Bridge.Settings.LogFileName + ".log");
            
            // wait for file
            while (!File.Exists(filePath))
            {
                if (token.IsCancellationRequested)
                    return;
            }
            
            await using var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fs);
            await reader.ReadToEndAsync();
            string output = string.Empty;
            while (true)
            {
                if (token.IsCancellationRequested)
                    return;
                output += await reader.ReadToEndAsync();
                while (output.Contains('\n'))
                {
                    int newLineIndex = output.IndexOf('\n');
                    string eventLog = output[..newLineIndex];
                    output = output[(newLineIndex + 1)..];
                    Tf2Bridge.ExecuteOnBridgeThread(() =>
                    {
                        Events.ExecuteOnConsoleOutput(eventLog);
                    });
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}