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
    await Task.Delay(1000);
    if (!(await TF2DirectAPI.GetBindValue("w")).Contains("echo"))
        await TF2DirectAPI.SetBindValue("w", await TF2DirectAPI.GetBindValue("w") + ";echo");
}
#endif