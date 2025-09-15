using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedConsole;

/// <summary>
/// Minimal built-in 5-line "block" font (A-Z, 0-9, basic punctuation).
/// For full FIGlet support you can plug in custom fonts later.
/// </summary>
public static class Figlet
{
    private static readonly Dictionary<char, string[]> Font = new()
    {
        // Very small block letters (5 lines). Add as needed.
        ['A'] = new[] { "  ██  ", " █  █ ", " ████ ", " █  █ ", " █  █ " },
        ['B'] = new[] { " ███  ", " █  █ ", " ███  ", " █  █ ", " ███  " },
        ['C'] = new[] { "  ███ ", " █    ", " █    ", " █    ", "  ███ " },
        ['D'] = new[] { " ███  ", " █  █ ", " █  █ ", " █  █ ", " ███  " },
        ['E'] = new[] { " ████ ", " █    ", " ███  ", " █    ", " ████ " },
        ['F'] = new[] { " ████ ", " █    ", " ███  ", " █    ", " █    " },
        ['G'] = new[] { "  ███ ", " █    ", " █ ██ ", " █  █ ", "  ███ " },
        ['H'] = new[] { " █  █ ", " █  █ ", " ████ ", " █  █ ", " █  █ " },
        ['I'] = new[] { " ███ ", "  █  ", "  █  ", "  █  ", " ███ " },
        ['J'] = new[] { "   ██ ", "    █ ", "    █ ", " █  █ ", "  ██  " },
        ['K'] = new[] { " █  █ ", " █ █  ", " ██   ", " █ █  ", " █  █ " },
        ['L'] = new[] { " █    ", " █    ", " █    ", " █    ", " ████ " },
        ['M'] = new[] { " █  █ ", " ██ ██", " █ ██ ", " █  █ ", " █  █ " },
        ['N'] = new[] { " █  █ ", " ██ █ ", " █ ██ ", " █  █ ", " █  █ " },
        ['O'] = new[] { "  ██  ", " █  █ ", " █  █ ", " █  █ ", "  ██  " },
        ['P'] = new[] { " ███  ", " █  █ ", " ███  ", " █    ", " █    " },
        ['Q'] = new[] { "  ██  ", " █  █ ", " █  █ ", " █ ██ ", "  ███ " },
        ['R'] = new[] { " ███  ", " █  █ ", " ███  ", " █ █  ", " █  █ " },
        ['S'] = new[] { "  ███ ", " █    ", "  ██  ", "    █ ", " ███  " },
        ['T'] = new[] { " █████", "   █  ", "   █  ", "   █  ", "   █  " },
        ['U'] = new[] { " █  █ ", " █  █ ", " █  █ ", " █  █ ", "  ██  " },
        ['V'] = new[] { " █  █ ", " █  █ ", " █  █ ", "  ██  ", "  ██  " },
        ['W'] = new[] { " █  █ ", " █  █ ", " █ ██ ", " ██ ██", " █  █ " },
        ['X'] = new[] { " █  █ ", "  ██  ", "  ██  ", "  ██  ", " █  █ " },
        ['Y'] = new[] { " █  █ ", "  ██  ", "   █  ", "   █  ", "   █  " },
        ['Z'] = new[] { " ████ ", "   ██ ", "  ██  ", " ██   ", " ████ " },
        ['0'] = new[] { "  ██  ", " █  █ ", " █ ██ ", " █  █ ", "  ██  " },
        ['1'] = new[] { "  ██  ", " █ █  ", "   █  ", "   █  ", " ████ " },
        ['2'] = new[] { "  ██  ", " █  █ ", "   █  ", "  █   ", " ████ " },
        ['3'] = new[] { " ███  ", "    █ ", "  ██  ", "    █ ", " ███  " },
        ['4'] = new[] { " █  █ ", " █  █ ", " ████ ", "    █ ", "    █ " },
        ['5'] = new[] { " ████ ", " █    ", " ███  ", "    █ ", " ███  " },
        ['6'] = new[] { "  ██  ", " █    ", " ███  ", " █  █ ", "  ██  " },
        ['7'] = new[] { " ████ ", "    █ ", "   █  ", "  █   ", "  █   " },
        ['8'] = new[] { "  ██  ", " █  █ ", "  ██  ", " █  █ ", "  ██  " },
        ['9'] = new[] { "  ██  ", " █  █ ", "  ███ ", "    █ ", "  ██  " },
        ['!'] = new[] { "  █  ", "  █  ", "  █  ", "     ", "  █  " },
        ['?'] = new[] { " ███ ", "    █", "  ██ ", "     ", "  █  " },
        [' '] = new[] { "    ", "    ", "    ", "    ", "    " },
        ['-'] = new[] { "     ", "     ", " ███ ", "     ", "     " },
        ['.'] = new[] { "   ", "   ", "   ", "   ", " █ " },
        [','] = new[] { "   ", "   ", "   ", " █ ", " █ " },
        [':'] = new[] { "   ", " █ ", "   ", " █ ", "   " },
        [';'] = new[] { "   ", " █ ", "   ", " █ ", " █ " },
        ['\''] = new[] { " █ ", " █ ", "   ", "   ", "   " },
        ['"'] = new[] { " █ █ ", " █ █ ", "     ", "     ", "     " },
        ['('] = new[] { "  █ ", " █  ", " █  ", " █  ", "  █ " },
        [')'] = new[] { " █  ", "  █ ", "  █ ", "  █ ", " █  " },
        ['['] = new[] { " ██ ", " █  ", " █  ", " █  ", " ██ " },
        [']'] = new[] { " ██ ", "  █ ", "  █ ", "  █ ", " ██ " },
        ['+'] = new[] { "     ", "  █  ", " ███ ", "  █  ", "     " },
        ['*'] = new[] { "     ", " █ █ ", "  █  ", " █ █ ", "     " },
        ['='] = new[] { "     ", " ███ ", "     ", " ███ ", "     " },
        ['/'] = new[] { "    █", "   █ ", "  █  ", " █   ", "█    " },
        ['\\'] = new[] { "█    ", " █   ", "  █  ", "   █ ", "    █" },
        ['_'] = new[] { "     ", "     ", "     ", "     ", " ███ " },
    };


    /// <summary>
    /// Writes text using the built-in FIGlet-style font with default height of 5 lines.
    /// </summary>
    /// <param name="text">The text to render.</param>
    /// <param name="color">Optional foreground color to use while rendering.</param>
    public static void Write(string text, ConsoleColor? color = null)
    {
        Write(text, 5, color);
    }

    /// <summary>
    /// Writes text using the built-in FIGlet-style font with a selectable height.
    /// </summary>
    /// <param name="text">The text to render.</param>
    /// <param name="height">The font height in lines. Supported values: 5 or 7.</param>
    /// <param name="color">Optional foreground color to use while rendering.</param>
    public static void Write(string text, int height, ConsoleColor? color = null)
    {
        if (height != 5 && height != 7)
            throw new ArgumentOutOfRangeException(nameof(height), "Only heights 5 or 7 are supported.");

        var lines = new string[height];
        foreach (var ch in text.ToUpperInvariant())
        {
            var baseGlyph = Font.TryGetValue(ch, out var g) ? g : Font['?'];
            var glyph = height == 5 ? baseGlyph : ExpandToSevenLines(baseGlyph);

            for (int i = 0; i < height; i++)
                lines[i] += glyph[i] + " ";
        }

        ConsoleX.WithColor(color, null, () =>
        {
            foreach (var l in lines) Console.WriteLine(l);
        });
    }

    private static string[] ExpandToSevenLines(string[] fiveLineGlyph)
    {
        // Simple vertical expansion: duplicate some rows to achieve 7 lines.
        // Mapping: 0,1,1,2,3,3,4
        var indices = new[] { 0, 1, 1, 2, 3, 3, 4 };
        var seven = new string[7];
        for (int i = 0; i < 7; i++)
        {
            var src = Math.Clamp(indices[i], 0, fiveLineGlyph.Length - 1);
            seven[i] = fiveLineGlyph[src];
        }
        return seven;
    }
}
