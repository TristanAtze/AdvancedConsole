using System;

namespace AdvancedConsole;

public static class Window
{
    public static void SetSizeSafe(int width, int height)
    {
        try { Console.SetWindowSize(Math.Min(width, Console.LargestWindowWidth), Math.Min(height, Console.LargestWindowHeight)); }
        catch { }
    }

    public static void MoveCursor(int left, int top)
    {
        left = Math.Clamp(left, 0, Console.BufferWidth - 1);
        top = Math.Clamp(top, 0, Console.BufferHeight - 1);
        Console.SetCursorPosition(left, top);
    }

    public static void ClearRegion(int left, int top, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            Console.SetCursorPosition(left, top + y);
            Console.Write(new string(' ', Math.Max(0, Math.Min(width, Console.BufferWidth - left))));
        }
    }

    public static void BeepSuccess() { TryBeep(880, 120); }
    public static void BeepError() { TryBeep(220, 180); }

    private static void TryBeep(int freq, int dur) { try { Console.Beep(freq, dur); } catch { } }
}
