using AdvancedConsole;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Demo;

internal static class Program
{
    private static async Task Main()
    {
        // Section & Figlet
        TextLayout.Section("AdvancedConsole Demo");
        Figlet.Write("ADV CONSOLE", ConsoleColor.Cyan);

        ColorWriter.WriteLineInfo("This is an info line");
        ColorWriter.WriteLineSuccess("This is a success line");
        ColorWriter.WriteLineWarning("This is a warning line");
        ColorWriter.WriteLineError("This is an error line");

        ColorWriter.WriteTaggedLine("Inline tags: [green]green[/], [red]red[/], [bg=blue][white]white on blue[/][/]");
        Styled.WithForeground(ConsoleColor.Yellow).Bold().WriteLine("Fluent: Bold yellow line");

        // Box
        Box.Write("Hello in a box!\nMultiple lines supported.", BoxStyle.Rounded, ConsoleColor.Magenta);

        // Prompts
        bool cont = Prompt.YesNo("Continue?");
        var name = Prompt.Input("Your name", defaultValue: "Ben");
        var secret = Prompt.Password("Enter password");
        var sel = Prompt.Select("Pick one", new[] { "Alpha", "Bravo", "Charlie" }, defaultIndex: 1);
        var mult = Prompt.MultiSelect("Pick some", new[] { "One", "Two", "Three", "Four" }, defaults: new[] { 0, 2 });

        ColorWriter.WriteTaggedLine($"Selected: [cyan]{sel}[/], Multi: [cyan]{string.Join(", ", mult)}[/]");

        // Progress + spinner + status
        using (var status = StatusLine.Start("Doing work"))
        {
            using var pb = new ProgressBar("Downloading…", width: 30);
            using var sp = new Spinner("Please wait…");
            for (int i = 0; i <= 100; i += 10)
            {
                pb.Report(i / 100.0, $"Progress {i}%");
                sp.Update($"Please wait… {i}%");
                await Task.Delay(120);
            }
            status.Complete("All done");
        }

        // Timer
        using (new ConsoleTimer("Measured section"))
        {
            await Task.Delay(300);
        }

        // Table
        new Table()
            .WithHeaders("Id", "Name", "Score")
            .AddRow("1", "Alice", "12")
            .AddRow("2", "Bob", "37")
            .AddRow("3", "Céline", "25")
            .WithBorderColor(ConsoleColor.DarkCyan)
            .Write();

        // Tree
        var root = new TreeNode("root");
        root.Add("bin").Add("debug");
        var src = root.Add("src");
        src.Add("ConsoleX");
        src.Add("Demo");
        TreeRenderer.Write(root);

        // Word wrap
        var text = "This is a long paragraph that will be wrapped to your console width with a small indent.";
        foreach (var line in TextLayout.WordWrap(text, Math.Max(40, Console.WindowWidth - 4), indent: 2))
            Console.WriteLine(line);

        // Window utils
        Window.BeepSuccess();
        Window.ClearRegion(0, Console.CursorTop, 10, 1); // tiny wipe

        // Animations
        await Animation.Marquee("Scrolling marquee demo text →", width: 30, ms: 80, loops: 1);
        Animation.GradientText("Gradient text demo", ConsoleColor.Cyan, ConsoleColor.Magenta);

        // Log capture
        LogCapture.Start("advancedconsole-demo.log");
        Console.WriteLine("This will also be written to advancedconsole-demo.log");
        LogCapture.Stop();

        // Simple shimmer, left→right
        await Animation.Shimmer(
            text: "ADVANCED CONSOLE",
            baseColor: ConsoleColor.DarkGray,
            highlightColor: ConsoleColor.White,
            window: 5,
            ms: 40,
            loops: 4);

        // Reverse direction, bold highlight off
        await Animation.Shimmer(
            text: "Shimmering…",
            baseColor: ConsoleColor.Gray,
            highlightColor: ConsoleColor.Cyan,
            window: 3,
            ms: 55,
            loops: 4,           // endless until du cancelst (oder brichst mit Ctrl+C)
            fromRight: true,
            boldHighlight: false);


        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey(); // per your preference
    }
}
