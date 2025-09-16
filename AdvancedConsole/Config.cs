using System;

namespace AdvancedConsole;

/// <summary>
/// Console theme.
/// </summary>
public sealed class ConsoleTheme
{
    /// <summary>
    /// Info color.
    /// </summary>
    public ConsoleColor Info = ConsoleColor.Cyan;
    /// <summary>
    /// Success color.
    /// </summary>
    public ConsoleColor Success = ConsoleColor.Green;
    /// <summary>
    /// Warning color.
    /// </summary>
    public ConsoleColor Warning = ConsoleColor.Yellow;
    /// <summary>
    /// Error color.
    /// </summary>
    public ConsoleColor Error = ConsoleColor.Red;
    /// <summary>
    /// Accent color.
    /// </summary>
    public ConsoleColor Accent = ConsoleColor.Magenta;
}

/// <summary>
/// Console helpers.
/// </summary>
public static class ConsoleX
{
    /// <summary>
    /// Render lock.
    /// </summary>
    public static readonly object RenderLock = new();
    /// <summary>
    /// Console theme.
    /// </summary>
    public static ConsoleTheme Theme { get; } = new();

    /// <summary>
    /// Scoped color write. Resets at the end.
    /// </summary>
    /// <param name="fg"></param>
    /// <param name="bg"></param>
    /// <param name="action"></param>
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
