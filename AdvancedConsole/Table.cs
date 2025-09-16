using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedConsole;

/// <summary>
/// Table.
/// </summary>
public sealed class Table
{
    private readonly List<string[]> _rows = new();
    private string[]? _headers;
    private ConsoleColor? _borderColor = null;

    /// <summary>
    /// With headers.
    /// </summary>
    /// <param name="headers"></param>
    /// <returns></returns>
    public Table WithHeaders(params string[] headers) { _headers = headers; return this; }
    /// <summary>
    /// Add a row.
    /// </summary>
    /// <param name="cells"></param>
    /// <returns></returns>
    public Table AddRow(params string[] cells) { _rows.Add(cells); return this; }
    /// <summary>
    /// With border color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public Table WithBorderColor(ConsoleColor color) { _borderColor = color; return this; }

    /// <summary>
    /// Write the table.
    /// </summary>
    public void Write()
    {
        var all = new List<string[]>();
        if (_headers is not null) all.Add(_headers);
        all.AddRange(_rows);
        int cols = all.Max(r => r.Length);

        var widths = new int[cols];
        foreach (var r in all)
            for (int i = 0; i < r.Length; i++)
                widths[i] = Math.Max(widths[i], r[i]?.Length ?? 0);

        void Line(char l, char m, char r, char h)
        {
            ConsoleX.WithColor(_borderColor, null, () =>
            {
                Console.Write(l);
                for (int i = 0; i < cols; i++)
                {
                    Console.Write(new string(h, widths[i] + 2));
                    Console.Write(i == cols - 1 ? r : m);
                }
                Console.WriteLine();
            });
        }

        Line('┌', '┬', '┐', '─');
        if (_headers is not null)
        {
            Row(_headers, widths, bold: true);
            Line('├', '┼', '┤', '─');
        }
        foreach (var r in _rows) Row(r, widths, bold: false);
        Line('└', '┴', '┘', '─');
    }

    /// <summary>
    /// Write a row.
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="widths"></param>
    /// <param name="bold"></param>
    private static void Row(string[] cells, int[] widths, bool bold)
    {
        Console.Write('│');
        for (int i = 0; i < widths.Length; i++)
        {
            var text = i < cells.Length ? cells[i] ?? "" : "";
            if (bold && Ansi.IsEnabled) Console.Write(Ansi.Bold);
            Console.Write(" " + text.PadRight(widths[i]) + " ");
            if (bold && Ansi.IsEnabled) Console.Write(Ansi.Reset);
            Console.Write(i == widths.Length - 1 ? '│' : '│');
        }
        Console.WriteLine();
    }
}
