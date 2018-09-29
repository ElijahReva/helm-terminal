using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class ReleaseNote
{
    public string Content { get; }

    public ReleaseNote(string content)
    {
        Content = content;
    }

    public override string ToString() => Content;
}
    
public class ReleaseNotes
{
    public Version Version { get; }
    public IReadOnlyList<ReleaseNote> Notes { get; }

    public ReleaseNotes(Version version, IReadOnlyList<ReleaseNote> notes)
    {
        Version = version;
        Notes = notes;
    }
        
    public override string ToString() => string.Join(Environment.NewLine, Notes);
}

public class Changelog
{
    public Version CurrentVersion => Current.Version;
    public IReadOnlyList<ReleaseNote> CurrentNotes => Current.Notes;

    ReleaseNotes Current => AllReleases.First();
    public IReadOnlyList<ReleaseNotes> AllReleases { get; }

    public Changelog(IReadOnlyList<ReleaseNotes> allReleases)
    {
        AllReleases = allReleases;
    }

    public override string ToString() => Current.ToString();
}

public static class ChangelogService
{
    public static Changelog ReadChangelog(string path)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Changelog not found", path);
        }

        var lines = File.ReadAllLines(path);
        if (lines.Length > 0)
        {
            var line = lines[0].Trim();

            if (line.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                return Parse(lines);
            }
        }

        throw new FormatException("Unknown changelog format.");
    }

    private static Changelog Parse(string[] lines)
    {
        var versionRegex = new Regex(@"([0-9]+\.)+[0-9]+");
        var lineIndex = 0;
        var result = new List<ReleaseNotes>();

        while (true)
        {
            if (lineIndex >= lines.Length)
            {
                break;
            }

            // Parse header.
            var versionLine = lines[lineIndex];
            var versionResult = versionRegex.Match(versionLine);
            if (!versionResult.Success)
            {
                throw new FormatException("Could not parse version from release notes header.");
            }

            // Create release notes.
            var version = Version.Parse(versionResult.Value);

            // Increase the line index.
            lineIndex++;

            // Parse content.
            var notes = new List<ReleaseNote>();
            while (true)
            {
                // Sanity checks.
                if (lineIndex >= lines.Length)
                {
                    break;
                }
                    
                var currentLine = lines[lineIndex];
                if (currentLine.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                // Get the current line.
                var line = (currentLine ?? string.Empty).Trim('*').Trim();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    notes.Add(new ReleaseNote(line));
                }

                lineIndex++;
            }

            result.Add(new ReleaseNotes(version, notes.AsReadOnly()));
        }

        return new Changelog(result.OrderByDescending(x => x.Version).ToList().AsReadOnly());
           
    }
}