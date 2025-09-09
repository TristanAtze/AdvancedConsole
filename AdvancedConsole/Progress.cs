using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedConsole;

public sealed class ProgressBar : IDisposable
{
    private readonly int _width;
    private readonly int _top;
    private readonly CancellationTokenSource _cts = new();
    private int _value;
    private string _label = "";

    public ProgressBar(string label = "", int width = 40)
    {
        _width = Math.Max(10, width);
        _top = Console.CursorTop;
        _label = label;
        Draw();
    }

    public void Report(double progress, string? label = null)
    {
        _value = (int)Math.Clamp(Math.Round(progress * _width), 0, _width);
        if (label != null) _label = label;
        Draw();
    }

    private void Draw()
    {
        int left = Console.CursorLeft;
        int top = Console.CursorTop;

        Console.SetCursorPosition(0, _top);
        var bar = new string('█', _value) + new string('─', _width - _value);
        Console.Write($"[{bar}] {_label.PadRight(30)}");

        Console.SetCursorPosition(left, top);
    }

    public void Dispose()
    {
        Console.SetCursorPosition(0, _top);
        Console.WriteLine(new string(' ', _width + 35));
    }
}

public sealed class Spinner : IDisposable
{
    private readonly string[] _frames = new[] { "⠋", "⠙", "⠸", "⠴", "⠦", "⠇" };
    private readonly CancellationTokenSource _cts = new();
    private readonly Task _loop;
    private readonly int _top;
    private string _text;

    public Spinner(string text = "Working…", int intervalMs = 80)
    {
        _text = text;
        _top = Console.CursorTop;
        _loop = Task.Run(async () =>
        {
            int i = 0;
            while (!_cts.Token.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, _top);
                Console.Write($"{_frames[i % _frames.Length]} {_text}   ");
                i++;
                try { await Task.Delay(intervalMs, _cts.Token); } catch { }
            }
        });
    }

    public void Update(string text) => _text = text;

    public void Dispose()
    {
        _cts.Cancel();
        try { _loop.Wait(200); } catch { }
        Console.SetCursorPosition(0, _top);
        Console.WriteLine(new string(' ', 60));
    }
}
