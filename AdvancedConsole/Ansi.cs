using System;
using System.Runtime.InteropServices;

namespace AdvancedConsole;

/// <summary>
/// ANSI helpers and Windows VT enabling.
/// </summary>
internal static class Ansi
{
    public const string Reset = "\u001b[0m";
    public const string Bold = "\u001b[1m";
    public const string Underline = "\u001b[4m";

    public static bool IsEnabled { get; private set; } = TryEnable();

    private static bool TryEnable()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                // Enable VT processing on Windows 10+
                IntPtr handle = GetStdHandle(-11); // STD_OUTPUT_HANDLE
                if (handle == IntPtr.Zero || handle == new IntPtr(-1)) return false;
                if (!GetConsoleMode(handle, out int mode)) return false;
                const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
                if ((mode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) != ENABLE_VIRTUAL_TERMINAL_PROCESSING)
                {
                    mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
                    if (!SetConsoleMode(handle, mode)) return false;
                }
            }
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            return true;
        }
        catch { return false; }
    }

    public static string Fg(ConsoleColor c) => c switch
    {
        ConsoleColor.Black => "\u001b[30m",
        ConsoleColor.DarkRed => "\u001b[31m",
        ConsoleColor.DarkGreen => "\u001b[32m",
        ConsoleColor.DarkYellow => "\u001b[33m",
        ConsoleColor.DarkBlue => "\u001b[34m",
        ConsoleColor.DarkMagenta => "\u001b[35m",
        ConsoleColor.DarkCyan => "\u001b[36m",
        ConsoleColor.Gray => "\u001b[37m",
        ConsoleColor.DarkGray => "\u001b[90m",
        ConsoleColor.Red => "\u001b[91m",
        ConsoleColor.Green => "\u001b[92m",
        ConsoleColor.Yellow => "\u001b[93m",
        ConsoleColor.Blue => "\u001b[94m",
        ConsoleColor.Magenta => "\u001b[95m",
        ConsoleColor.Cyan => "\u001b[96m",
        ConsoleColor.White => "\u001b[97m",
        _ => "\u001b[39m"
    };

    public static string Bg(ConsoleColor c) => c switch
    {
        ConsoleColor.Black => "\u001b[40m",
        ConsoleColor.DarkRed => "\u001b[41m",
        ConsoleColor.DarkGreen => "\u001b[42m",
        ConsoleColor.DarkYellow => "\u001b[43m",
        ConsoleColor.DarkBlue => "\u001b[44m",
        ConsoleColor.DarkMagenta => "\u001b[45m",
        ConsoleColor.DarkCyan => "\u001b[46m",
        ConsoleColor.Gray => "\u001b[47m",
        ConsoleColor.DarkGray => "\u001b[100m",
        ConsoleColor.Red => "\u001b[101m",
        ConsoleColor.Green => "\u001b[102m",
        ConsoleColor.Yellow => "\u001b[103m",
        ConsoleColor.Blue => "\u001b[104m",
        ConsoleColor.Magenta => "\u001b[105m",
        ConsoleColor.Cyan => "\u001b[106m",
        ConsoleColor.White => "\u001b[107m",
        _ => "\u001b[49m"
    };

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);
}
