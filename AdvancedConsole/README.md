# 🎨 AdvancedConsole

[![NuGet](https://img.shields.io/nuget/v/AdvancedConsole.svg)](https://www.nuget.org/packages/AdvancedConsole)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

**AdvancedConsole** ist ein modernes .NET 8 Utility-Paket für **stylische Konsolenanwendungen**.  
Es kombiniert Farben, UI-Elemente, Animationen und Logging in einer einzigen, leichten Bibliothek.

---

## ✨ Features

- 🎨 **Colors & Styling**
  - ANSI + Windows VT support
  - Inline color tags (`[red]error[/]`, `[bg=blue][white]text[/]`)
  - Fluent API (`Styled.WithForeground(...).Bold().WriteLine(...)`)

- 📦 **UI Components**
  - Boxes (`BoxStyle.Single`, `Double`, `Rounded`)
  - Figlet mini-font banners
  - Tables & Trees
  - Sections, Word-wrap, Indentation

- 📝 **Prompts**
  - Yes/No questions
  - Input with default & validation
  - Masked password input
  - Single-Select & Multi-Select menus

- 📊 **Progress & Status**
  - ProgressBar
  - Spinner
  - StatusLine
  - Timers (measure execution time)

- 🌀 **Animations**
  - Marquee scrolling text
  - Gradient text
  - **Shimmering text (new!)**

- 🔔 **Window & Sound**
  - Resize helpers
  - Region clear
  - BeepSuccess / BeepError

- 📜 **Logging**
  - Log capture to file
  - ILogger extensions (`LogSuccess`, `LogWarningBox`, `LogErrorBox`)

---

## 📦 Installation

```powershell
dotnet add package AdvancedConsole
````

Target framework: **.NET 8.0**
Keine Abhängigkeiten außer `Microsoft.Extensions.Logging.Abstractions`.

---

## 🚀 Quickstart

```csharp
using AdvancedConsole;

// Section & Figlet
TextLayout.Section("Demo");
Figlet.Write("ADV CONSOLE", ConsoleColor.Cyan);

// Info lines
ColorWriter.WriteLineInfo("This is info");
ColorWriter.WriteLineSuccess("This is success");
ColorWriter.WriteLineWarning("This is warning");
ColorWriter.WriteLineError("This is error");

// Inline tags
ColorWriter.WriteTaggedLine("Inline [green]green[/], [red]red[/], [bg=blue][white]white on blue[/][/]");

// Prompts
bool go = Prompt.YesNo("Continue?");
string name = Prompt.Input("Your name", defaultValue: "Ben");
string pw = Prompt.Password("Enter password");
var choice = Prompt.Select("Pick one", new[] { "Alpha", "Bravo", "Charlie" });

// Progress & spinner
using (var pb = new ProgressBar("Working...", 30))
{
    for (int i = 0; i <= 100; i += 10)
    {
        pb.Report(i / 100.0, $"Progress {i}%");
        await Task.Delay(120);
    }
}
```

---

## 🌀 Animations

```csharp
// Marquee scrolling
await Animation.Marquee("Scrolling text →", width: 30, ms: 80, loops: 1);

// Gradient text
Animation.GradientText("Hello Gradient", ConsoleColor.Cyan, ConsoleColor.Magenta);

// Shimmering text
await Animation.Shimmer(
    text: "ADVANCED CONSOLE",
    baseColor: ConsoleColor.DarkGray,
    highlightColor: ConsoleColor.White,
    window: 5,
    ms: 40,
    loops: 2);
```

---

## 📊 Tables & Trees

```csharp
new Table()
    .WithHeaders("Id", "Name", "Score")
    .AddRow("1", "Alice", "12")
    .AddRow("2", "Bob", "37")
    .WithBorderColor(ConsoleColor.DarkCyan)
    .Write();

var root = new TreeNode("root");
root.Add("bin").Add("debug");
var src = root.Add("src");
src.Add("ConsoleX");
TreeRenderer.Write(root);
```

---

## 📜 Logging

```csharp
using Microsoft.Extensions.Logging;

ILogger logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger("demo");

logger.LogSuccess("Operation succeeded");
logger.LogWarningBox("Something looks odd...");
logger.LogErrorBox("Something failed", new Exception("Boom!"));

// Capture console to file
LogCapture.Start("output.log");
Console.WriteLine("This line is also written to output.log");
LogCapture.Stop();
```

---

## 🛠️ Roadmap

* 🎨 TrueColor (24-bit RGB) ANSI support
* 📊 Multi-progress bar
* 🔀 Async prompt API
* 🎵 Cross-platform sound effects

---

## 📄 License

MIT © Ben Sowieja
Feel free to use in commercial and open-source projects.

