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

    private static readonly Regex _tag = new(@"\[(?<tag>\/|\/?[A-Za-z]+(?:=[A-Za-z]+)?)\]",
    RegexOptions.Compiled | RegexOptions.CultureInvariant);
    /// <summary>
    /// Write a line with info color.
    /// </summary>
    /// <param name="text"></param>
    public static void WriteLineInfo(string text) => WriteTaggedLine(text, ConsoleX.Theme.Info);
    /// <summary>
    /// Write a line with success color.
    /// </summary>
    /// <param name="text"></param>
    public static void WriteLineSuccess(string text) => WriteTaggedLine(text, ConsoleX.Theme.Success);
    /// <summary>
    /// Write a line with warning color.
    /// </summary>
    /// <param name="text"></param>
    public static void WriteLineWarning(string text) => WriteTaggedLine(text, ConsoleX.Theme.Warning);
    /// <summary>
    /// Write a line with error color.
    /// </summary>
    /// <param name="text"></param>
    public static void WriteLineError(string text) => WriteTaggedLine(text, ConsoleX.Theme.Error);

    /// <summary>
    /// Write a line with tagged color.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="defaultColor"></param>
    public static void WriteTaggedLine(string text, ConsoleColor? defaultColor = null)
    {
        WriteTagged(text, defaultColor);
        Console.WriteLine();
    }

    /// <summary>
    /// Write a line with tagged color.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="defaultColor"></param>
    public static void WriteTagged(string text, ConsoleColor? defaultColor = null)
    {
        if (string.IsNullOrEmpty(text))
            return;

        var fgStack = new Stack<ConsoleColor?>();
        var bgStack = new Stack<ConsoleColor?>();
        bool bold = false, underline = false;

        if (defaultColor.HasValue) fgStack.Push(defaultColor);
        ApplyStyle();

        int lastIndex = 0;

        // Helper: apply current style (ANSI or Console)
        void ApplyStyle()
        {
            var fg = fgStack.Count > 0 ? fgStack.Peek() : (ConsoleColor?)null;
            var bg = bgStack.Count > 0 ? bgStack.Peek() : (ConsoleColor?)null;

            if (Ansi.IsEnabled)
            {
                Console.Write(Ansi.Reset);
                if (fg.HasValue) Console.Write(Ansi.Fg(fg.Value));
                if (bg.HasValue) Console.Write(Ansi.Bg(bg.Value));
                if (bold) Console.Write(Ansi.Bold);
                if (underline) Console.Write(Ansi.Underline);
            }
            else
            {
                // non-ANSI: set Console colors
                var oldFg = Console.ForegroundColor;
                var oldBg = Console.BackgroundColor;
                if (fg.HasValue) Console.ForegroundColor = fg.Value;
                if (bg.HasValue) Console.BackgroundColor = bg.Value;
                // we don't buffer old colors per char; we rely on explicit resets below
            }
        }

        void ResetStyle()
        {
            if (Ansi.IsEnabled)
            {
                Console.Write(Ansi.Reset);
            }
            else
            {
                // reset to defaults
                Console.ResetColor();
            }
        }

        foreach (Match m in _tag.Matches(text))
        {
            // write literal chunk before tag with current style
            var literal = text.AsSpan(lastIndex, m.Index - lastIndex);
            if (!literal.IsEmpty)
                Console.Write(literal.ToString());


            lastIndex = m.Index + m.Length;

            var tag = m.Groups["tag"].Value;

            // Close tag: [/]
            if (tag == "/")
            {
                // pop one level each
                if (fgStack.Count > 0) fgStack.Pop();
                if (bgStack.Count > 0) bgStack.Pop();
                bold = false; underline = false;

                ApplyStyle();
                continue;
            }

            // [bold], [underline]
            if (tag.Equals("bold", StringComparison.OrdinalIgnoreCase)) { bold = true; ApplyStyle(); continue; }
            if (tag.Equals("underline", StringComparison.OrdinalIgnoreCase)) { underline = true; ApplyStyle(); continue; }

            // [bg=...]
            if (tag.StartsWith("bg=", StringComparison.OrdinalIgnoreCase))
            {
                var name = tag.Split('=')[1];
                if (_colors.TryGetValue(name, out var c))
                {
                    bgStack.Push(c);
                    ApplyStyle();
                }
                continue;
            }

            // [red], [cyan], ...
            if (_colors.TryGetValue(tag, out var col))
            {
                fgStack.Push(col);
                ApplyStyle();
                continue;
            }

            // Unknown tag → ignore
        }

        // tail after last match
        if (lastIndex < text.Length)
            Console.Write(text.AsSpan(lastIndex).ToString());


        // final reset
        ResetStyle();
    }



    /// <summary>
    /// Get ANSI code for current style.
    /// </summary>
    /// <param name="fg"></param>
    /// <param name="bg"></param>
    /// <param name="bold"></param>
    /// <param name="underline"></param>
    /// <returns></returns>
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
