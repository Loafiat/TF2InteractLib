using TF2InteractLib.Tf2Events.Parsers;
using TF2InteractLib.Tf2Events.Types;

namespace TF2InteractLib.Tf2Events;

public class EventManager
{
    public static List<IEventParser> Parsers = 
    [
        new ChatParser(),
        new DeathParser(),
        new DefendParser(),
        new CaptureParser(),
        new SoundParser(),
        new ServerParser(),
        new ConnectionEvent()
    ];
    
    public static void ParseLogEvent(string logLine)
    {
        foreach (var parser in Parsers)
            parser.Parse(logLine);
    }
}