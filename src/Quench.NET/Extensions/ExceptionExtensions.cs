// Created: 22nd July 2013
// Updated: 19th September 2026

using System.Diagnostics;

namespace Quench.Extensions;

/// <summary>
///  Extension methods that delegate to <see cref="Core"/>.
/// </summary>
public static class ExceptionExtensions
{
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
    public static bool MayBeQuenched(this Exception x)
    {
        return Core.MayBeQuenched(x);
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
    public static bool MayBeQuenched(this Exception x, Type? catchingType)
    {
        return Core.MayBeQuenched(x, catchingType);
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
    public static bool MustBeRethrown(this Exception x)
    {
        return Core.MustBeRethrown(x);
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
    public static bool MustBeRethrown(this Exception x, Type? catchingType)
    {
        return Core.MustBeRethrown(x, catchingType);
    }
}
