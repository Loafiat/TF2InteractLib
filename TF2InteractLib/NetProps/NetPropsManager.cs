using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events;
using TF2InteractLib.Types;

namespace TF2InteractLib.NetProps;

public class NetPropsManager
{
    private static string _netPropRegex = @"(?<variable>\S+)\[(?<index>\d+)\]\s+(?<type>\S+)\s+\((?<value>.*)\)";
    
    public static Tf2Player[] CachedPlayerList = new Tf2Player[102];

    public static void Init()
    {
        for (var i = 0; i < CachedPlayerList.Length; i++)
            CachedPlayerList[i] = new Tf2Player();
    }

    public static async Task RefreshNetPropData()
    {
        string g15 = await Tf2Bridge.ExecuteCommand("g15_dumpplayer", true);
        g15 = g15.Trim();
        string[] lines = g15.Split('\n');
        try
        {
            foreach (var line in lines)
            {
                Match match = Regex.Match(line, _netPropRegex);
                if (match.Success)
                {
                    int Index = int.Parse(match.Groups["index"].Value);

                    // Pinky promise you'll use the health variable for only good things? Yeah? Okay thank you :D
                    switch (match.Groups["variable"].Value)
                    {
                        case "m_szName":
                            CachedPlayerList[Index].SteamName = match.Groups["value"].Value;
                            break;
                        case "m_iPing":
                            CachedPlayerList[Index].Ping = int.Parse(match.Groups["value"].Value);
                            break;
                        case "m_iScore":
                            CachedPlayerList[Index].Score = int.Parse(match.Groups["value"].Value);
                            break;
                        case "m_iDeaths":
                            CachedPlayerList[Index].Deaths = int.Parse(match.Groups["value"].Value);
                            break;
                        case "m_bConnected":
                            CachedPlayerList[Index].IsConnected = bool.Parse(match.Groups["value"].Value);
                            break;
                        case "m_iTeam":
                            CachedPlayerList[Index].Team = (Tf2Team)int.Parse(match.Groups["value"].Value);
                            break;
                        case "m_bAlive":
                            CachedPlayerList[Index].Alive = bool.Parse(match.Groups["value"].Value);
                            break;
                        case "m_iHealth":
                            CachedPlayerList[Index].Health = int.Parse(match.Groups["value"].Value);
                            break;
                        case "m_iAccountID":
                            CachedPlayerList[Index].SteamID = int.Parse(match.Groups["value"].Value);
                            break;
                        case "m_bValid":
                            CachedPlayerList[Index].IsValid = bool.Parse(match.Groups["value"].Value);
                            break;
                        case "m_iUserID":
                            CachedPlayerList[Index].UserID = int.Parse(match.Groups["value"].Value);
                            break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Events.ExecuteOnNetPropsUpdated();
    }
}