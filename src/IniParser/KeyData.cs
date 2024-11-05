namespace Ploch.IniParser;

/// <summary>
///     Represents an entry in an INI file, along with optional comments associated with it.
/// </summary>
/// <param name="key">The key or name of the INI entry.</param>
/// <param name="value">The value assigned to the key in the INI entry.</param>
/// <param name="comments">Optional comments associated with the INI entry.</param>
public class KeyData(string key, string value, IEnumerable<string> comments)
{
    /// <summary>The key or name of the INI entry.</summary>
    public string Key { get; } = key;

    /// <summary>The value assigned to the key in the INI entry.</summary>
    public string Value { get; } = value;

    /// <summary>Optional comments associated with the INI entry.</summary>
    public IEnumerable<string> Comments { get; } = comments;
}
