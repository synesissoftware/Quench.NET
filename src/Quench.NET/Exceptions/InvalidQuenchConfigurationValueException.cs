// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench.Exceptions;

/// <summary>
///  Exception thrown when an attribute has an invalid value.
/// </summary>
public class InvalidQuenchConfigurationValueException : QuenchConfigurationException
{
    internal InvalidQuenchConfigurationValueException(
        string subsectionName,
        string attributeName,
        string invalidValue)
        : base(
            subsectionName,
            $"Invalid value '{invalidValue}' for attribute '{attributeName}' in subsection '{subsectionName}'")
    {
        AttributeName = attributeName;
        InvalidValue = invalidValue;
    }

    /// <summary>
    ///  The invalid attribute name.
    /// </summary>
    public string AttributeName { get; }

    /// <summary>
    ///  The invalid attribute value.
    /// </summary>
    public string InvalidValue { get; }
}
