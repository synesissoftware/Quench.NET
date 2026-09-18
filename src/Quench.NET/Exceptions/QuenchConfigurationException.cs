// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench.Exceptions;

/// <summary>
///  Root exception for Quench configuration.
/// </summary>
public class QuenchConfigurationException : QuenchException
{
    internal QuenchConfigurationException(
        string? containingSectionName,
        string message)
        : base(message)
    {
        ContainingSectionName = containingSectionName;
    }

    /// <summary>
    ///  Name of the containing section, if any.
    /// </summary>
    /// <remarks>May be <see langword="null"/>.</remarks>
    public string? ContainingSectionName { get; }
}
