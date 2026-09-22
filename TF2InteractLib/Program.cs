using System.Text;
using TF2InteractLib;
using TF2InteractLib.Events;

if  (!await Tf2Bridge.Start
     (
         new Tf2BridgeSettings
         {
             RconPassword = "sillyguy123",
             Tf2Path = @"C:\Program Files (x86)\Steam\steamapps\common\Team Fortress 2",
             LogFileName = "Tf2LogRead"
         }
     )
    )
{
    Console.WriteLine("Tf2Bridge failed");
    return int.MinValue;
}

Events.OnMessageSent += message =>
{
    StringBuilder sb = new();
    sb.Append(message.steam_name);
    sb.Append(" said \"");
    sb.Append(message.message + "\"");
    if (message.isTeam)
        sb.Append(" in team chat");
    if (message.isDead)
        sb.Append(" while dead");
    sb.Append('.');
    
    Console.WriteLine(sb.ToString());
};

while (true)
{
    
}

return 0;