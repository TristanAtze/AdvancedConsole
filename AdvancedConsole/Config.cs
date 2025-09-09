using System;

namespace AdvancedConsole;

public sealed class ConsoleTheme
{
    public ConsoleColor Info = ConsoleColor.Cyan;
    public ConsoleColor Success = ConsoleColor.Green;
    public ConsoleColor Warning = ConsoleColor.Yellow;
    public ConsoleColor Error = ConsoleColor.Red;
    public ConsoleColor Accent = ConsoleColor.Magenta;
}

public static class ConsoleX
{
    public static readonly object RenderLock = new();
    public static ConsoleTheme Theme { get; } = new();

    /// <summary>Scoped color write. Resets at the end.</summary>
    public static void WithColor(ConsoleColor? fg, ConsoleColor? bg, Action action)
    {
        if (Ansi.IsEnabled && (fg.HasValue || bg.HasValue))
        {
            var reset = Ansi.Reset;
            if (fg.HasValue) Console.Write(Ansi.Fg(fg.Value));
            if (bg.HasValue) Console.Write(Ansi.Bg(bg.Value));
            action();
            Console.Write(reset);
            return;
        }

        var oldFg = Console.ForegroundColor;
        var oldBg = Console.BackgroundColor;
        if (fg.HasValue) Console.ForegroundColor = fg.Value;
        if (bg.HasValue) Console.BackgroundColor = bg.Value;
        action();
        Console.ForegroundColor = oldFg;
        Console.BackgroundColor = oldBg;
    }
}
