using Ploch.IniParser;

Console.WriteLine("Enter the path to the file: (press enter to use the provided .editorconfig file)");
var filePath = Console.ReadLine();

filePath = string.IsNullOrWhiteSpace(filePath) ? ".editorconfig" : filePath;

if (string.IsNullOrWhiteSpace(filePath))
{
    Console.WriteLine("No file path provided. Exiting.");
    return;
}

if (!File.Exists(filePath))
{
    Console.WriteLine("File does not exist. Exiting.");
    return;
}

var fileLines = await File.ReadAllLinesAsync(filePath);
var iniFile = IniFileParser.Parse(fileLines);

Console.WriteLine("Parsed INI file:");
Console.WriteLine();
Console.WriteLine("=====================================");
Console.WriteLine("Global Section:");
Console.WriteLine("=====================================");
ProcessSection(iniFile.GlobalSection);

Console.WriteLine();
Console.WriteLine("=====================================");
Console.WriteLine("Sections:");
Console.WriteLine("=====================================");
Console.WriteLine();
foreach (var section in iniFile.Sections.Select(s => s.Value))
{
    ProcessSection(section);
}

// Accessing entries by name
Console.WriteLine();
var iniFileSection = iniFile.Sections["**/*Tests.cs"];
var keyData = iniFileSection.Entries["dotnet_diagnostic.sa0001.severity"];
Console.WriteLine($"Section {iniFileSection.Name}, Key: {keyData.Key}, Value: {keyData.Value}");

// Check if there are any duplicate sections. Those extend section with the same name, but can also be accessed like this:
var duplicateSections = iniFile.DuplicateSections;
if (duplicateSections.Any())
{
    Console.WriteLine("Duplicate sections found:");
    foreach (var section in duplicateSections)
    {
        Console.WriteLine($"***** Duplicate Section {section.Key} *****");
        ProcessSection(section.Value);
    }
}
else
{
    Console.WriteLine("No duplicate sections found.");
}

static void ProcessSection(IniSection section)
{
    Console.WriteLine($"***** Section [{section.Name}] *****");
    Console.WriteLine("Section Comments:");
    foreach (var comment in section.SectionComments)
    {
        Console.WriteLine(comment);
    }

    Console.WriteLine("All Comments in the section:");
    foreach (var comment in section.Comments)
    {
        Console.WriteLine(comment);
    }

    Console.WriteLine();
    Console.WriteLine("***** Entries: *****");
    foreach (var entry in section.Entries.Select(e => e.Value))
    {
        if (entry.Comments.Any())
        {
            Console.WriteLine("Entry comments:");
            foreach (var comment in entry.Comments)
            {
                Console.WriteLine(comment);
            }
        }

        Console.WriteLine($"Key data: {entry.Key}={entry.Value}");
    }

    Console.WriteLine("***** Duplicate Entries: *****");
    foreach (var duplicateEntry in section.DuplicateEntries)
    {
        Console.WriteLine($"Duplicate Key data: {duplicateEntry.Key}={duplicateEntry.Value}");
    }

    Console.WriteLine("-------------------------------------");
}