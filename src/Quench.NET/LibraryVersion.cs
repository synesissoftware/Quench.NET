// Created: 19th September 2026
// Updated: 19th September 2026

namespace Quench;

/// <summary>
///  Library version constants for <b>Quench.NET</b>.
/// </summary>
public static class LibraryVersion
{
    /// <summary>Major version component.</summary>
    public const int Major = 0;

    /// <summary>Minor version component.</summary>
    public const int Minor = 1;

    /// <summary>Patch version component.</summary>
    public const int Patch = 0;

    /// <summary>
    ///  Human-readable version string matching
    ///  <see cref="Major"/>.<see cref="Minor"/>.<see cref="Patch"/>.
    /// </summary>
    public static string VersionString => $"{Major}.{Minor}.{Patch}";
}
