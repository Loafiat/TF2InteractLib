using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;
using TF2InteractLib.Types;

namespace TF2InteractLib.Tf2Events.Parsers;

public class CaptureParser : IEventParser
{
    public string captureRegex = @"^(?<name>.+?) captured (?<point>.+?) for team #(?<teamnum>\d+)$";
    
    public void Parse(string logLine)
    {
        MatchCollection matches = Regex.Matches(logLine.Trim(), captureRegex);
        if (matches.Count <= 0)
            return;
        CaptureParams cParams = new CaptureParams();
        Match match = matches[0];
        cParams.attackerName = match.Groups["name"].Value;
        cParams.pointName = match.Groups["point"].Value;
        cParams.teamnumber = (Tf2Team)int.Parse(match.Groups["teamnum"].Value);
        Events.ExecuteOnPointCaptured(cParams);
    }
}