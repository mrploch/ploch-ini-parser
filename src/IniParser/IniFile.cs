namespace Ploch.IniParser;

/// <summary>
///     Represents an INI file, which consists of a global section and a collection
///     of named sections, each containing key-value pairs and optional comments.
/// </summary>
public class IniFile
{
    /// <summary>
    ///     Gets the global section of the INI file. The global section contains key-value pairs and
    ///     optional comments that are not associated with any specific named section in the file.
    /// </summary>
    public IniSection GlobalSection { get; } = new(string.Empty);

    /// <summary>
    ///     Gets a dictionary of named sections within the INI file. Each section contains
    ///     key-value pairs and optional comments associated with a specific section name.
    /// </summary>
    public IDictionary<string, IniSection> Sections { get; } = new Dictionary<string, IniSection>();

    /// <summary>
    ///     Gets the collection of sections in the INI file that have duplicate section names.
    ///     Each entry in this collection represents a section that has the same name as another section
    ///     in the <see cref="Sections" /> collection but contains different key-value pairs or comments.
    /// </summary>
    public IDictionary<string, IniSection> DuplicateSections { get; } = new Dictionary<string, IniSection>();
}
