namespace Ploch.IniParser.Tests;

public class IniParserTests
{
    [Fact]
    public void Parse_can_parse_simple_data()
    {
        var lines = File.ReadAllLines("test-files/simple-editorconfig");
        var iniFile = IniFileParser.Parse(lines);

        var globalSection = iniFile.GlobalSection;

        ValidateSection(globalSection, string.Empty, 2, null, ["global_comment_above_section"]);

        ValidateEntry(globalSection.Entries.First().Value, "global_option_1", "value1", "global_option_1_above_comment", "global_option_1_side_comment");
        ValidateEntry(globalSection.Entries.Skip(1).First().Value, "global_option_2", "value2");

        iniFile.Sections.Should().HaveCount(3);

        var iniSection = ValidateSection(iniFile, "*.ext1", 6, ["global_comment_above_section"], ["group1.option1 comment 1", "group2.option1_comment"]);

        var entries = iniSection.Entries.Select(e => e.Value);
        ValidateEntry(entries.FirstOrDefault(), "group1.option1", "value1", "group1.option1 comment 1", "group1.option1 comment 2");
        ValidateEntry(entries.Skip(1).FirstOrDefault(), "group1.option2", "value2");
        ValidateEntry(iniSection.Entries["group1.option3"], "group1.option3", "value3");

        var secondSection = ValidateSection(iniFile, "*.{ ext2,ext3,ext4}", 6);
        ValidateEntry(secondSection.Entries["customkey_with_comment"], "customkey_with_comment", "true", "Comment here");
    }

    [Fact]
    public void Can_parse_complex_editorconfig_file()
    {
        var lines = File.ReadAllLines("test-files/complex-editorconfigfile");
        var iniFile = IniFileParser.Parse(lines);
        iniFile.Sections.Should().NotBeNull();
        iniFile.Sections.Should().HaveCount(2);
        var csharpFilesSection = iniFile.Sections.First().Value;
        csharpFilesSection.Entries.Should().NotBeNull().And.HaveCount(211);

        var testsSection = ValidateSection(iniFile, "**/*Tests.cs", 7, ["Test projects overrides"]);
        var testsSectionEntry = testsSection.Entries["dotnet_diagnostic.ca1707.severity"];
        ValidateEntry(testsSectionEntry, "dotnet_diagnostic.ca1707.severity", "none", "Identifiers should not contain underscores");
    }

    private static IniSection ValidateSection(IniFile file,
                                              string sectionName,
                                              int expectedEntriesCount,
                                              IEnumerable<string>? sectionComments = null,
                                              IEnumerable<string>? comments = null)
    {
        file.Sections.Should().ContainKey(sectionName);
        var section = file.Sections[sectionName];

        ValidateSection(section, sectionName, expectedEntriesCount, sectionComments, comments);

        return section;
    }

    private static void ValidateSection(IniSection section,
                                        string sectionName,
                                        int expectedEntriesCount,
                                        IEnumerable<string>? sectionComments = null,
                                        IEnumerable<string>? comments = null)
    {
        section.Should().NotBeNull();
        section!.Name.Should().Be(sectionName);
        section.Entries.Should().HaveCount(expectedEntriesCount);
        if (comments != null && comments.Any())
        {
            section.Comments.Should().Contain(comments);
        }
        else
        {
            section.Comments.Should().BeNullOrEmpty();
        }

        if (sectionComments != null)
        {
            section.SectionComments.Should().Contain(sectionComments);
        }
        else
        {
            section.SectionComments.Should().BeNullOrEmpty();
        }
    }

    private static void ValidateEntry(KeyData? entry, string name, string value, params string[] comments)
    {
        entry.Should().NotBeNull();
        entry!.Key.Should().Be(name);
        entry.Value.Should().Be(value);
        if (comments.Length > 0)
        {
            entry.Comments.Should().Contain(comments);
        }
        else
        {
            entry.Comments.Should().BeNullOrEmpty();
        }
    }
}