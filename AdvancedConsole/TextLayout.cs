using System;
using System.Collections.Generic;

namespace AdvancedConsole;

/// <summary>
/// Text layout.
/// </summary>
public static class TextLayout
{
    /// <summary>
    /// Word wrap text.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="width"></param>
    /// <param name="indent"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static IEnumerable<string> WordWrap(string text, int width, int indent = 0, string? prefix = null)
    {
        width = Math.Max(10, width);
        var words = (text ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var line = new System.Text.StringBuilder();
        foreach (var w in words)
        {
            if (line.Length + w.Length + 1 > width)
            {
                yield return Indent(prefix, indent) + line.ToString();
                line.Clear();
            }
            if (line.Length > 0) line.Append(' ');
            line.Append(w);
        }
        if (line.Length > 0) yield return Indent(prefix, indent) + line.ToString();
    }

    /// <summary>
    /// Section text.
    /// </summary>
    /// <param name="title"></param>
    public static void Section(string title)
    {
        var line = new string('─', Math.Max(8, Math.Min(Console.WindowWidth - 2, title.Length + 6)));
        ColorWriter.WriteTaggedLine($"[magenta]{title}[/]");
        Console.WriteLine(line);
    }

    /// <summary>
    /// Indent scope.
    /// </summary>
    /// <param name="spaces"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static IDisposable IndentScope(int spaces = 2, string? prefix = null) => new Indentation(spaces, prefix);

    private static string Indent(string? prefix, int indent) => (prefix ?? "") + new string(' ', Math.Max(0, indent));

    /// <summary>
    /// Indentation.
    /// </summary>
    private sealed class Indentation : IDisposable
    {
        private readonly int _spaces;
        private readonly string? _prefix;
        public Indentation(int spaces, string? prefix) { _spaces = spaces; _prefix = prefix; Console.Write(new string(' ', _spaces)); if (!string.IsNullOrEmpty(_prefix)) Console.Write(_prefix); }
        public void Dispose() { /* purely visual scope; nothing persistent */ }
    }
}
