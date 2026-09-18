// Created: 18th August 2013
// Updated: 19th September 2026

using System.Diagnostics;
using System.Text;

namespace Quench.Diagnostics;

/// <summary>
///  Implementation of <see cref="ISimpleLogger"/> using diagnostic
///  logging services of <see cref="Debug"/>.
/// </summary>
public sealed class DebugSimpleLogger : ISimpleLogger
{
#if DEBUG
    private static readonly SimpleSeverityLevel CompileTimeDeterminedThreshold =
        SimpleSeverityLevel.Debug;
#else
    private static readonly SimpleSeverityLevel CompileTimeDeterminedThreshold =
        SimpleSeverityLevel.Warning;
#endif

    private static bool IsEnabled(SimpleSeverityLevel severity)
    {
        return severity <= CompileTimeDeterminedThreshold;
    }

    private static void DoLog(
        SimpleSeverityLevel severity,
        string? message,
        object[] args)
    {
        if (!IsEnabled(severity))
        {
            return;
        }

        message ??= string.Empty;

        if (message.Length == 0)
        {
            if (args.Length == 0)
            {
                Debug.WriteLine(severity);
                return;
            }

            StringBuilder sb = new(args.Length * 20);
            sb.Append(severity.ToString());
            sb.Append(": ");
            int n = 0;
            foreach (object? arg in args)
            {
                if (n++ != 0)
                {
                    sb.Append(", ");
                }

                sb.Append(arg);
            }

            Debug.WriteLine(sb.ToString());
            return;
        }

        if (args.Length == 0)
        {
            Debug.WriteLine(severity.ToString() + ": " + message);
            return;
        }

        int indexOfOpenCurly = message.IndexOf('{');
        if (indexOfOpenCurly >= 0)
        {
            try
            {
                Debug.WriteLine(
                    severity.ToString() + ": " + string.Format(message, args));
                return;
            }
            catch (FormatException)
            {
            }
        }

        StringBuilder fallback = new(args.Length * 20);
        fallback.Append(severity.ToString());
        fallback.Append(": ");
        fallback.Append(message);
        foreach (object? arg in args)
        {
            fallback.Append(", ");
            fallback.Append(arg);
        }

        Debug.WriteLine(fallback.ToString());
    }

    /// <inheritdoc />
    public bool IsLevelEnabled(SimpleSeverityLevel severity)
    {
        return IsEnabled(severity);
    }

    /// <inheritdoc />
    public void Log(
        SimpleSeverityLevel severity,
        string? message,
        params object[] args)
    {
        DoLog(severity, message, args);
    }

    /// <inheritdoc />
    public void LogDebug(string? message, params object[] args)
    {
        DoLog(SimpleSeverityLevel.Debug, message, args);
    }

    /// <inheritdoc />
    public void LogInformational(string? message, params object[] args)
    {
        DoLog(SimpleSeverityLevel.Informational, message, args);
    }

    /// <inheritdoc />
    public void LogWarning(string? message, params object[] args)
    {
        DoLog(SimpleSeverityLevel.Warning, message, args);
    }

    /// <inheritdoc />
    public void LogError(string? message, params object[] args)
    {
        DoLog(SimpleSeverityLevel.Error, message, args);
    }

    /// <inheritdoc />
    public void LogAlert(string? message, params object[] args)
    {
        DoLog(SimpleSeverityLevel.Alert, message, args);
    }

    /// <inheritdoc />
    public void LogEmergency(string? message, params object[] args)
    {
        DoLog(SimpleSeverityLevel.Emergency, message, args);
    }
}
