// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench.Exceptions;

/// <summary>
///  Exception thrown when a configuration section is not recognised.
/// </summary>
public class UnrecognisedConfigurationSectionException : QuenchConfigurationException
{
    internal UnrecognisedConfigurationSectionException(
        string? containingSectionName,
        string unrecognisedSectionName)
        : base(
            containingSectionName,
            MakeMessage(containingSectionName, unrecognisedSectionName))
    {
        UnrecognisedSectionName = unrecognisedSectionName;
    }

    internal UnrecognisedConfigurationSectionException(
        string? containingSectionName,
        string unrecognisedSectionName,
        string message)
        : base(containingSectionName, message)
    {
        UnrecognisedSectionName = unrecognisedSectionName;
    }

    /// <summary>
    ///  Name of the unrecognised section.
    /// </summary>
    public string UnrecognisedSectionName { get; }

    private static string MakeMessage(
        string? containingSectionName,
        string unrecognisedSectionName)
    {
        if (containingSectionName is null)
        {
            return $"Subsection name '{unrecognisedSectionName}' is not recognised";
        }

        return $"Subsection name '{unrecognisedSectionName}' (of section '{containingSectionName}') is not recognised";
    }
}
