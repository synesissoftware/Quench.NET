// Created: 22nd July 2013
// Updated: 19th September 2026

using Quench.Configuration;
using Quench.Diagnostics;
using Quench.Internal;

using System.Diagnostics;

namespace Quench;

/// <summary>
///  Public API class for Quench.
/// </summary>
public static class Core
{
    /// <summary>
    ///  Indicates whether a rule names this exception type and catching
    ///  type exactly, without inheritance walk.
    /// </summary>
    /// <param name="exceptionType">
    ///  The exception type to look up. May not be
    ///  <see langword="null"/>.
    /// </param>
    /// <param name="catchingType">
    ///  The catching type, or <see langword="null"/> for an
    ///  unqualified rule.
    /// </param>
    /// <returns>
    ///  <see langword="true"/> if a precise rule exists; otherwise
    ///  <see langword="false"/>.
    /// </returns>
    [DebuggerStepThrough]
    public static bool IsPreciselySpecified(
        Type exceptionType,
        Type? catchingType)
    {
        if (exceptionType is null)
        {
            throw new ArgumentNullException(nameof(exceptionType));
        }

        return Arbitrator.IsPreciselySpecified(exceptionType, catchingType);
    }

    /// <summary>
    ///  Indicates that Quench deems that the given exception may be
    ///  quenched.
    /// </summary>
    /// <param name="x">The exception to arbitrate over.</param>
    /// <returns>
    ///  <see langword="true"/> if the exception may be quenched;
    ///  otherwise <see langword="false"/>.
    /// </returns>
    [DebuggerStepThrough]
    public static bool MayBeQuenched(Exception x)
    {
        return !Arbitrator.MustBeRethrown(x);
    }

    /// <summary>
    ///  Indicates that Quench deems that the given exception may be
    ///  quenched when caught in the context of the given type.
    /// </summary>
    /// <param name="x">The exception to arbitrate over.</param>
    /// <param name="catchingType">
    ///  The type in whose context the exception is caught.
    /// </param>
    /// <returns>
    ///  <see langword="true"/> if the exception may be quenched;
    ///  otherwise <see langword="false"/>.
    /// </returns>
    [DebuggerStepThrough]
    public static bool MayBeQuenched(Exception x, Type? catchingType)
    {
        return !Arbitrator.MustBeRethrown(x, catchingType);
    }

    /// <summary>
    ///  Indicates that Quench deems that the given exception must be
    ///  rethrown.
    /// </summary>
    /// <param name="x">The exception to arbitrate over.</param>
    /// <returns>
    ///  <see langword="true"/> if the exception must be rethrown;
    ///  otherwise <see langword="false"/>.
    /// </returns>
    [DebuggerStepThrough]
    public static bool MustBeRethrown(Exception x)
    {
        return Arbitrator.MustBeRethrown(x);
    }

    /// <summary>
    ///  Indicates that Quench deems that the given exception must be
    ///  rethrown when caught in the context of the given type.
    /// </summary>
    /// <param name="x">The exception to arbitrate over.</param>
    /// <param name="catchingType">
    ///  The type in whose context the exception is caught.
    /// </param>
    /// <returns>
    ///  <see langword="true"/> if the exception must be rethrown;
    ///  otherwise <see langword="false"/>.
    /// </returns>
    [DebuggerStepThrough]
    public static bool MustBeRethrown(Exception x, Type? catchingType)
    {
        return Arbitrator.MustBeRethrown(x, catchingType);
    }

    /// <summary>
    ///  Replaces process-global quench rules. With no configuration
    ///  the default action is <see cref="QuenchAction.Throw"/>.
    /// </summary>
    /// <param name="configure">
    ///  Builder callback. May not be <see langword="null"/>.
    /// </param>
    public static void Configure(Action<QuenchConfigurationBuilder> configure)
    {
        if (configure is null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        QuenchConfigurationBuilder builder = new();
        configure(builder);
        Arbitrator.ReplaceConfiguration(builder.Build());
    }

    /// <summary>
    ///  Replaces process-global quench rules from a
    ///  <c>&lt;quench&gt;</c> XML fragment or document.
    /// </summary>
    /// <param name="xml">
    ///  XML containing a <c>quench</c> section in the Framework
    ///  0.1.1 schema. May not be <see langword="null"/>.
    /// </param>
    public static void ConfigureFromXml(string xml)
    {
        if (xml is null)
        {
            throw new ArgumentNullException(nameof(xml));
        }

        Arbitrator.ReplaceConfiguration(QuenchXmlParser.Parse(xml));
    }

    /// <summary>
    ///  Restores the safety default: rethrow, with no exception
    ///  rules.
    /// </summary>
    public static void ResetConfiguration()
    {
        Arbitrator.ReplaceConfiguration(
            QuenchConfigurationSnapshot.DefaultThrow);
    }

    /// <summary>
    ///  Sets the process-global logger, and returns the previously
    ///  registered instance.
    /// </summary>
    /// <param name="logger">
    ///  An instance of a type implementing
    ///  <see cref="ISimpleLogger"/>. May not be
    ///  <see langword="null"/>.
    /// </param>
    /// <returns>The previously registered instance.</returns>
    public static ISimpleLogger SetProcessGlobalLogger(ISimpleLogger logger)
    {
        return Arbitrator.SetProcessGlobalLogger(logger);
    }
}
