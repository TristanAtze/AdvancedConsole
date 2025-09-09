using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedConsole;

public static partial class Animation
{
    /// <summary>
    /// Shimmering text effect: a moving highlight passes across the text.
    /// </summary>
    /// <param name="text">Text to render on a single console line.</param>
    /// <param name="baseColor">Base color used for non-highlighted characters.</param>
    /// <param name="highlightColor">Color used for the highlight window.</param>
    /// <param name="window">Width of the highlight window (number of characters).</param>
    /// <param name="ms">Delay between frames in milliseconds.</param>
    /// <param name="loops">-1 for endless; otherwise how many full passes.</param>
    /// <param name="fromRight">If true, highlight travels right→left; otherwise left→right.</param>
    /// <param name="boldHighlight">If true and ANSI is enabled, highlight is also bold.</param>
    /// <param name="ct">Cancellation token.</param>
    public static async Task Shimmer(
        string text,
        ConsoleColor baseColor,
        ConsoleColor highlightColor,
        int window = 4,
        int ms = 60,
        int loops = -1,
        bool fromRight = false,
        bool boldHighlight = true,
        CancellationToken ct = default)
    {
        text ??= string.Empty;
        window = Math.Max(1, window);

        // Where to draw: lock to current line, column 0 for stable rendering
        int top = Console.CursorTop;
        int left = 0;

        // One full pass length: text length + window (lets highlight fully enter/exit)
        int passLen = Math.Max(1, text.Length + window);
        long totalTicks = loops < 0 ? long.MaxValue : (long)loops * passLen;

        bool oldVis = Console.CursorVisible;
        Console.CursorVisible = false;

        try
        {
            for (long tick = 0; tick < totalTicks && !ct.IsCancellationRequested; tick++)
            {
                int head = (int)(tick % passLen);                  // highlight head (0..passLen-1)
                if (fromRight) head = passLen - 1 - head;          // reverse direction

                Console.SetCursorPosition(left, top);

                // Render frame char-by-char to apply highlight window
                for (int i = 0; i < text.Length; i++)
                {
                    // Distance from highlight head; highlight spans [head-window+1 ... head]
                    bool isHi = i >= head - (window - 1) && i <= head;

                    if (Ansi.IsEnabled)
                    {
                        Console.Write(isHi ? Ansi.Fg(highlightColor) : Ansi.Fg(baseColor));
                        if (isHi && boldHighlight) Console.Write(Ansi.Bold);
                        Console.Write(text[i]);
                        if (isHi && boldHighlight) Console.Write(Ansi.Reset + Ansi.Fg(isHi ? highlightColor : baseColor));
                    }
                    else
                    {
                        var old = Console.ForegroundColor;
                        Console.ForegroundColor = isHi ? highlightColor : baseColor;
                        Console.Write(text[i]);
                        Console.ForegroundColor = old;
                    }
                }

                // Clear to end-of-line to avoid artifacts if previous frame was longer
                int width = Math.Max(1, Console.WindowWidth - 1);
                if (text.Length < width) Console.Write(new string(' ', width - text.Length));

                // Reset any ANSI styles at frame end
                if (Ansi.IsEnabled) Console.Write(Ansi.Reset);

                try { await Task.Delay(ms, ct); }
                catch (TaskCanceledException) { break; }
            }
        }
        finally
        {
            // Clean up: leave the line in base color text (no animation), or blank if you prefer
            Console.SetCursorPosition(left, top);
            if (text.Length > 0)
            {
                if (Ansi.IsEnabled) Console.Write(Ansi.Fg(baseColor));
                else Console.ForegroundColor = baseColor;

                Console.Write(text);

                int width = Math.Max(1, Console.WindowWidth - 1);
                if (text.Length < width) Console.Write(new string(' ', width - text.Length));

                if (Ansi.IsEnabled) Console.Write(Ansi.Reset);
            }
            else
            {
                int width = Math.Max(1, Console.WindowWidth - 1);
                Console.Write(new string(' ', width));
            }
            Console.SetCursorPosition(left, top);
            Console.CursorVisible = oldVis;
        }
    }
}
