namespace Ploch.IniParser;

/// <summary>
///     Represents an entry in an INI file, along with optional comments associated with it.
/// </summary>
/// <param name="Key">The key or name of the INI entry.</param>
/// <param name="Value">The value assigned to the key in the INI entry.</param>
/// <param name="Comments">Optional comments associated with the INI entry.</param>
public record KeyData(string Key, string Value, IEnumerable<string> Comments);