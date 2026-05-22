// See https://aka.ms/new-console-template for more information

#if TestingMode
using TF2InteractLib;
using TF2InteractLib.Players;

bool success = await TF2DirectAPI.Initialize(true, "/home/lofiat/.local/share/Steam/steamapps/common/Team Fortress 2", "sillyguy123");
if (!success)
{
    Console.WriteLine("Could not connect to server");
    return;
}
while (true)
{
    //await Task.Delay(200);
    TF2LocalPlayer player = await TF2InteractAPI.GetLocalPlayer();
    Console.WriteLine(player.m_iHealth);
    Console.WriteLine(player.m_iAmmo[1]); // scattergun
}
#endif