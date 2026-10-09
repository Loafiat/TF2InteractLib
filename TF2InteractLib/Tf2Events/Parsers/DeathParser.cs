using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;

namespace TF2InteractLib.Tf2Events.Parsers;

public partial class DeathParser : IEventParser
{
    public void Parse(string logLine)
    {
        Match match = DeathRegex().Match(logLine);
        if (!match.Success)
            return;
        DeathParams dParams = new DeathParams();
        dParams.killerName = match.Groups["killer"].Value;
        dParams.victimName = match.Groups["victim"].Value;
        dParams.weaponName = match.Groups["weapon"].Value;
        dParams.crit = match.Groups["crit"].Success;
        Events.ExecuteOnPlayerDied(dParams);
    }

    [GeneratedRegex(@"^(?<killer>.+?) killed (?<victim>.+?) with (?<weapon>.+?)\.(?: (?<crit>\(crit\)))?$")]
    public static partial Regex DeathRegex();
}