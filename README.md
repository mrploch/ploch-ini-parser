# Ploch INI Parser Library

## Overview

**Ploch.IniParser** is a small library for reading and parsing INI files in .NET.

It is implemented using .NET Standard 2.0, so it can be used in any .NET project that supports .NET Standard 2.0.
Besides handling of INI files, tt can be also used to parse the `.editorconfig` files, which are in the INI format.

## Features

- .NET Standard 2.0 library
- Parsing of INI files section
- Handling of comments in the INI file for both sections and entries
- Detection of duplicate sections and entries

## Usage

For a full sample see
the [sample project](https://github.com/mrploch/ploch-ini-parser/tree/main/samples/IniParserExample/Program.cs).

```csharp
var fileLines = await File.ReadAllLinesAsync(filePath);
var iniFile = IniFileParser.Parse(fileLines);

// ===================================================
// Reading global section contents
// ===================================================
foreach (var comment in iniFile.GlobalSection.Comments) // Comments in the global section
{
    Console.WriteLine(comment);
}

foreach (var (key, value) in iniFile.GlobalSection.Entries) // Entries in the global section
{
    Console.WriteLine($"{key}={value.Value}");
}

// ===================================================
// Reading ini file sections
// ===================================================
foreach (var section in iniFile.Sections.Select(s => s.Value))
{
    Console.WriteLine($"***** Section [{section.Name}] *****");
    Console.WriteLine("Comments:");
    foreach (var comment in section.Comments) // Comments in the section
    {
        Console.WriteLine(comment);
    }

    Console.WriteLine("***** Entries: *****");
    foreach (var entry in section.Entries.Select(e => e.Value)) // Entries in the section
    {
        // Checking if there are any comments for the entry
        // Comments are attached to an entry if they are on the same line as the entry or the line above the entry
        if (entry.Comments.Any())  
        {
            Console.WriteLine("Entry comments:");
            foreach (var comment in entry.Comments)
            {
                Console.WriteLine(comment);
            }
        }

        Console.WriteLine($"Key data: {entry.Key}={entry.Value}"); // Entry key and value
    }

    Console.WriteLine("***** Duplicate Entries: *****"); // Duplicate entries in the section
    foreach (var duplicateEntry in section.DuplicateEntries)
    {
        Console.WriteLine($"Duplicate Key data: {duplicateEntry.Key}={duplicateEntry.Value}");
    }

    Console.WriteLine("-------------------------------------");
}
```

There is also a way to check if there are any duplicate sections in the file. Those extend sections with the same name,
but they are also places in the `DuplicateSections` property of the `IniFile` object.

```csharp
var duplicateSections = iniFile.DuplicateSections;
foreach (var section in duplicateSections)
{
    Console.WriteLine($"Duplicate Section [{section.Name}]");
    foreach (var entry in section.Entries.Select(e => e.Value))
    {
        Console.WriteLine($"Key data: {entry.Key}={entry.Value}");
    }
}
```

### Reading of INI files

There is a few methods for loading of the INI contents.

File can be read by providing a file path:

```csharp
var iniFile = await IniFileParser.ParseAsync("my-ini-file.ini");
```

Using a string contents of the file:

```csharp
var text = await File.ReadAllTextAsync("my-ini-file.ini");
var iniFile = IniFileParser.Parse(text);
```

Using file lines:

```csharp
var lines = await File.ReadAllLinesAsync(filePath)`
var iniFile = IniFileParser.Parse(lines);
```

Using a stream, for example:

```csharp
using var stream = File.OpenRead(filePath);
var iniFile = await IniFileParser.ParseAsync(stream);
```

### Comments Handling

Comments that are immediately before a section are stored in the [
[`IniSection.SectionComments`](https://github.com/mrploch/ploch-ini-parser/tree/main/src/IniParser/IniSection.cs#L24)
property.
Those comments are assumed to be directly refering to the section.

All comments within a section are also stored in the [
[`IniSection.Comments`](https://github.com/mrploch/ploch-ini-parser/tree/main/src/IniParser/IniSection.cs#L19) property.

Comments for an entry (which are either on the same line as the entry, or are immediately above the entry) are stored in
the
[`KeyData.Comments`](https://github.com/mrploch/ploch-ini-parser/tree/main/src/IniParser/KeyData.cs#L9) property.
