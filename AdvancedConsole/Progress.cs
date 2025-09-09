using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedConsole;

public sealed class ProgressBar : IDisposable
{
    private readonly int _width;
    private readonly int _top;
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
        lock (ConsoleX.RenderLock)
        {
            int barVal = Math.Clamp(_value, 0, _width);
            string bar = new string('█', barVal) + new string('─', _width - barVal);

            // komplette Zeile bauen und rechts auffüllen -> keine Reste, kein Flackern
            string line = $"[{bar}] {_label}";
            int padWidth = Math.Max(0, (Console.WindowWidth > 1 ? Console.WindowWidth - 1 : _width + 35) - line.Length);
            if (padWidth > 0) line += new string(' ', padWidth);

            int oldLeft = Console.CursorLeft;
            int oldTop = Console.CursorTop;

            Console.SetCursorPosition(0, _top);
            Console.Write(line);

            // Cursor da lassen, wo er war (keine sichtbaren Sprünge)
            Console.SetCursorPosition(oldLeft, oldTop);
        }
    }

    public void Dispose()
    {
        lock (ConsoleX.RenderLock)
        {
            int width = Console.WindowWidth > 1 ? Console.WindowWidth - 1 : _width + 35;
            Console.SetCursorPosition(0, _top);
            Console.Write(new string(' ', width));
        }
    }
}


public sealed class Spinner : IDisposable
{
    private readonly string[] _frames = new[] { "⠋", "⠙", "⠸", "⠴", "⠦", "⠇" };
    private readonly CancellationTokenSource _cts = new();
    private readonly Task _loop;
    private readonly int _top;
    private string _text;
    private readonly int _intervalMs;

    public Spinner(string text = "Working…", int intervalMs = 80)
    {
        _text = text;
        _intervalMs = intervalMs;

        // Nimmt IMMER eine eigene Zeile unterhalb der aktuellen ein
        lock (ConsoleX.RenderLock)
        {
            Console.WriteLine();               // reserviere neue Zeile
            _top = Console.CursorTop - 1;      // gehe auf die soeben geschriebene Zeile
        }

        _loop = Task.Run(Loop);
    }

    private async Task Loop()
    {
        int i = 0;
        while (!_cts.IsCancellationRequested)
        {
            lock (ConsoleX.RenderLock)
            {
                int width = Math.Max(20, (Console.WindowWidth > 1 ? Console.WindowWidth - 1 : 60));
                string frameText = $"{_frames[i % _frames.Length]} {_text}";
                if (frameText.Length < width)
                    frameText = frameText.PadRight(width);

                int oldLeft = Console.CursorLeft;
                int oldTop = Console.CursorTop;

                Console.SetCursorPosition(0, _top);
                Console.Write(frameText);

                Console.SetCursorPosition(oldLeft, oldTop);
            }

            i++;
            try { await Task.Delay(_intervalMs, _cts.Token); }
            catch { break; }
        }
    }

    public void Update(string text) => _text = text;

    public void Dispose()
    {
        _cts.Cancel();
        try { _loop.Wait(200); } catch { }

        lock (ConsoleX.RenderLock)
        {
            int width = Console.WindowWidth > 1 ? Console.WindowWidth - 1 : 60;
            Console.SetCursorPosition(0, _top);
            Console.Write(new string(' ', width));
        }
    }
}

