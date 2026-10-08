using SharpHook;
using SharpHook.Data;

namespace TF2InteractLib.Inputs;

// Basically just SharpHook but used specifically to capture inputs going into TF2, use a different hook to capture all
// inputs everywhere if need be. 
public class InputEvents
{
    internal static EventLoopGlobalHook hook;
    
    internal static void Initialize()
    {
        hook = new();
        hook.KeyPressed += (sender, args) =>
        {
            if (Tf2ActivityHandler.IsFocused())
                KeyPressed?.Invoke(args.Data);
        };
        hook.KeyReleased += (sender, args) =>
        {
            if (Tf2ActivityHandler.IsFocused())
                KeyReleased?.Invoke(args.Data);
        };
        hook.MouseClicked += (sender, args) =>
        {
            if (Tf2ActivityHandler.IsFocused())
                MouseClicked?.Invoke(args.Data);
        };
        hook.MouseReleased += (sender, args) =>
        {
            if (Tf2ActivityHandler.IsFocused())
                MouseReleased?.Invoke(args.Data);
        };
        hook.MouseMoved += (sender, args) =>
        {
            if (Tf2ActivityHandler.IsFocused())
                MouseMoved?.Invoke(args.Data);
        };
        hook.MouseMovedRelative += (sender, args) =>
        {
            if (Tf2ActivityHandler.IsFocused())
                MouseMoved?.Invoke(args.Data);
        };
        hook.MouseWheel += (sender, args) =>
        {
            if (Tf2ActivityHandler.IsFocused())
                MouseWheeled?.Invoke(args.Data);
        };
        hook.Run();
    }
    
    public static event Action<KeyboardEventData>? KeyPressed;
    
    public static event Action<KeyboardEventData>? KeyReleased;
    
    public static event Action<MouseEventData>? MouseClicked;
    
    public static event Action<MouseEventData>? MouseReleased;
    
    public static event Action<MouseEventData>? MouseMoved;
    
    public static event Action<MouseWheelEventData>? MouseWheeled;
}