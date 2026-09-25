using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;

namespace TF2InteractLib.Tf2Events.Parsers;

public class ChatParser : IEventParser
{
    public static string chatRegex =
        @"^(?<dead>\*DEAD\*)?\s*(?<team>\(TEAM\))?\s*(?<steam_name>[^:]+?) :  (?<message>.*)$";
    
    public void Parse(string logLine)
    {
        MatchCollection matches = Regex.Matches(logLine, chatRegex);
        if (matches.Count <= 0)
            return;
        ChatMessage message = new ChatMessage();
        foreach (Group group in matches[0].Groups)
        {
            switch (group.Name)
            {
                case "dead":
                    message.isDead = group.Value == "*DEAD*";
                    break;
                case "team":
                    message.isTeam = group.Value == "(TEAM)";
                    break;
                case "steam_name":
                    message.steam_name = group.Value.Trim();
                    break;
                case "message":
                    // trim the EVIL character that fucks up the console
                    message.message = group.Value.Trim();
                    break;
            }
        }
        Events.ExecuteOnMessageSent(message);
    }
}