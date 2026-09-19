// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench.Exceptions;

/// <summary>
///  Exception thrown when a configuration attribute is not
///  recognised.
/// </summary>
public class UnrecognisedConfigurationAttributeException
    : QuenchConfigurationException
{
    internal UnrecognisedConfigurationAttributeException(
        string containingSectionName,
        string unrecognisedAttributeName)
        : base(
            containingSectionName,
            MakeMessage(containingSectionName, unrecognisedAttributeName))
    {
        if (unrecognisedAttributeName is null)
        {
            throw new ArgumentNullException(nameof(unrecognisedAttributeName));
        }

        UnrecognisedAttributeName = unrecognisedAttributeName;
    }

    /// <summary>
    ///  Name of the unrecognised attribute.
    /// </summary>
    public string UnrecognisedAttributeName { get; }

    private static string MakeMessage(
        string containingSectionName,
        string unrecognisedAttributeName)
    {
        return $"Attribute name '{unrecognisedAttributeName}' (of section '{containingSectionName}') is not recognised";
    }
}
