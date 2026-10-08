using TF2InteractLib.Tf2Events.Types;

namespace TF2InteractLib.Tf2Events.Parsers;

public class PlayerConnectParser : IEventParser
{
    public void Parse(string logLine)
    {
        logLine = logLine.Trim();
        if (logLine.EndsWith(" connected"))
        {
            string name = logLine[..^" connected".Length];
            Events.ExecuteOnPlayerConnected(name);
        }
    }
}