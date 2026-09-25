using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;

namespace TF2InteractLib.Tf2Events.Parsers;

public class DeathParser : IEventParser
{
    public string deathRegex = @"^(?<killer>.+?) killed (?<victim>.+?) with (?<weapon>.+?)\.(?: (?<crit>\(crit\)))?$";
    
    public void Parse(string logLine)
    {
        MatchCollection matches = Regex.Matches(logLine.Trim(), deathRegex);
        if (matches.Count <= 0)
            return;
        DeathParams dParams = new DeathParams();
        Match match = matches[0];
        dParams.killerName = match.Groups["killer"].Value;
        dParams.victimName = match.Groups["victim"].Value;
        dParams.weaponName = match.Groups["weapon"].Value;
        dParams.crit = match.Groups["crit"].Success;
        Events.ExecuteOnPlayerDied(dParams);
    }
}