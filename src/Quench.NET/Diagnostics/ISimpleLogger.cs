// Created: 18th August 2013
// Updated: 19th September 2026

namespace Quench.Diagnostics;

/// <summary>
///  Defines a diagnostic logging interface.
/// </summary>
public interface ISimpleLogger
{
    /// <summary>
    ///  Indicates whether a log statement associated with the given
    ///  severity level will be logged by the implementing instance.
    /// </summary>
    /// <param name="severity">Severity to query.</param>
    /// <returns>
    ///  <see langword="true"/> if the level is enabled; otherwise
    ///  <see langword="false"/>.
    /// </returns>
    bool IsLevelEnabled(SimpleSeverityLevel severity);

    /// <summary>
    ///  Schedules a log-statement at the given severity.
    /// </summary>
    /// <param name="severity">Severity of the statement.</param>
    /// <param name="message">
    ///  The message to be scheduled. May be <see langword="null"/>.
    /// </param>
    /// <param name="args">
    ///  Optional arguments to be formatted into
    ///  <paramref name="message"/>.
    /// </param>
    void Log(
        SimpleSeverityLevel severity,
        string? message,
        params object[] args);

    /// <summary>
    ///  Schedules a log-statement at the
    ///  <see cref="SimpleSeverityLevel.Debug"/> severity level.
    /// </summary>
    /// <param name="message">The message to be scheduled.</param>
    /// <param name="args">
    ///  Optional number of arguments to be formatted into
    ///  <paramref name="message"/>.
    /// </param>
    void LogDebug(string? message, params object[] args);

    /// <summary>
    ///  Schedules a log-statement at the
    ///  <see cref="SimpleSeverityLevel.Informational"/> severity
    ///  level.
    /// </summary>
    /// <param name="message">The message to be scheduled.</param>
    /// <param name="args">
    ///  Optional number of arguments to be formatted into
    ///  <paramref name="message"/>.
    /// </param>
    void LogInformational(string? message, params object[] args);

    /// <summary>
    ///  Schedules a log-statement at the
    ///  <see cref="SimpleSeverityLevel.Warning"/> severity level.
    /// </summary>
    /// <param name="message">The message to be scheduled.</param>
    /// <param name="args">
    ///  Optional number of arguments to be formatted into
    ///  <paramref name="message"/>.
    /// </param>
    void LogWarning(string? message, params object[] args);

    /// <summary>
    ///  Schedules a log-statement at the
    ///  <see cref="SimpleSeverityLevel.Error"/> severity level.
    /// </summary>
    /// <param name="message">The message to be scheduled.</param>
    /// <param name="args">
    ///  Optional number of arguments to be formatted into
    ///  <paramref name="message"/>.
    /// </param>
    void LogError(string? message, params object[] args);

    /// <summary>
    ///  Schedules a log-statement at the
    ///  <see cref="SimpleSeverityLevel.Alert"/> severity level.
    /// </summary>
    /// <param name="message">The message to be scheduled.</param>
    /// <param name="args">
    ///  Optional number of arguments to be formatted into
    ///  <paramref name="message"/>.
    /// </param>
    void LogAlert(string? message, params object[] args);

    /// <summary>
    ///  Schedules a log-statement at the
    ///  <see cref="SimpleSeverityLevel.Emergency"/> severity level.
    /// </summary>
    /// <param name="message">The message to be scheduled.</param>
    /// <param name="args">
    ///  Optional number of arguments to be formatted into
    ///  <paramref name="message"/>.
    /// </param>
    void LogEmergency(string? message, params object[] args);
}
