using System;
using System.Diagnostics;

namespace AdvancedConsole;

/// <summary>
/// Status line.
/// </summary>
public sealed class StatusLine : IDisposable
{
    private readonly int _top;
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    private string _text;

    /// <summary>
    /// Initialize a new status line.
    /// </summary>
    /// <param name="text"></param>
    public StatusLine(string text)
    {
        _text = text;
        _top = Console.CursorTop;
        Render();
    }

    /// <summary>
    /// Start a new status line.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static StatusLine Start(string text) => new(text);

    /// <summary>
    /// Update the text of the status line.
    /// </summary>
    /// <param name="text"></param>
    public void Update(string text) { _text = text; Render(); }

    /// <summary>
    /// Complete the status line.
    /// </summary>
    /// <param name="text"></param>
    public void Complete(string text = "Done.")
    {
        _text = text;
        Render();
        Console.WriteLine();
    }

    /// <summary>
    /// Render the status line.
    /// </summary>
    private void Render()
    {
        Console.SetCursorPosition(0, _top);
        Console.Write($"{_text} [{_sw.Elapsed:mm\\:ss}]".PadRight(Console.WindowWidth - 1));
    }

    /// <summary>
    /// Dispose the status line.
    /// </summary>
    public void Dispose() => Complete();
}

/// <summary>
/// Console timer.
/// </summary>
public sealed class ConsoleTimer : IDisposable
{
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    private readonly string _label;

    /// <summary>
    /// Initialize a new console timer.
    /// </summary>
    /// <param name="label"></param>
    public ConsoleTimer(string label) { _label = label; }

    /// <summary>
    /// Dispose the console timer.
    /// </summary>
    public void Dispose() => ColorWriter.WriteLineInfo($"{_label}: {_sw.Elapsed}");
}
