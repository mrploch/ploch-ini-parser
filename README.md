# INI File Format Parsing Library

A simple INI file format parsing library.

## Usage

```csharp
var lines = File.ReadAllLines("myfile.ini");
var iniFile = IniFileParser.Parse(lines);

var section = iniFile.Sections["section"];
var entry = section.Entries["key"];
Console.WriteLine($"Key value: {entry.Value}");
foreach (var entryComment in entry.Comments)
{
    Console.WriteLine(entryComment);
}
```
