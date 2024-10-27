using System.Text.RegularExpressions;

namespace Ploch.IniParser;

/// <summary>
///     The IniFileParser class provides functionality to parse INI files and convert them into an IniFile object.
/// </summary>
public static class IniFileParser
{
    private static readonly Regex SectionNameRegex = new(@"^\[(?<sectionName>.+)\]");
    private static readonly Regex EntryRegex = new(@"^(?<key>[^=]+)=(?<value>[^#]+)#?(?<comment>.*$)");
    private static readonly Regex CommentRegex = new(@"#\s*(?<comment>.+)");

    /// <summary>
    ///     Parses a collection of strings representing lines of an INI file and converts them into an IniFile object.
    /// </summary>
    /// <param name="lines">The lines of the INI file to be parsed.</param>
    /// <returns>An IniFile object containing the data parsed from the provided lines.</returns>
    public static IniFile Parse(IEnumerable<string> lines)
    {
        var iniFile = new IniFile();

        string? currentComment = null;
        var currentSection = iniFile.GlobalSection;
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                currentComment = null;
                continue;
            }

            if (ProcessSection(out var section, line, currentComment))
            {
                currentSection = section!;
                SetSection(iniFile, section!);
                currentComment = null;
            }
            else if (ProcessEntry(line, currentSection, currentComment))
            {
                currentComment = null;
            }
            else
            {
                currentComment = ProcessCommentLine(line, currentSection);
            }
        }

        return iniFile;
    }

    private static bool ProcessSection(out IniSection? section, string line, string? currentComment)
    {
        var match = SectionNameRegex.Match(line);
        if (match.Success)
        {
            var sectionName = match.Groups["sectionName"].Value.Trim();
            var comments = GetComments(line, currentComment);
            section = new IniSection(sectionName, comments);

            return true;
        }

        section = null;
        return false;
    }

    private static void SetSection(IniFile iniFile, IniSection section)
    {
        if (iniFile.Sections.ContainsKey(section.Name))
        {
            iniFile.DuplicateSections.Add(section.Name, section);
        }

        iniFile.Sections[section.Name] = section;
    }

    private static bool ProcessEntry(string line, IniSection currentSection, string? currentComment)
    {
        var entryMatch = EntryRegex.Match(line);
        if (entryMatch.Success)
        {
            var keyName = entryMatch.Groups["key"].Value.Trim();
            if (currentSection.Entries.TryGetValue(keyName, out var value))
            {
                currentSection.DuplicateEntries.Add(value);
            }

            var comments = GetComments(line, currentComment);
            currentSection.Entries[keyName] = new KeyData(keyName, entryMatch.Groups["value"].Value.Trim(), comments);

            return true;
        }

        return false;
    }

    private static string? ProcessCommentLine(string line, IniSection currentSection)
    {
        var match = CommentRegex.Match(line);
        string? comment = null;
        if (match.Success)
        {
            comment = match.Groups["comment"].Value;
            if (!currentSection.SectionComments.Contains(comment, StringComparer.InvariantCulture))
            {
                currentSection.Comments.Add(comment);
            }
        }

        return comment;
    }

    private static List<string> GetComments(string line, string? currentComment)
    {
        var comments = new List<string>();

        if (TryGetComment(out var comment, line))
        {
            comments.Add(comment!);
        }

        if (currentComment is not null)
        {
            comments.Add(currentComment);
        }

        return comments;
    }

    private static bool TryGetComment(out string? comment, string line)
    {
        var match = CommentRegex.Match(line);
        if (match.Success)
        {
            comment = match.Groups["comment"].Value;
            return true;
        }

        comment = null;
        return false;
    }
}