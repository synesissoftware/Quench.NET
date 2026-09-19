// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench.Exceptions;

/// <summary>
///  Exception thrown to indicate general invalid
///  <c>&lt;quench&gt;</c> configuration.
/// </summary>
public class InvalidQuenchConfigurationException : QuenchConfigurationException
{
    internal InvalidQuenchConfigurationException(
        string sectionName,
        string message)
        : base(sectionName, message)
    {
        SubsectionNameOrNull = null;
    }

    internal InvalidQuenchConfigurationException(
        string sectionName,
        string message,
        string? subsectionNameOrNull)
        : base(sectionName, message)
    {
        SubsectionNameOrNull = subsectionNameOrNull;
    }

    /// <summary>
    ///  Relevant subsection name, or <see langword="null"/> if none
    ///  relevant.
    /// </summary>
    public string? SubsectionNameOrNull { get; }
}
