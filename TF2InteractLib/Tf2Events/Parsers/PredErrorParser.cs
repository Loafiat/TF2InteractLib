using System.Text.RegularExpressions;
using TF2InteractLib.Tf2Events.Types;
using TF2InteractLib.Tf2Events.Types.ParserTypes;

namespace TF2InteractLib.Tf2Events.Parsers;

// requires cl_showerror 2, can get certain detailed info about entities at the cost of an annoying UI element on the right of the screen.
// alternative: spamming NetPropsManager.RefreshNetPropData(), this has a major performance cost lag so cl_showerror is better for computers that can't tank it.
public partial class PredErrorParser : IEventParser
{
    public void Parse(string logLine)
    {
        Match predictMatch = PredRegex().Match(logLine);
        if (predictMatch.Success)
        {
            PredictionError error = new PredictionError();
            error.tick = int.Parse(predictMatch.Groups["tick"].Value);
            error.index = int.Parse(predictMatch.Groups["index"].Value);
            error.property = predictMatch.Groups["property"].Value;
            error.type = predictMatch.Groups["type"].Value;
            if (predictMatch.Groups["net"].Success)
                error.actual = predictMatch.Groups["net"].Value;
            if (predictMatch.Groups["net2"].Success)
                error.actual = predictMatch.Groups["net2"].Value;
            if (predictMatch.Groups["pred"].Success)
                error.actual = predictMatch.Groups["pred"].Value;
            if (predictMatch.Groups["pred2"].Success)
                error.actual = predictMatch.Groups["pred2"].Value;
            error.difference = predictMatch.Groups["diff"].Value;
            Events.ExecuteOnPredictionError(error);
        }
    }

    [GeneratedRegex(@"^\[Tick (?<tick>\d+)\] (?<index>\d+) (?<property>\S+) - (?<type>\S+) differs?(?: \(1st diff\))? (?:\(net (?<net>.+?) (?:- )?pred (?<pred>.+?)\)|\(net\) (?<net2>\S+) \(pred\) (?<pred2>\S+))(?: (?:delta|diff)\((?<diff>[^)]*)\))?$")]
    private static partial Regex PredRegex();
}