// Created: 18th August 2013
// Updated: 19th September 2026

namespace Quench.Diagnostics;

/// <summary>
///  Designates the severity level of a log statement.
/// </summary>
public enum SimpleSeverityLevel
{
    /// <summary>
    ///  Indicates a condition in which the program has violated its
    ///  own design.
    /// </summary>
    Emergency = 0,

    /// <summary>
    ///  Indicates a condition that is practically unrecoverable for
    ///  the program.
    /// </summary>
    Alert = 1,

    /// <summary>
    ///  Indicates an error condition.
    /// </summary>
    Error = 3,

    /// <summary>
    ///  Indicates a warning condition.
    /// </summary>
    Warning = 4,

    /// <summary>
    ///  Severity associated with informational statements.
    /// </summary>
    Informational = 6,

    /// <summary>
    ///  Severity associated with debug statements.
    /// </summary>
    Debug = 7,
}
