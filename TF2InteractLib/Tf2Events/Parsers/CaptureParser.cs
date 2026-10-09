using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;
using TF2InteractLib.Types;

namespace TF2InteractLib.Tf2Events.Parsers;

public partial class CaptureParser : IEventParser
{
    public void Parse(string logLine)
    {
        Match match = CaptureRegex().Match(logLine);
        if (!match.Success)
            return;
        CaptureParams cParams = new CaptureParams();
        cParams.attackerName = match.Groups["name"].Value;
        cParams.pointName = match.Groups["point"].Value;
        cParams.teamnumber = (Tf2Team)int.Parse(match.Groups["teamnum"].Value);
        Events.ExecuteOnPointCaptured(cParams);
    }

    [GeneratedRegex(@"^(?<name>.+?) captured (?<point>.+?) for team #(?<teamnum>\d+)$")]
    private static partial Regex CaptureRegex();
}