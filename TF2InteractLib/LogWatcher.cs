using System.Text;
using TF2InteractLib.Tf2Events;

namespace TF2InteractLib;

public class LogWatcher
{
    public static async Task ConsoleWatchLoop(CancellationToken token)
    {
        if (Tf2Bridge.Settings.Tf2Path == null)
            return;
        string filePath = Path.Combine(Tf2Bridge.Settings.Tf2Path, "tf", Tf2Bridge.Settings.LogFileName + ".log");

        try
        {
            while (!File.Exists(filePath))
                await Task.Delay(100, token);

            await using var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var reader = new StreamReader(fs);
            await reader.ReadToEndAsync(token);
            
            // decided while I'm here to change this to a string builder.
            StringBuilder output = new StringBuilder();
            while (!token.IsCancellationRequested)
            {
                string chunk = await reader.ReadToEndAsync(token);
                if (chunk.Length == 0)
                {
                    await Task.Delay(1, token);
                    continue;
                }
                
                output.Append(chunk);
                string text = output.ToString();
                int start = 0;
                int nl;
                while ((nl = text.IndexOf('\n', start)) >= 0)
                {
                    string line = text[start..nl];
                    start = nl + 1;
                    Tf2Bridge.ExecuteOnBridgeThread(() => Events.ExecuteOnConsoleOutput(line));
                }
                output.Clear().Append(text, start, text.Length - start);
            }
        }
        catch (OperationCanceledException) { }
    }
}