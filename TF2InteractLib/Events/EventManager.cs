using TF2InteractLib.Events.Parsers;
using TF2InteractLib.Events.Types;

namespace TF2InteractLib.Events;

public class EventManager
{
    public static List<IEventParser> Parsers = 
    [
        new ChatParser()
    ];
    
    public static void ParseLogEvent(string logLine)
    {
        foreach (var parser in Parsers)
            parser.Parse(logLine);
    }
}