// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench.Internal;

internal readonly struct ExceptionTypesKey : IEquatable<ExceptionTypesKey>
{
    internal ExceptionTypesKey(Type exceptionType, Type? catchingType)
    {
        ExceptionType = exceptionType;
        CatchingType = catchingType;
    }

    internal Type ExceptionType { get; }

    internal Type? CatchingType { get; }

    public bool Equals(ExceptionTypesKey other)
    {
        return ExceptionType == other.ExceptionType
            && CatchingType == other.CatchingType;
    }

    public override bool Equals(object? obj)
    {
        return obj is ExceptionTypesKey other && Equals(other);
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
