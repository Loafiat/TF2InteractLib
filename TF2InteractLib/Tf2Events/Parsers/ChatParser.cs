using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;

namespace TF2InteractLib.Tf2Events.Parsers;

public partial class ChatParser : IEventParser
{
    public void Parse(string logLine)
    {
        Match match = ChatRegex().Match(logLine);
        if (!match.Success)
            return;
        ChatMessage message = new ChatMessage();
        message.isDead = match.Groups["dead"].Value == "*DEAD*";
        message.isTeam = match.Groups["team"].Value == "(TEAM)";
        message.steam_name = match.Groups["steam_name"].Value;
        message.message = match.Groups["message"].Value;
        Events.ExecuteOnMessageSent(message);
    }

    [GeneratedRegex(@"^(?<dead>\*DEAD\*)?\s*(?<team>\(TEAM\))?\s*(?<steam_name>[^:]+?) :  (?<message>.*)$")]
    public static partial Regex ChatRegex();
}