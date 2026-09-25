using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;

namespace TF2InteractLib.Tf2Events.Parsers;

public class SoundParser : IEventParser
{
    public string soundRegex = @"^No caption found for '([^']+)'$";
    
    public void Parse(string logLine)
    {
        MatchCollection matches = Regex.Matches(logLine.Trim(), soundRegex);
        if (matches.Count <= 0)
            return;
        Events.ExecuteOnCaptionSoundPlayed(matches[0].Groups[1].Value);
    }
}