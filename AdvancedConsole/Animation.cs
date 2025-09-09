using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedConsole;

public static partial class Animation
{
    public static async Task Marquee(
    string text,
    int width = 30,
    int ms = 100,
    int loops = -1,           // -1 => endless, sonst: wie oft der gesamte Scroll-String durchlaufen wird
    bool dynamicWidth = false,
    CancellationToken ct = default)
    {
        text ??= string.Empty;
        width = Math.Max(10, width);

        string spacer = new string(' ', width);
        string src = text + spacer;
        if (src.Length == 0) return;

        // Remember line to redraw in place
        int top = Console.CursorTop;
        int left = 0; // always draw from col 0 to avoid wrap artifacts

        // How many total ticks? (-1 means endless)
        long totalTicks = loops < 0 ? long.MaxValue : (long)loops * src.Length;

        // Local helper: get a wrapped slice of length 'w' starting at 'start'
        static string WrapSlice(string s, int start, int w)
        {
            if (w <= 0) return string.Empty;
            // Fast path: no wrap needed
            if (start + w <= s.Length) return s.AsSpan(start, w).ToString();

            // Wrap around
            int first = s.Length - start;
            var sb = new System.Text.StringBuilder(w);
            sb.Append(s.AsSpan(start, first));
            sb.Append(s.AsSpan(0, w - first));
            return sb.ToString();
        }

        int idx = 0;
        long tick = 0;

        // Hide cursor for nicer effect
        bool oldVis = Console.CursorVisible;
        Console.CursorVisible = false;

        try
        {
            while (!ct.IsCancellationRequested && tick < totalTicks)
            {
                int w = dynamicWidth
                    ? Math.Max(10, Math.Min(Console.WindowWidth - 1, width))
                    : width;

                string frame = WrapSlice(src, idx, w);

                // Draw in-place, clear remainder of the line
                Console.SetCursorPosition(left, top);
                if (Ansi.IsEnabled)
                {
                    // Use CR + pad to fully overwrite prior frame
                    Console.Write('\r');
                    Console.Write(frame.PadRight(w));
                }
                else
                {
                    // Non-ANSI consoles: still pad to wipe artifacts
                    Console.Write(frame.PadRight(w));
                }

                // Advance circular index
                idx++;
                if (idx >= src.Length) idx = 0;

                tick++;

                try { await Task.Delay(ms, ct); }
                catch (TaskCanceledException) { break; }
            }
        }
        finally
        {
            // Clean up line once finished/canceled
            Console.SetCursorPosition(left, top);
            Console.Write(new string(' ', Math.Max(10, dynamicWidth ? Math.Min(Console.WindowWidth - 1, width) : width)));
            Console.SetCursorPosition(left, top);
            Console.CursorVisible = oldVis;
        }
    }


    public static void GradientText(string text, ConsoleColor start, ConsoleColor end)
    {
        // Simple two-color blend: first half 'start', second half 'end'
        int mid = Math.Max(1, text.Length / 2);
        for (int i = 0; i < text.Length; i++)
        {
            var col = i < mid ? start : end;
            ConsoleX.WithColor(col, null, () => Console.Write(text[i]));
        }
        Console.WriteLine();
    }
}
