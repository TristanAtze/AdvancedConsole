using System;
using System.Diagnostics;

namespace AdvancedConsole;

public sealed class StatusLine : IDisposable
{
    private readonly int _top;
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    private string _text;

    public StatusLine(string text)
    {
        _text = text;
        _top = Console.CursorTop;
        Render();
    }

    public static StatusLine Start(string text) => new(text);

    public void Update(string text) { _text = text; Render(); }

    public void Complete(string text = "Done.")
    {
        _text = text;
        Render();
        Console.WriteLine();
    }

    private void Render()
    {
        Console.SetCursorPosition(0, _top);
        Console.Write($"{_text} [{_sw.Elapsed:mm\\:ss}]".PadRight(Console.WindowWidth - 1));
    }

    public void Dispose() => Complete();
}

public sealed class ConsoleTimer : IDisposable
{
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    private readonly string _label;
    public ConsoleTimer(string label) { _label = label; }
    public void Dispose() => ColorWriter.WriteLineInfo($"{_label}: {_sw.Elapsed}");
}
