// Created: 18th August 2013
// Updated: 19th September 2026

namespace Quench.Diagnostics;

/// <summary>
///  Null Object implementation of <see cref="ISimpleLogger"/>.
/// </summary>
public sealed class NullSimpleLogger : ISimpleLogger
{
    /// <inheritdoc />
    public bool IsLevelEnabled(SimpleSeverityLevel severity)
    {
        return false;
    }

    /// <inheritdoc />
    public void Log(
        SimpleSeverityLevel severity,
        string? message,
        params object[] args)
    {
    }

    /// <inheritdoc />
    public void LogDebug(string? message, params object[] args)
    {
    }

    /// <inheritdoc />
    public void LogInformational(string? message, params object[] args)
    {
    }

    /// <inheritdoc />
    public void LogWarning(string? message, params object[] args)
    {
    }

    /// <inheritdoc />
    public void LogError(string? message, params object[] args)
    {
    }

    /// <inheritdoc />
    public void LogAlert(string? message, params object[] args)
    {
    }

    /// <inheritdoc />
    public void LogEmergency(string? message, params object[] args)
    {
    }
}
