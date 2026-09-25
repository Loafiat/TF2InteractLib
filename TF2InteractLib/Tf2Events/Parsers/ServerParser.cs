using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;

namespace TF2InteractLib.Tf2Events.Parsers;

public class ServerParser : IEventParser
{
    public string serverRegex =
        @"^Connected to (?<ip>\d{1,3}(?:\.\d{1,3}){3}):(?<port>\d+)[\s\S]*?^Map: (?<map>\S+)\s*$[\s\S]*?^Players: (?<current_players>\d+)\s*\/\s*(?<max_players>\d+)\s*$[\s\S]*?^Server Number: (?<server_number>\d+)\s*$";
    public readonly List<string> Lines = [];
    public int lineNum = 0;
    public bool isReadingLines;
    
    public void Parse(string logLine)
    {
        logLine = logLine.Trim();
        if (isReadingLines)
        {
            Lines.Add(logLine);
            lineNum++;
        }

        if (logLine.StartsWith("Connected to "))
        {
            isReadingLines = true;
            Lines.Add(logLine);
        }
        
        if (logLine.StartsWith("Server Number: "))
        {
            string stringToParse = string.Join("\n", Lines);
            isReadingLines = false;
            lineNum = 0;
            Lines.Clear();
            MatchCollection matches = Regex.Matches(stringToParse.Trim(), serverRegex, RegexOptions.Multiline);
            if (matches.Count <= 0)
                return;
            JoinedServerInfo serverInfo = new JoinedServerInfo();
            Match match = matches[0];
            serverInfo.ipAddress = match.Groups["ip"].Value;
            serverInfo.port = int.Parse(match.Groups["port"].Value);
            serverInfo.map = match.Groups["map"].Value;
            serverInfo.currentPlayerCount = int.Parse(match.Groups["current_players"].Value);
            serverInfo.maxPlayerCount = int.Parse(match.Groups["max_players"].Value);
            serverInfo.serverNumber = int.Parse(match.Groups["server_number"].Value);
            Events.ExecuteOnServerJoined(serverInfo);
        }
    }
}