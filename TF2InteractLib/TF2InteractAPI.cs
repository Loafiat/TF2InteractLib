using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using TF2InteractLib.Events;
using TF2InteractLib.Players;

namespace TF2InteractLib;

public class TF2InteractAPI
{
    // this causes timeouts for some reason
    public static async void EventParser(string newInfo)
    {
        if (newInfo.Contains("suicided") || newInfo.Contains("killed"))
        {
            foreach (string line in newInfo.Split('\n'))
            {
                PlayerKillArgs args = new();
                // What the fuck? Under any other circumstance GetPlayerList works but specifically here the server decides not to respond CONSISTENTLY.
                List<TF2Player> validPlayers = await GetValidPlayerListForEvent();
                string operationLine = line;
                string debugLine = "";
                if (operationLine.EndsWith("(crit)"))
                {
                    operationLine = operationLine.Replace(" (crit)", string.Empty);
                    args.CriticalKill = true;
                }
                if (line.Contains("killed"))
                {
                    foreach (TF2Player player in validPlayers)
                    {
                        if (player.SteamName == null)
                            continue;
                        debugLine = operationLine;
                        if (operationLine.StartsWith(player.SteamName))
                        {
                            args.Killer = player;
                            operationLine = operationLine.Replace(player.SteamName + " killed ", string.Empty);
                            break;
                        }
                    }
                    foreach (TF2Player player in validPlayers)
                    {
                        if (player.SteamName == null)
                            continue;
                        if (operationLine.StartsWith(player.SteamName))
                        {
                            if (player == args.Victim)
                                Console.WriteLine(debugLine);
                            args.Victim = player;
                            operationLine = operationLine.Replace(player.SteamName, string.Empty);
                            break;
                        }
                    }
                    if (operationLine.Contains("with"))
                    {
                        operationLine = operationLine.Replace(" with ", string.Empty);
                        args.WeaponName = operationLine.Replace(".", string.Empty);
                    }
                }
                if (line.Contains("suicided"))
                {
                    foreach (TF2Player player in validPlayers)
                    {
                        if (player.SteamName == null)
                            continue;
                        if (line.StartsWith(player.SteamName))
                            args.Victim = player;
                    }
                }
                if (args.Victim != null)
                    TF2InteractEvents.ExecutePlayerKilled(args);
            }
        }
    }
    
    // All regex taken from MegaAntiCheat
    private static string szNameRegex = @"^m_szName\[(\d+)\]\s+string\s+\((.+)\)$";
    
    private static string iPingRegex = @"^m_iPing\[(\d+)\]\s+integer\s+\((\d+)\)$";
    
    private static string iScoreRegex = @"^m_iScore\[(\d+)\]\s+integer\s+\((\d+)\)$";
    
    private static string iDeathsRegex = @"^m_iDeaths\[(\d+)\]\s+integer\s+\((\d+)\)$";
    
    private static string bConnectedRegex = @"^m_bConnected\[(\d+)\]\s+bool\s+\((false|true)\)$";
    
    private static string iTeamRegex = @"^m_iTeam\[(\d+)\]\s+integer\s+\(([0-3])\)$";
    
    private static string bAliveRegex = @"^m_bAlive\[(\d+)\]\s+bool\s+\((false|true)\)$";

    private static string iHealthRegex = @"^m_iHealth\[(\d+)\]\s+integer\s+\((\d+)\)$";

    private static string iAccountIDRegex = @"^m_iAccountID\[(\d+)\]\s+integer\s+\((\d{4,})\)$";

    private static string bValidRegex = @"^m_bValid\[(\d+)\]\s+bool\s+\((false|true)\)$";

    private static string iUserIDRegex = @"^m_iUserID\[(\d+)\]\s+integer\s+\((\d+)\)$";
    
    private static string[] PlayerInfoLineMarkers =
    [
        "m_szName[",
        "m_iPing[",
        "m_iScore[",
        "m_iDeaths[",
        "m_bConnected[",
        "m_iTeam[",
        "m_bAlive[",
        "m_iHealth[",
        "m_iAccountID[",
        "m_bValid[",
        "m_iUserID["
    ];

    private static Dictionary<string, string> MarkerToRegexTable = new()
    {
        { "m_szName[", szNameRegex },
        { "m_iPing[", iPingRegex },
        { "m_iScore[", iScoreRegex },
        { "m_iDeaths[", iDeathsRegex },
        { "m_bConnected[", bConnectedRegex },
        { "m_iTeam[", iTeamRegex },
        { "m_bAlive[", bAliveRegex },
        { "m_iHealth[", iHealthRegex },
        { "m_iAccountID[", iAccountIDRegex },
        { "m_bValid[", bValidRegex },
        { "m_iUserID[", iUserIDRegex }
    };

    private static Dictionary<string, Type> MarkerToTypeTable = new()
    {
        { "m_szName[", typeof(string) },
        { "m_iPing[", typeof(int) },
        { "m_iScore[", typeof(int) },
        { "m_iDeaths[", typeof(int) },
        { "m_bConnected[", typeof(bool) },
        { "m_iTeam[", typeof(int) },
        { "m_bAlive[", typeof(bool) },
        { "m_iHealth[", typeof(int) },
        { "m_iAccountID[", typeof(int) },
        { "m_bValid[", typeof(bool) },
        { "m_iUserID[", typeof(int) }
    };

    private static Dictionary<string, string> MarkerToPlayerPropertyTable = new()
    {
        { "m_szName[", "SteamName" },
        { "m_iPing[", "Ping" },
        { "m_iScore[", "Score" },
        { "m_iDeaths[", "Deaths" },
        { "m_bConnected[", "IsConnected" },
        { "m_iTeam[", "Team" },
        { "m_bAlive[", "Alive" },
        { "m_iHealth[", "Health" },
        { "m_iAccountID[", "SteamID" },
        { "m_bValid[", "IsValid" },
        { "m_iUserID[", "UserID" }
    };

    private static async Task<List<TF2Player>> GetValidPlayerListForEvent()
    {
        TF2Player[] players = new TF2Player[102];
        for (int i = 0; i < players.Length; i++)
            players[i] = new TF2Player();
        
        string playerDump = await TF2DirectAPI.EventRConClient!.SendCommandAsync("g15_dumpplayer");
        List<string> splitted = playerDump.Split('\n').ToList();
        
        // needs to be a seperate numerator to operate on list while looping
        for (var i = 0; i < splitted.Count; i++)
        {
            if (splitted[i].Count(x => x == ')') > 1)
            {
                int firstParthesis = splitted[i].IndexOf(')')+1;
                splitted[i] = splitted[i][..firstParthesis];
                splitted.Add(splitted[i][firstParthesis..]);
            }
        }
        
        foreach (string line in splitted)
        {
            for (var i = 0; i < PlayerInfoLineMarkers.Length; i++)
            {
                var marker = PlayerInfoLineMarkers[i];
                if (line.Contains(marker))
                {
                    Match regMatch = Regex.Match(line, MarkerToRegexTable[marker]);
                    if (!regMatch.Success)
                        continue;
                    int index = int.Parse(regMatch.Groups[1].Value);
                    object value = ArbitraryParser.Parse(MarkerToTypeTable[marker], regMatch.Groups[2].Value);
                    TF2Player player = players[index];
                    PropertyInfo? prop = typeof(TF2Player).GetProperty(MarkerToPlayerPropertyTable[marker]);
                    // This shit is why TF2Player isn't a struct. Why dotnet, why?
                    prop.SetValue(player, value);
                }
            }
        }

        List<TF2Player> validList = new();
        foreach (TF2Player player in players)
        {
            if (player.IsValid)
                validList.Add(player);
        }
        
        return validList;
    }
    
    public static async Task<TF2Player[]> GetPlayerList()
    {
        if (!TF2DirectAPI.Initialized)
        {
            Console.WriteLine("Not initialized couldn't get player list!");
            return null;
        }

        TF2Player[] players = new TF2Player[102];
        for (var i = 0; i < players.Length; i++)
            players[i] = new TF2Player();

        string playerDump = await TF2DirectAPI.ExecuteCommand("g15_dumpplayer");
        List<string> splitted = playerDump.Split('\n').ToList();
        
        // needs to be a seperate numerator to operate on list while looping
        for (var i = 0; i < splitted.Count; i++)
        {
            if (splitted[i].Count(x => x == ')') > 1)
            {
                int firstParthesis = splitted[i].IndexOf(')')+1;
                splitted[i] = splitted[i][..firstParthesis];
                splitted.Add(splitted[i][firstParthesis..]);
            }
        }

        foreach (string line in splitted)
        {
            for (var i = 0; i < PlayerInfoLineMarkers.Length; i++)
            {
                var marker = PlayerInfoLineMarkers[i];
                if (line.Contains(marker))
                {
                    Match regMatch = Regex.Match(line, MarkerToRegexTable[marker]);
                    if (!regMatch.Success)
                        continue;
                    int index = int.Parse(regMatch.Groups[1].Value);
                    object value = ArbitraryParser.Parse(MarkerToTypeTable[marker], regMatch.Groups[2].Value);
                    TF2Player player = players[index];
                    PropertyInfo? prop = typeof(TF2Player).GetProperty(MarkerToPlayerPropertyTable[marker]);
                    // This shit is why TF2Player isn't a struct. Why dotnet, why?
                    prop?.SetValue(player, value);
                }
            }
        }
        return players;
    }

    internal static string playerDump = string.Empty;
    internal static string currentLine = string.Empty;
    
    public static async Task<TF2LocalPlayer> GetLocalPlayer()
    {
        TF2LocalPlayer localPlayer = new TF2LocalPlayer();
        playerDump = await TF2DirectAPI.ExecuteCommand("g15_dumpplayer");
        foreach (string line in playerDump.Split('\n'))
        {
            currentLine = line;
            foreach (PropertyInfo prop in typeof(TF2LocalPlayer).GetProperties())
            {
                if (prop.Name != "m_Shared" && prop.Name != "m_Local" && prop.Name != "m_Collision")
                { 
                    if (line.Contains(prop.Name))
                    {
                        string operationLine = line.Substring(line.IndexOf(prop.Name, StringComparison.Ordinal));
                        if (prop.PropertyType.IsArray)
                        {
                            string valueLine = operationLine;
                            valueLine = valueLine[(valueLine.IndexOf('(') + 1)..valueLine.IndexOf(')')];
                            object valueParsed = ArbitraryParser.Parse(prop.PropertyType, valueLine);
                            string indexLine = operationLine.Substring(operationLine.IndexOf('[') + 1);
                            indexLine = indexLine.Substring(0, indexLine.IndexOf(']'));
                            int index = int.Parse(indexLine);
                            ((Array)prop.GetValue(localPlayer)).SetValue(valueParsed, index);
                        }
                        else
                        {
                            // ensure not an array since it shouldn't be
                            if (!line.Contains(prop.Name + " "))
                                continue;
                            string operationLine2 = operationLine;
                            operationLine2 = operationLine2[(operationLine2.IndexOf('(') + 1)..operationLine2.IndexOf(')')];
                            object valueParsed = ArbitraryParser.Parse(prop.PropertyType, operationLine2);
                            prop.SetValue(localPlayer, valueParsed);
                        }
                    }
                }
            }
            foreach (PropertyInfo prop in typeof(TF2SharedPlayer).GetProperties())
            {
                if (line.Contains("m_Shared." + prop.Name))
                {
                    string operationLine = line.Substring(line.IndexOf("m_Shared." + prop.Name, StringComparison.Ordinal));
                    if (prop.PropertyType.IsArray)
                    {
                        string valueLine = line;
                        valueLine = valueLine[(valueLine.IndexOf('(') + 1)..valueLine.IndexOf(')')];
                        object valueParsed = ArbitraryParser.Parse(prop.PropertyType, valueLine);
                        string indexLine = line.Substring(line.IndexOf('[') + 1);
                        indexLine = indexLine.Substring(0, indexLine.IndexOf(']'));
                        int index = int.Parse(indexLine);
                        ((Array)prop.GetValue(localPlayer.m_Shared)).SetValue(valueParsed, index);
                    }
                    else
                    {
                        // ensure not an array since it shouldn't be
                        if (!line.Contains(prop.Name + " "))
                            continue;
                        string operationLine2 = operationLine;
                        operationLine2 = operationLine2[(operationLine2.IndexOf('(') + 1)..operationLine2.IndexOf(')')];
                        object valueParsed = ArbitraryParser.Parse(prop.PropertyType, operationLine2);
                        prop.SetValue(localPlayer.m_Shared, valueParsed);
                    }
                }
            }
            foreach (PropertyInfo prop in typeof(TF2LocalPlayer).GetProperties())
            {
                if (line.Contains("m_Local." + prop.Name))
                {
                    if (prop.PropertyType.IsArray)
                    {
                        string valueLine = line;
                        valueLine = valueLine[(valueLine.IndexOf('(') + 1)..valueLine.IndexOf(')')];
                        object valueParsed = ArbitraryParser.Parse(prop.PropertyType, valueLine);
                        string indexLine = line.Substring(line.IndexOf('[') + 1);
                        indexLine = indexLine.Substring(0, indexLine.IndexOf(']'));
                        int index = int.Parse(indexLine);
                        ((Array)prop.GetValue(localPlayer.m_Local)).SetValue(valueParsed, index);
                    }
                    else
                    {
                        // ensure not an array since it shouldn't be
                        if (!line.Contains(prop.Name + " "))
                            continue;
                        string operationLine = line;
                        operationLine = operationLine[(operationLine.IndexOf('(') + 1)..operationLine.IndexOf(')')];
                        object valueParsed = ArbitraryParser.Parse(prop.PropertyType, operationLine);
                        prop.SetValue(localPlayer.m_Local, valueParsed);
                    }
                }
            }
            foreach (PropertyInfo prop in typeof(TF2PlayerCollision).GetProperties())
            {
                if (line.Contains("m_Collision." + prop.Name))
                {
                    string operationLine =
                        line.Substring(line.IndexOf("m_Collision." + prop.Name, StringComparison.Ordinal));
                    if (prop.PropertyType.IsArray)
                    {
                        string valueLine = line;
                        valueLine = valueLine[(valueLine.IndexOf('(') + 1)..valueLine.IndexOf(')')];
                        object valueParsed = ArbitraryParser.Parse(prop.PropertyType, valueLine);
                        string indexLine = line.Substring(line.IndexOf('[') + 1);
                        indexLine = indexLine.Substring(0, indexLine.IndexOf(']'));
                        int index = int.Parse(indexLine);
                        ((Array)prop.GetValue(localPlayer.m_Collision)).SetValue(valueParsed, index);
                    }
                    else
                    {
                        // ensure not an array since it shouldn't be
                        if (!line.Contains(prop.Name + " "))
                            continue;
                        string operationLine2 = operationLine;
                        operationLine2 = operationLine2[(operationLine2.IndexOf('(') + 1)..operationLine2.IndexOf(')')];
                        object valueParsed = ArbitraryParser.Parse(prop.PropertyType, operationLine2);
                        prop.SetValue(localPlayer.m_Collision, valueParsed);
                    }
                }
            }
        }
        return localPlayer;
    }
}

public static class ArbitraryParser
{
    public static object? Parse(Type type, string value)
    {
        try
        {
            if (type == typeof(string))
                return value;
            
            if (type.IsArray)
                type = type.GetElementType();

            try
            {
                if (type == typeof(Vector3))
                {
                    string[] values = value.Split(' ');
                    return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
                }
            }
            catch
            {
                return new Vector3();
            }
        
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.IsEnum)
                return Enum.Parse(type, value, true);

            MethodInfo? parse = type.GetMethod(
                "Parse",
                BindingFlags.Public | BindingFlags.Static,
                new[] { typeof(string) });

            if (parse != null)
                return parse.Invoke(null, new object[] { value });

            return Convert.ChangeType(value, type);
        }
        catch (Exception e)
        {
            Console.WriteLine(TF2InteractAPI.playerDump);
            Console.WriteLine(TF2InteractAPI.currentLine);
            throw e;
        }
    }
}