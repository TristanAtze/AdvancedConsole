using System;
using System.Linq;

namespace AdvancedConsole;

/// <summary>
/// Box style.
/// </summary>  
public enum BoxStyle { 
    /// <summary>
    /// Single.
    /// </summary>
    Single, 
    /// <summary>
    /// Double.
    /// </summary>
    Double, 
    /// <summary>
    /// Rounded.
    /// </summary>
    Rounded 
}

/// <summary>
/// Box.
/// </summary>
public static class Box
{
    /// <summary>
    /// Write a box.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="style"></param>
    /// <param name="color"></param>
    public static void Write(string text, BoxStyle style = BoxStyle.Single, ConsoleColor? color = null)
    {
        var lines = (text ?? string.Empty).Replace("\r", "").Split('\n');
        int width = Math.Max(1, lines.Max(l => l.Length));
        var c = Chars(style);

        ConsoleX.WithColor(color, null, () =>
        {
            Console.WriteLine($"{c.TopLeft}{new string(c.Horizontal, width + 2)}{c.TopRight}");
            foreach (var l in lines)
                Console.WriteLine($"{c.Vertical} {l.PadRight(width)} {c.Vertical}");
            Console.WriteLine($"{c.BottomLeft}{new string(c.Horizontal, width + 2)}{c.BottomRight}");
        });
    }

    private static (char TopLeft, char TopRight, char BottomLeft, char BottomRight, char Horizontal, char Vertical) Chars(BoxStyle s) => s switch
    {
        BoxStyle.Double => ('╔', '╗', '╚', '╝', '═', '║'),
        BoxStyle.Rounded => ('╭', '╮', '╰', '╯', '─', '│'),
        _ => ('┌', '┐', '└', '┘', '─', '│'),
    };
}
