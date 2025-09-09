using System;
using System.Collections.Generic;

namespace AdvancedConsole;

public sealed class TreeNode
{
    public string Text { get; }
    public List<TreeNode> Children { get; } = new();
    public TreeNode(string text) => Text = text;
    public TreeNode Add(string text) { var n = new TreeNode(text); Children.Add(n); return n; }
}

public static class TreeRenderer
{
    public static void Write(TreeNode root)
    {
        Render(root, "", true);
    }

    private static void Render(TreeNode node, string prefix, bool last)
    {
        var connector = prefix.Length == 0 ? "" : last ? "└─ " : "├─ ";
        Console.WriteLine(prefix + connector + node.Text);
        var nextPrefix = prefix + (prefix.Length == 0 ? "" : last ? "   " : "│  ");
        for (int i = 0; i < node.Children.Count; i++)
            Render(node.Children[i], nextPrefix, i == node.Children.Count - 1);
    }
}
