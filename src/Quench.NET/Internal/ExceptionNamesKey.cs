// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench.Internal;

internal readonly struct ExceptionNamesKey : IEquatable<ExceptionNamesKey>
{
    internal ExceptionNamesKey(string exceptionType, string? catchingType)
    {
        ExceptionType = exceptionType;
        CatchingType = catchingType;
    }

    internal string ExceptionType { get; }

    internal string? CatchingType { get; }

    public bool Equals(ExceptionNamesKey other)
    {
        return ExceptionType == other.ExceptionType
            && CatchingType == other.CatchingType;
    }

    public override bool Equals(object? obj)
    {
        return obj is ExceptionNamesKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = (hash * 31) + ExceptionType.GetHashCode();
            hash = (hash * 31) + (CatchingType?.GetHashCode() ?? 0);
            return hash;
        }
    }

    public override string ToString()
    {
        return $"{{{ExceptionType}, {CatchingType}}}";
    }
}
