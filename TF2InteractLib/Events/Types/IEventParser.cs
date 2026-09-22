namespace TF2InteractLib.Events.Types;

public interface IEventParser
{
    public void Parse(string logLine);
}