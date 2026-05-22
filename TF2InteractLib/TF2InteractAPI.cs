using System.ComponentModel;
using System.Numerics;
using System.Reflection;
using TF2InteractLib.Players;

namespace TF2InteractLib;

public class TF2InteractAPI
{
    public static string[] PlayerInfoLineMarkers =
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
    
    public static async Task<TF2Player[]> GetPlayerList()
    {
        if (!TF2DirectAPI.Initialized)
        {
            Console.WriteLine("Not initialized couldn't get player list!");
            return null;
        }
        TF2Player[] players = new TF2Player[102];
        string playerDump = await TF2DirectAPI.ExecuteCommand("g15_dumpplayer");
        foreach (string line in playerDump.Split('\n'))
        {
            string operationLine = line.Trim();
            bool foundMarkerForLine = false;
            for (var i = 0; i < PlayerInfoLineMarkers.Length; i++)
            {
                var marker = PlayerInfoLineMarkers[i];
                if (operationLine.Contains(marker))
                {
                    operationLine = operationLine[operationLine.IndexOf(marker)..];
                    operationLine = operationLine.Replace(marker, "");
                    try
                    {
                        int.Parse(operationLine[..operationLine.IndexOf(']')]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(line);
                        throw e;
                    }
                    int playerIndex = int.Parse(operationLine[..operationLine.IndexOf(']')]);
                    operationLine = operationLine[(operationLine.IndexOf('(') + 1)..operationLine.IndexOf(')')];
                    try
                    {
                        switch (PlayerInfoLineMarkers[i])
                        {
                            case "m_szName[":
                                players[playerIndex].SteamName = operationLine;
                                break;
                            case "m_iPing[":
                                players[playerIndex].Ping = int.Parse(operationLine);
                                break;
                            case "m_iScore[":
                                players[playerIndex].Score = int.Parse(operationLine);
                                break;
                            case "m_iDeaths[":
                                players[playerIndex].Deaths = int.Parse(operationLine);
                                break;
                            case "m_bConnected":
                                players[playerIndex].IsConnected = bool.Parse(operationLine);
                                break;
                            case "m_iTeam[":
                                players[playerIndex].Team = (TF2Team)int.Parse(operationLine);
                                break;
                            case "m_bAlive[":
                                players[playerIndex].Alive = bool.Parse(operationLine);
                                break;
                            case "m_iHealth[":
                                players[playerIndex].Health = int.Parse(operationLine);
                                break;
                            case "m_iAccountID[":
                                players[playerIndex].SteamID = int.Parse(operationLine);
                                break;
                            case "m_bValid[":
                                players[playerIndex].IsValid = bool.Parse(operationLine);
                                break;
                            case "m_iUserID[":
                                players[playerIndex].UserID = int.Parse(operationLine);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(playerIndex);
                        throw e;
                    }
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