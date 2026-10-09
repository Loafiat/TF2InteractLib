using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;

namespace TF2InteractLib.Tf2Events.Parsers;

public partial class SoundParser : IEventParser
{
    public string soundRegex = @"";
    
    public void Parse(string logLine)
    {
        Match match = SoundRegex().Match(logLine);
        if (!match.Success)
            return;
        Events.ExecuteOnCaptionSoundPlayed(match.Groups[1].Value);
    }
    
    [GeneratedRegex("^No caption found for '([^']+)'$")]
    public static partial Regex SoundRegex();
}