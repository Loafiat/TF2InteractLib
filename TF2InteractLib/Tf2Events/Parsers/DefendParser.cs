using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;
using TF2InteractLib.Types;

namespace TF2InteractLib.Tf2Events.Parsers;

public class DefendParser : IEventParser
{
    public string defenseRegex = @"^(?<name>.+?) defended (?<point>.+?) for team #(?<teamnum>\d+)$";
    
    public void Parse(string logLine)
    {
        MatchCollection matches = Regex.Matches(logLine.Trim(), defenseRegex);
        if (matches.Count <= 0)
            return;
        DefenseParams dParams = new DefenseParams();
        Match match = matches[0];
        dParams.defenderName = match.Groups["name"].Value;
        dParams.pointName = match.Groups["point"].Value;
        dParams.teamnumber = (Tf2Team)int.Parse(match.Groups["teamnum"].Value);
        Events.ExecuteOnPointDefended(dParams);
    }
}