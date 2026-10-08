using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TF2InteractLib.Inputs;

public class Tf2ActivityHandler
{
    // windows only apis
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    public static bool IsFocused()
    {
        if (OperatingSystem.IsWindows())
        {
            IntPtr foregroundWindow = GetForegroundWindow();

            if (foregroundWindow != IntPtr.Zero)
            {
                GetWindowThreadProcessId(foregroundWindow, out uint pid);

                try
                {
                    Process focussedProcess = Process.GetProcessById((int)pid);
                    return focussedProcess.ProcessName is "tf_win64" or "tf";
                }
                catch
                {
                    // ignore since we don't need to do anything if it's an invalid process.
                }
            }
        }
        else if (OperatingSystem.IsLinux())
        {
            Console.WriteLine("Sorry! Linux is currently not supported for inputs.");
            // TODO: Get current active window for x11, plasma, gnome, hyprland, etc. (wayland has no built in way to check so I gotta do a bunch of stuff seperately)
        }
        else
        {
            Console.WriteLine("This platform is currently not supported for inputs.");
            // TODO: figure out what the FUCK to do for these.
        }
        return false;
    }
}