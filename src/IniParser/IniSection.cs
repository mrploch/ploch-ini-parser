namespace Ploch.IniParser;

/// <summary>
///     Represents a section in an INI file, identified by a name and containing key-value
///     pairs along with optional comments.
/// </summary>
/// <param name="name">The name of the INI section.</param>
/// <param name="sectionComments">Optional comments associated with the section.</param>
public class IniSection(string name, IEnumerable<string>? sectionComments = null)
{
    /// <summary>
    ///     Gets the name of the INI section.
    /// </summary>
    public string Name => name;

    /// <summary>
    ///     Gets the comments contained in this INI section.
    /// </summary>
    public ICollection<string> Comments { get; } = new List<string>();

    /// <summary>
    ///     Gets the optional comments associated with the INI section.
    /// </summary>
    public IEnumerable<string> SectionComments { get; } = sectionComments?.ToArray() ?? [];

    /// <summary>
    ///     Gets the collection of entries contained in the INI section.
    /// </summary>
    public IDictionary<string, KeyData> Entries { get; } = new Dictionary<string, KeyData>();

    /// <summary>
    ///     Gets the collection of duplicate entries found within the INI section.
    /// </summary>
    /// s
    public ICollection<KeyData> DuplicateEntries { get; } = new List<KeyData>();
}