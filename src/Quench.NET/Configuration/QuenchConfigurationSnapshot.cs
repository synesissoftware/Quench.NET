// Created: 19th September 2026
// Updated: 19th September 2026

using Quench.Internal;

namespace Quench.Configuration;

internal sealed class QuenchConfigurationSnapshot
{
    internal static QuenchConfigurationSnapshot DefaultThrow { get; } =
        new(QuenchAction.Throw, new Dictionary<ExceptionNamesKey, QuenchAction>());

    internal QuenchConfigurationSnapshot(
        QuenchAction defaultAction,
        Dictionary<ExceptionNamesKey, QuenchAction> rulesByNames)
    {
        DefaultAction = defaultAction;
        RulesByNames = rulesByNames;
    }

    internal QuenchAction DefaultAction { get; }

    internal Dictionary<ExceptionNamesKey, QuenchAction> RulesByNames { get; }
}
