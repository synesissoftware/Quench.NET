// Created: 19th September 2026
// Updated: 19th September 2026

using Quench.Configuration;
using Quench.Exceptions;
using Quench.Internal;

namespace Quench;

/// <summary>
///  Fluent builder for process-global quench rules.
/// </summary>
public sealed class QuenchConfigurationBuilder
{
    private readonly List<NamedExceptionRule> _rules = [];
    private NamedExceptionRule? _current;

    /// <summary>
    ///  Default action when no rule matches. Defaults to
    ///  <see cref="QuenchAction.Throw"/>.
    /// </summary>
    public QuenchAction DefaultAction { get; set; } = QuenchAction.Throw;

    /// <summary>
    ///  Adds a rule for <paramref name="exceptionType"/>, optionally
    ///  with a default action before any <see cref="ExceptWhen"/>.
    /// </summary>
    /// <param name="exceptionType">
    ///  Exception type. May not be <see cref="Exception"/> itself.
    /// </param>
    /// <param name="action">
    ///  Action for the type when no catching-type override matches.
    ///  May be omitted when at least one <see cref="ExceptWhen"/>
    ///  follows.
    /// </param>
    /// <returns>This builder.</returns>
    public QuenchConfigurationBuilder ForException(
        Type exceptionType,
        QuenchAction? action = null)
    {
        if (exceptionType is null)
        {
            throw new ArgumentNullException(nameof(exceptionType));
        }

        if (!typeof(Exception).IsAssignableFrom(exceptionType))
        {
            throw new ArgumentException(
                "Type must be System.Exception or a subclass.",
                nameof(exceptionType));
        }

        AddNamedRule(RequireFullName(exceptionType), action);
        return this;
    }

    /// <summary>
    ///  Adds a rule for <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">Exception type.</typeparam>
    /// <param name="action">
    ///  Action for the type when no catching-type override matches.
    /// </param>
    /// <returns>This builder.</returns>
    public QuenchConfigurationBuilder ForException<TException>(
        QuenchAction? action = null)
        where TException : Exception
    {
        return ForException(typeof(TException), action);
    }

    /// <summary>
    ///  Adds a catching-type override for the most recent
    ///  <see cref="ForException(Type, QuenchAction?)"/> rule.
    /// </summary>
    /// <param name="catchingType">
    ///  Catching type. May not be <see langword="null"/>.
    /// </param>
    /// <param name="action">Action in that catching context.</param>
    /// <returns>This builder.</returns>
    public QuenchConfigurationBuilder ExceptWhen(
        Type catchingType,
        QuenchAction action)
    {
        if (catchingType is null)
        {
            throw new ArgumentNullException(nameof(catchingType));
        }

        if (_current is null)
        {
            throw new InvalidOperationException(
                "ExceptWhen requires a preceding ForException.");
        }

        _current.ExceptWhens.Add(
            new NamedCatchingOverride(RequireFullName(catchingType), action));
        return this;
    }

    /// <summary>
    ///  Adds a catching-type override for
    ///  <typeparamref name="TCatching"/>.
    /// </summary>
    /// <typeparam name="TCatching">Catching type.</typeparam>
    /// <param name="action">Action in that catching context.</param>
    /// <returns>This builder.</returns>
    public QuenchConfigurationBuilder ExceptWhen<TCatching>(
        QuenchAction action)
    {
        return ExceptWhen(typeof(TCatching), action);
    }

    internal void AddNamedRule(string ofClass, QuenchAction? action)
    {
        RejectSystemException(ofClass);
        NamedExceptionRule rule = new(ofClass, action);
        _rules.Add(rule);
        _current = rule;
    }

    internal void AddNamedExceptWhen(string inClass, QuenchAction action)
    {
        if (_current is null)
        {
            throw new InvalidOperationException(
                "ExceptWhen requires a preceding ForException.");
        }

        _current.ExceptWhens.Add(new NamedCatchingOverride(inClass, action));
    }

    internal QuenchConfigurationSnapshot Build()
    {
        Dictionary<ExceptionNamesKey, QuenchAction> map = new();

        foreach (NamedExceptionRule rule in _rules)
        {
            if (!rule.Action.HasValue && rule.ExceptWhens.Count == 0)
            {
                throw new InvalidQuenchConfigurationException(
                    "forException",
                    "Missing quenchAction with no exceptWhen nodes specified");
            }

            if (rule.Action.HasValue)
            {
                map.Add(
                    new ExceptionNamesKey(rule.OfClass, null),
                    rule.Action.Value);
            }

            foreach (NamedCatchingOverride exceptWhen in rule.ExceptWhens)
            {
                map.Add(
                    new ExceptionNamesKey(rule.OfClass, exceptWhen.InClass),
                    exceptWhen.Action);
            }
        }

        return new QuenchConfigurationSnapshot(DefaultAction, map);
    }

    private static string RequireFullName(Type type)
    {
        string? fullName = type.FullName;
        if (fullName is null)
        {
            throw new ArgumentException(
                "Type must have a full name.",
                nameof(type));
        }

        return fullName;
    }

    private static void RejectSystemException(string ofClass)
    {
        if (ofClass == typeof(Exception).FullName)
        {
            throw new InvalidQuenchConfigurationValueException(
                "forException",
                "ofClass",
                ofClass);
        }
    }

    private sealed class NamedExceptionRule
    {
        internal NamedExceptionRule(string ofClass, QuenchAction? action)
        {
            OfClass = ofClass;
            Action = action;
        }

        internal string OfClass { get; }

        internal QuenchAction? Action { get; }

        internal List<NamedCatchingOverride> ExceptWhens { get; } = [];
    }

    private readonly struct NamedCatchingOverride
    {
        internal NamedCatchingOverride(string inClass, QuenchAction action)
        {
            InClass = inClass;
            Action = action;
        }

        internal string InClass { get; }

        internal QuenchAction Action { get; }
    }
}
