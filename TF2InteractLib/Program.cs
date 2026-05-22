// See https://aka.ms/new-console-template for more information

#if TestingMode
using TF2InteractLib;

bool success = await TF2DirectAPI.Initialize(true, "/home/lofiat/.local/share/Steam/steamapps/common/Team Fortress 2", "sillyguy123");
if (!success)
{
    Console.WriteLine("Could not connect to server");
    return;
}

StreamReader reader = TF2DirectAPI.GetConsoleStream();

while (true)
{
    string output = reader.ReadToEnd();
    if (output != string.Empty)
        Console.WriteLine(output);
}
#endif