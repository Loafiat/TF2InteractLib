// See https://aka.ms/new-console-template for more information

#if TestingMode
using TF2InteractLib;

bool success = await TF2DirectAPI.Initialize(true, "/home/lofiat/.local/share/Steam/steamapps/common/Team Fortress 2", "sillyguy123");
if (!success)
{
    Console.WriteLine("Could not connect to server");
    return;
}

TF2InteractEvents.PlayerMessage += (sender, msg) =>
{
    Console.WriteLine($"{sender.SteamName} said {msg}");
};

//TF2InteractEvents.PlayerKilled += (_, killArgs) =>
//{
//    Console.Write(killArgs.Victim.SteamName + " died");
//    if (killArgs.Killer != null)
//        Console.Write(" to " + killArgs.Killer.SteamName);
//    if (killArgs.WeaponName != null)
//        Console.Write(", they were using " + killArgs.WeaponName);
//    Console.Write('.');
//    if (killArgs.CriticalKill)
//    {
//        Console.Write(" It was a critical hit.");
//    }
//    Console.WriteLine();
//};

while (true)
{
    
}

#endif