using System;

namespace AdvancedConsole;

/// <summary>
/// Fluent API for temporary styling.
/// </summary>
public sealed class Styled
{
    private readonly ConsoleColor? _fg;
    private readonly ConsoleColor? _bg;
    private readonly bool _bold;
    private readonly bool _underline;

    private Styled(ConsoleColor? fg, ConsoleColor? bg, bool bold, bool underline)
    { _fg = fg; _bg = bg; _bold = bold; _underline = underline; }

    public static Styled WithForeground(ConsoleColor color) => new(color, null, false, false);
    public Styled WithBackground(ConsoleColor color) => new(_fg, color, _bold, _underline);
    public Styled Bold() => new(_fg, _bg, true, _underline);
    public Styled Underline() => new(_fg, _bg, _bold, true);

    public void Write(string text)
    {
        if (Ansi.IsEnabled)
        {
            if (_fg.HasValue) Console.Write(Ansi.Fg(_fg.Value));
            if (_bg.HasValue) Console.Write(Ansi.Bg(_bg.Value));
            if (_bold) Console.Write(Ansi.Bold);
            if (_underline) Console.Write(Ansi.Underline);
            Console.Write(text);
            Console.Write(Ansi.Reset);
        }
        else
        {
            ConsoleX.WithColor(_fg, _bg, () => Console.Write(text));
        }
    }

    public void WriteLine(string text) { Write(text); Console.WriteLine(); }
}
