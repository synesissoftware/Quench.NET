// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench.Exceptions;

/// <summary>
///  Exception thrown when a <c>&lt;quench&gt;</c> (sub)section
///  contains a missing configuration value.
/// </summary>
public class MissingQuenchConfigurationValueException
    : InvalidQuenchConfigurationException
{
    internal MissingQuenchConfigurationValueException(
        string sectionName,
        string valueName)
        : base(
            sectionName,
            $"Missing value '{valueName}' in subsection '{sectionName}'")
    {
        ValueName = valueName;
    }

    /// <summary>
    ///  The name of the missing value.
    /// </summary>
    public string ValueName { get; }
}
