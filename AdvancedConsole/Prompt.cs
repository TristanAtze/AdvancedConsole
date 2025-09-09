using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdvancedConsole;

public static class Prompt
{
    public static bool YesNo(string question, bool defaultYes = true)
    {
        ColorWriter.WriteTagged($"{question} [");
        ColorWriter.WriteTagged(defaultYes ? "[green]Y[/]/n" : "y/[red]N[/]");
        ColorWriter.WriteTagged("]: ");
        while (true)
        {
            var key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.Enter) { Console.WriteLine(defaultYes ? "y" : "n"); return defaultYes; }
            if (key == ConsoleKey.Y) { Console.WriteLine("y"); return true; }
            if (key == ConsoleKey.N) { Console.WriteLine("n"); return false; }
        }
    }

    public static string Input(string prompt, string? defaultValue = null, Func<string, (bool ok, string? error)>? validator = null)
    {
        ColorWriter.WriteTagged($"{prompt} ");
        if (!string.IsNullOrEmpty(defaultValue)) ColorWriter.WriteTagged($"([cyan]{defaultValue}[/]) ");
        ColorWriter.WriteTagged(": ");
        while (true)
        {
            string? s = Console.ReadLine();
            if (string.IsNullOrEmpty(s)) s = defaultValue ?? "";
            if (validator is null) return s;
            var (ok, err) = validator(s);
            if (ok) return s;
            ColorWriter.WriteLineError(err ?? "Invalid input.");
            ColorWriter.WriteTagged(": ");
        }
    }

    public static string Password(string prompt, char mask = '*')
    {
        ColorWriter.WriteTagged($"{prompt}: ");
        var sb = new System.Text.StringBuilder();
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
            if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
                Console.Write("\b \b");
                continue;
            }
            if (!char.IsControl(key.KeyChar))
            {
                sb.Append(key.KeyChar);
                Console.Write(mask);
            }
        }
        return sb.ToString();
    }

    public static T Select<T>(string title, IReadOnlyList<T> options, int? defaultIndex = 0)
    {
        if (options.Count == 0) throw new ArgumentException("Options empty");
        int index = Math.Clamp(defaultIndex ?? 0, 0, options.Count - 1);

        int topStart = Console.CursorTop;
        Console.CursorVisible = false;
        try
        {
            while (true)
            {
                Console.SetCursorPosition(0, topStart);
                WriteLineCleared(title);

                for (int i = 0; i < options.Count; i++)
                {
                    string line = $"{(i == index ? "> " : "  ")}{options[i]}";
                    if (Ansi.IsEnabled)
                    {
                        if (i == index) ColorWriter.WriteTaggedLine($"[green]{line}[/]");
                        else WriteLineCleared(line);
                    }
                    else
                    {
                        if (i == index) ConsoleX.WithColor(ConsoleColor.Green, null, () => WriteLineCleared(line));
                        else WriteLineCleared(line);
                    }
                }

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow: index = (index - 1 + options.Count) % options.Count; break;
                    case ConsoleKey.DownArrow: index = (index + 1) % options.Count; break;
                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(0, topStart + options.Count + 1);
                        return options[index];
                }
            }
        }
        finally { Console.CursorVisible = true; }
    }

    public static IReadOnlyList<T> MultiSelect<T>(string title, IReadOnlyList<T> options, IReadOnlyCollection<int>? defaults = null)
    {
        if (options.Count == 0) throw new ArgumentException("Options empty");
        var selected = new HashSet<int>(defaults ?? Array.Empty<int>());
        int index = 0;

        int topStart = Console.CursorTop;
        Console.CursorVisible = false;
        try
        {
            while (true)
            {
                Console.SetCursorPosition(0, topStart);
                WriteLineCleared(title + " (Space to toggle, Enter to confirm)");

                for (int i = 0; i < options.Count; i++)
                {
                    bool isSel = selected.Contains(i);
                    string marker = isSel ? "●" : "○";
                    string line = $"{(i == index ? "> " : "  ")}{marker} {options[i]}";

                    if (Ansi.IsEnabled)
                    {
                        if (i == index) ColorWriter.WriteTaggedLine($"[green]{line}[/]");
                        else WriteLineCleared(line);
                    }
                    else
                    {
                        if (i == index) ConsoleX.WithColor(ConsoleColor.Green, null, () => WriteLineCleared(line));
                        else WriteLineCleared(line);
                    }
                }

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow: index = (index - 1 + options.Count) % options.Count; break;
                    case ConsoleKey.DownArrow: index = (index + 1) % options.Count; break;
                    case ConsoleKey.Spacebar:
                        if (!selected.Add(index)) selected.Remove(index);
                        break;
                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(0, topStart + options.Count + 1);
                        return selected.OrderBy(i => i).Select(i => options[i]).ToArray();
                }
            }
        }
        finally { Console.CursorVisible = true; }
    }

    /// <summary>Writes a line and pads with spaces to clear artifacts from previous renders.</summary>
    private static void WriteLineCleared(string text)
    {
        int width = Math.Max(1, Console.WindowWidth - 1);
        if (text.Length > width) text = text[..width];
        Console.WriteLine(text.PadRight(width));
    }

}
