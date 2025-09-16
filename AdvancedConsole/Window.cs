using System;
using System.Runtime.Versioning;

namespace AdvancedConsole;

/// <summary>
/// Window.
/// </summary>
public static class Window
{
    /// <summary>
    /// Set size safe.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void SetSizeSafe(int width, int height)
    {
        try { Console.SetWindowSize(Math.Min(width, Console.LargestWindowWidth), Math.Min(height, Console.LargestWindowHeight)); }
        catch { }
    }

    /// <summary>
    /// Move cursor.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="top"></param>
    public static void MoveCursor(int left, int top)
    {
        left = Math.Clamp(left, 0, Console.BufferWidth - 1);
        top = Math.Clamp(top, 0, Console.BufferHeight - 1);
        Console.SetCursorPosition(left, top);
    }

    /// <summary>
    /// Clear region.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="top"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void ClearRegion(int left, int top, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            Console.SetCursorPosition(left, top + y);
            Console.Write(new string(' ', Math.Max(0, Math.Min(width, Console.BufferWidth - left))));
        }
    }

    /// <summary>
    /// Beep success.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static void BeepSuccess() { TryBeep(880, 120); }
    /// <summary>
    /// Beep error.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static void BeepError() { TryBeep(220, 180); }

    /// <summary>
    /// Try beep.
    /// </summary>
    /// <param name="freq"></param>
    /// <param name="dur"></param>
    [SupportedOSPlatform("windows")]
    private static void TryBeep(int freq, int dur) { try { Console.Beep(freq, dur); } catch { } }
}
