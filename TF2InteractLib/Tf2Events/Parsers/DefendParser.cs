using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;
using TF2InteractLib.Types;

namespace TF2InteractLib.Tf2Events.Parsers;

public partial class DefendParser : IEventParser
{
    public void Parse(string logLine)
    {
        Match match = DefenseRegex().Match(logLine);
        if (!match.Success)
            return;
        DefenseParams dParams = new DefenseParams();
        dParams.defenderName = match.Groups["name"].Value;
        dParams.pointName = match.Groups["point"].Value;
        dParams.teamnumber = (Tf2Team)int.Parse(match.Groups["teamnum"].Value);
        Events.ExecuteOnPointDefended(dParams);
    }

    [GeneratedRegex(@"^(?<name>.+?) defended (?<point>.+?) for team #(?<teamnum>\d+)$")]
    public static partial Regex DefenseRegex();
}