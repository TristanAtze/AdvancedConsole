using System;
using System.IO;
using System.Text;

namespace AdvancedConsole;

/// <summary>
/// Captures all Console.Write* output to a file while still writing to the original stream.
/// </summary>
public static class LogCapture
{
    private sealed class TeeWriter : TextWriter
    {
        private readonly TextWriter _a, _b;
        public TeeWriter(TextWriter a, TextWriter b) { _a = a; _b = b; }
        public override Encoding Encoding => _a.Encoding;
        public override void Write(char value) { _a.Write(value); _b.Write(value); }
        public override void Write(string? value) { _a.Write(value); _b.Write(value); }
        public override void WriteLine(string? value) { _a.WriteLine(value); _b.WriteLine(value); }
        protected override void Dispose(bool disposing) { base.Dispose(disposing); _b.Dispose(); }
        public override void Flush() { _a.Flush(); _b.Flush(); }
    }

    private static TextWriter? _orig;
    private static StreamWriter? _file;

    public static void Start(string filePath, bool append = true)
    {
        if (_orig != null) return;
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath)) ?? ".");
        _orig = Console.Out;
        _file = new StreamWriter(new FileStream(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read))
        { AutoFlush = true };
        Console.SetOut(new TeeWriter(_orig, _file));
    }

    public static void Stop()
    {
        if (_orig == null) return;
        Console.Out.Flush();
        Console.SetOut(_orig);
        _file?.Dispose();
        _file = null;
        _orig = null;
    }
}
