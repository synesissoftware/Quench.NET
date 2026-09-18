// Created: 22nd July 2013
// Updated: 19th September 2026

namespace Quench;

/// <summary>
///  Action to be performed for a given exception (and catching type).
/// </summary>
/// <remarks>
///  Because the default action is appropriately
///  <see cref="Throw"/>, that enumerator is the 0-valued member.
/// </remarks>
public enum QuenchAction
{
    /// <summary>
    ///  Exception must be rethrown.
    /// </summary>
    Throw = 0,

    /// <summary>
    ///  Exception may be quenched.
    /// </summary>
    Quench = 1,
}
