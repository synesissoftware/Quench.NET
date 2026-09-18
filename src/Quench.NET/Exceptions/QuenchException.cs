// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench.Exceptions;

/// <summary>
///  Root exception for Quench.
/// </summary>
public class QuenchException : Exception
{
    internal QuenchException(string message)
        : base(message)
    {
    }
}
