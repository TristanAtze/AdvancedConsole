using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdvancedConsole;

/// <summary>
/// Colored output + inline color tags.
/// Tags: [red], [green], [blue], [yellow], [magenta], [cyan], [white], [gray], [bold], [underline], [bg=red], [/]
/// Example: WriteTagged("[red]Error[/]: [yellow]something[/] broke")
/// </summary>
public static class ColorWriter
{
    private static readonly Dictionary<string, ConsoleColor> _colors = new(StringComparer.OrdinalIgnoreCase)
    {
        ["black"] = ConsoleColor.Black,
        ["darkred"] = ConsoleColor.DarkRed,
        ["darkgreen"] = ConsoleColor.DarkGreen,
        ["darkyellow"] = ConsoleColor.DarkYellow,
        ["darkblue"] = ConsoleColor.DarkBlue,
        ["darkmagenta"] = ConsoleColor.DarkMagenta,
        ["darkcyan"] = ConsoleColor.DarkCyan,
        ["gray"] = ConsoleColor.Gray,
        ["darkgray"] = ConsoleColor.DarkGray,
        ["red"] = ConsoleColor.Red,
        ["green"] = ConsoleColor.Green,
        ["yellow"] = ConsoleColor.Yellow,
        ["blue"] = ConsoleColor.Blue,
        ["magenta"] = ConsoleColor.Magenta,
        ["cyan"] = ConsoleColor.Cyan,
        ["white"] = ConsoleColor.White,
    };

    private static readonly Regex _tag = new(@"\[(?<tag>/?[a-zA-Z]+(?:=[a-zA-Z]+)?)\]", RegexOptions.Compiled);

    public static void WriteLineInfo(string text) => WriteTaggedLine(text, ConsoleX.Theme.Info);
    public static void WriteLineSuccess(string text) => WriteTaggedLine(text, ConsoleX.Theme.Success);
    public static void WriteLineWarning(string text) => WriteTaggedLine(text, ConsoleX.Theme.Warning);
    public static void WriteLineError(string text) => WriteTaggedLine(text, ConsoleX.Theme.Error);

    public static void WriteTaggedLine(string text, ConsoleColor? defaultColor = null)
    {
        WriteTagged(text, defaultColor);
        Console.WriteLine();
    }

    public static void WriteTagged(string text, ConsoleColor? defaultColor = null)
    {
        if (!Ansi.IsEnabled)
        {
            // Strip ALL [tags] inkl. [/], [bg=...], [bold], [underline], [color]
            string plain = _tag.Replace(text, string.Empty);
            ConsoleX.WithColor(defaultColor, null, () => Console.Write(plain));
            return;
        }

        var sb = new StringBuilder();
        var fgStack = new Stack<ConsoleColor?>();
        var bgStack = new Stack<ConsoleColor?>();
        bool bold = false, underline = false;

        int lastIndex = 0;
        foreach (Match m in _tag.Matches(text))
        {
            sb.Append(text.AsSpan(lastIndex, m.Index - lastIndex));
            lastIndex = m.Index + m.Length;

            var tag = m.Groups["tag"].Value;
            if (tag.Equals("/", StringComparison.Ordinal))
            {
                if (fgStack.Count > 0) sb.Append(Ansi.Reset).Append(GetAnsi(fgStack.Pop(), bgStack.Count > 0 ? bgStack.Peek() : null, bold, underline));
                if (bgStack.Count > 0) { /* handled by GetAnsi */ }
                bold = false; underline = false;
                continue;
            }

            if (tag.Equals("bold", StringComparison.OrdinalIgnoreCase)) { bold = true; sb.Append("\u001b[1m"); continue; }
            if (tag.Equals("underline", StringComparison.OrdinalIgnoreCase)) { underline = true; sb.Append("\u001b[4m"); continue; }

            if (tag.StartsWith("bg=", StringComparison.OrdinalIgnoreCase))
            {
                var name = tag.Split('=')[1];
                if (_colors.TryGetValue(name, out var c))
                {
                    bgStack.Push(c);
                    sb.Append(Ansi.Bg(c));
                }
                continue;
            }

            if (_colors.TryGetValue(tag, out var col))
            {
                fgStack.Push(col);
                sb.Append(Ansi.Fg(col));
            }
        }

        sb.Append(text.AsSpan(lastIndex));

        if (defaultColor.HasValue)
            Console.Write(Ansi.Fg(defaultColor.Value));

        Console.Write(sb.ToString());
        Console.Write(Ansi.Reset);
    }


    private static string GetAnsi(ConsoleColor? fg, ConsoleColor? bg, bool bold, bool underline)
    {
        var sb = new StringBuilder();
        if (fg.HasValue) sb.Append(Ansi.Fg(fg.Value));
        if (bg.HasValue) sb.Append(Ansi.Bg(bg.Value));
        if (bold) sb.Append(Ansi.Bold);
        if (underline) sb.Append(Ansi.Underline);
        return sb.ToString();
    }
}
