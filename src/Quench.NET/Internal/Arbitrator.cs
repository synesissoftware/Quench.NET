// Created: 22nd July 2013
// Updated: 19th September 2026

using Quench.Configuration;
using Quench.Diagnostics;

using System.Diagnostics;
using System.Threading;

namespace Quench.Internal;

internal static class Arbitrator
{
    private static readonly object sm_lock = new();
    private static readonly Dictionary<ExceptionNamesKey, QuenchAction> sm_exceptionRecordByNames = new();
    private static readonly Dictionary<ExceptionTypesKey, QuenchAction> sm_exceptionRecords = new();
    private static readonly Dictionary<Type, List<Type>> sm_exceptionInheritances = new();
    private static readonly Dictionary<Type, List<Type>> sm_callerInheritances = new();

    private static ISimpleLogger sm_simpleLogger = new NullSimpleLogger();
    private static QuenchAction DefaultQuenchAction;

    internal static ISimpleLogger Logger => sm_simpleLogger;

    static Arbitrator()
    {
        sm_simpleLogger = new DebugSimpleLogger();
        DefaultQuenchAction = QuenchAction.Throw;
    }

    internal static void ReplaceConfiguration(QuenchConfigurationSnapshot snapshot)
    {
        lock (sm_lock)
        {
            DefaultQuenchAction = snapshot.DefaultAction;
            sm_exceptionRecordByNames.Clear();
            sm_exceptionRecords.Clear();

            foreach (KeyValuePair<ExceptionNamesKey, QuenchAction> pair in snapshot.RulesByNames)
            {
                sm_exceptionRecordByNames.Add(pair.Key, pair.Value);
            }
        }
    }

    internal static ISimpleLogger SetProcessGlobalLogger(ISimpleLogger logger)
    {
        if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        return Interlocked.Exchange(ref sm_simpleLogger, logger);
    }

    internal static bool IsPreciselySpecified(Type exceptionType, Type? catchingType)
    {
        Logger.LogDebug(
            "IsPreciselySpecified({0}, {1})",
            exceptionType,
            catchingType!);

        lock (sm_lock)
        {
            return sm_exceptionRecordByNames.ContainsKey(
                new ExceptionNamesKey(
                    exceptionType.FullName ?? exceptionType.Name,
                    catchingType?.FullName));
        }
    }

    [DebuggerStepThrough]
    internal static bool MustBeRethrown(Exception x)
    {
        if (x is null)
        {
            throw new ArgumentNullException(nameof(x));
        }

        Type exceptionType = x.GetType();
        Logger.LogDebug("MustBeRethrown({0})", exceptionType);
        return QuenchAction.Quench != Arbitrate(exceptionType, null);
    }

    [DebuggerStepThrough]
    internal static bool MustBeRethrown(Exception x, Type? catchingType)
    {
        if (x is null)
        {
            throw new ArgumentNullException(nameof(x));
        }

        Type exceptionType = x.GetType();
        Logger.LogDebug(
            "MustBeRethrown({0}, {1})",
            exceptionType,
            catchingType!);
        return QuenchAction.Quench != Arbitrate(exceptionType, catchingType);
    }

    private static List<Type> GetParentExceptionTypes(Type exceptionType)
    {
        Logger.LogDebug("GetParentExceptionTypes({0})", exceptionType);

        lock (sm_lock)
        {
            if (!sm_exceptionInheritances.TryGetValue(exceptionType, out List<Type>? list))
            {
                list = [];

                for (Type? parentType = exceptionType.BaseType;
                     parentType is not null
                        && parentType != typeof(Exception)
                        && parentType != typeof(object);
                     parentType = parentType.BaseType)
                {
                    Debug.Assert(parentType.IsSubclassOf(typeof(Exception)));
                    list.Add(parentType);
                }

                sm_exceptionInheritances.Add(exceptionType, list);
            }

            return list;
        }
    }

    private static List<Type> GetParentCatchingTypes(Type catchingType)
    {
        Logger.LogDebug("GetParentCatchingTypes({0})", catchingType);

        lock (sm_lock)
        {
            if (!sm_callerInheritances.TryGetValue(catchingType, out List<Type>? list))
            {
                list = [];

                for (Type? parentType = catchingType.BaseType;
                     parentType is not null && parentType != typeof(object);
                     parentType = parentType.BaseType)
                {
                    list.Add(parentType);
                }

                sm_callerInheritances.Add(catchingType, list);
            }

            return list;
        }
    }

    private static bool TryCacheAndReturn(
        ExceptionTypesKey typesKey,
        QuenchAction action,
        out QuenchAction result)
    {
        if (!sm_exceptionRecords.ContainsKey(typesKey))
        {
            Logger.LogInformational("adding exception record {{{0} => {1}}}", typesKey, action);
            sm_exceptionRecords.Add(typesKey, action);
        }

        result = action;
        return true;
    }

    private static QuenchAction Arbitrate(Type exceptionType, Type? catchingType)
    {
        if (exceptionType is null)
        {
            throw new ArgumentNullException(nameof(exceptionType));
        }

        Debug.Assert(
            exceptionType.Equals(typeof(Exception))
            || exceptionType.IsSubclassOf(typeof(Exception)));

        Logger.LogDebug("Arbitrate({0}, {1})", exceptionType, catchingType!);

        lock (sm_lock)
        {
            if (sm_exceptionRecordByNames.Count == 0)
            {
                return DefaultQuenchAction;
            }

            QuenchAction action;
            ExceptionTypesKey typesKey = new(exceptionType, catchingType);

            // 1. Try the specific exception record. If it exists for this
            //    type, then that's the most precise answer.
            if (sm_exceptionRecords.TryGetValue(typesKey, out action))
            {
                return action;
            }

            // 2. Now try and look for the exact type from the
            //    exception-names map.
            ExceptionNamesKey namesKey = new(
                exceptionType.FullName ?? exceptionType.Name,
                catchingType?.FullName);

            if (sm_exceptionRecordByNames.TryGetValue(namesKey, out action)
                && TryCacheAndReturn(typesKey, action, out action))
            {
                return action;
            }

            // 3. Inheritance relationships (step comments from 0.1.1):
            // 3.1. If no calling type -> look for ancestor types
            // 3.2. Specific exception-type with calling-type's parent types
            // 3.3. Exception-type's parent types with specific calling-type
            // 3.4. Exception-type's parent types with calling-type's parents
            // 3.5. Exception-type with no calling context
            if (catchingType is not null)
            {
                List<Type> exceptionParentTypes = GetParentExceptionTypes(exceptionType);
                List<Type> callingParentTypes = GetParentCatchingTypes(catchingType);

                // 3.2. Specific exception-type with calling-type's parent types
                foreach (Type callingParentType in callingParentTypes)
                {
                    ExceptionTypesKey parentTypesKey = new(exceptionType, callingParentType);
                    if (sm_exceptionRecords.TryGetValue(parentTypesKey, out action)
                        && TryCacheAndReturn(typesKey, action, out action))
                    {
                        return action;
                    }

                    ExceptionNamesKey parentNamesKey = new(
                        exceptionType.FullName ?? exceptionType.Name,
                        callingParentType.FullName);
                    if (sm_exceptionRecordByNames.TryGetValue(parentNamesKey, out action)
                        && TryCacheAndReturn(typesKey, action, out action))
                    {
                        return action;
                    }
                }

                // 3.3. Exception-type's parent types with specific calling-type.
                // Hautacam 0.1.1 used exceptionType (not exceptionParentType)
                // in this loop, ignoring the parent walk the comments
                // describe. This port uses exceptionParentType.
                foreach (Type exceptionParentType in exceptionParentTypes)
                {
                    ExceptionTypesKey parentTypesKey = new(exceptionParentType, catchingType);
                    if (sm_exceptionRecords.TryGetValue(parentTypesKey, out action)
                        && TryCacheAndReturn(typesKey, action, out action))
                    {
                        return action;
                    }

                    ExceptionNamesKey parentNamesKey = new(
                        exceptionParentType.FullName ?? exceptionParentType.Name,
                        catchingType.FullName);
                    if (sm_exceptionRecordByNames.TryGetValue(parentNamesKey, out action)
                        && TryCacheAndReturn(typesKey, action, out action))
                    {
                        return action;
                    }
                }

                // 3.4. Exception-type's parent types with calling-type's parents
                foreach (Type callingParentType in callingParentTypes)
                {
                    foreach (Type exceptionParentType in exceptionParentTypes)
                    {
                        ExceptionTypesKey parentTypesKey = new(exceptionParentType, callingParentType);
                        if (sm_exceptionRecords.TryGetValue(parentTypesKey, out action)
                            && TryCacheAndReturn(typesKey, action, out action))
                        {
                            return action;
                        }

                        ExceptionNamesKey parentNamesKey = new(
                            exceptionParentType.FullName ?? exceptionParentType.Name,
                            callingParentType.FullName);
                        if (sm_exceptionRecordByNames.TryGetValue(parentNamesKey, out action)
                            && TryCacheAndReturn(typesKey, action, out action))
                        {
                            return action;
                        }
                    }
                }

                // 3.5. Exception-type with no calling context
                ExceptionTypesKey unqualifiedTypesKey = new(exceptionType, null);
                if (sm_exceptionRecords.TryGetValue(unqualifiedTypesKey, out action)
                    && TryCacheAndReturn(typesKey, action, out action))
                {
                    return action;
                }

                ExceptionNamesKey unqualifiedNamesKey = new(
                    exceptionType.FullName ?? exceptionType.Name,
                    null);
                if (sm_exceptionRecordByNames.TryGetValue(unqualifiedNamesKey, out action)
                    && TryCacheAndReturn(typesKey, action, out action))
                {
                    return action;
                }
            }

            // 3.1. Ancestor exception types with no catching context
            foreach (Type parentType in GetParentExceptionTypes(exceptionType))
            {
                ExceptionTypesKey parentTypesKey = new(parentType, null);
                if (sm_exceptionRecords.TryGetValue(parentTypesKey, out action)
                    && TryCacheAndReturn(typesKey, action, out action))
                {
                    return action;
                }

                ExceptionNamesKey parentNamesKey = new(
                    parentType.FullName ?? parentType.Name,
                    null);
                if (sm_exceptionRecordByNames.TryGetValue(parentNamesKey, out action)
                    && TryCacheAndReturn(typesKey, action, out action))
                {
                    return action;
                }
            }

            // 4. Use default
            ExceptionTypesKey wildcardKey = new(exceptionType, null);
            if (!sm_exceptionRecords.ContainsKey(wildcardKey))
            {
                Logger.LogInformational(
                    "adding exception record {{{0} => {1}}}",
                    wildcardKey,
                    DefaultQuenchAction);
                sm_exceptionRecords.Add(wildcardKey, DefaultQuenchAction);
            }

            return DefaultQuenchAction;
        }
    }
}
