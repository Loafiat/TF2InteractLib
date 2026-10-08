using TF2InteractLib.Tf2Events.Types;

namespace TF2InteractLib.Tf2Events.Parsers;

public class UnknownCommandParser : IEventParser
{
    public void Parse(string logLine)
    {
        logLine = logLine.Trim();
        if (logLine.StartsWith("Unknown command: "))
        {
            logLine = logLine["Unknown command: ".Length..];
            Events.ExecuteOnUnknownCommand(logLine);
        }
    }
}