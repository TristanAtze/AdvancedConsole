using System;
using System.Collections.Generic;

namespace AdvancedConsole;

/// <summary>
/// Tree node.
/// </summary>
public sealed class TreeNode
{
    /// <summary>
    /// Text.
    /// </summary>
    public string Text { get; }
    /// <summary>
    /// Children.
    /// </summary>
    public List<TreeNode> Children { get; } = new();
    /// <summary>
    /// Initialize a new tree node.
    /// </summary>
    /// <param name="text"></param>
    public TreeNode(string text) => Text = text;
    /// <summary>
    /// Add a child node.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public TreeNode Add(string text) { var n = new TreeNode(text); Children.Add(n); return n; }
}

/// <summary>
/// Tree renderer.
/// </summary>
public static class TreeRenderer
{
    /// <summary>
    /// Write a tree.
    /// </summary>
    /// <param name="root"></param>
    public static void Write(TreeNode root)
    {
        Render(root, "", true);
    }

    /// <summary>
    /// Render a tree node.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="prefix"></param>
    /// <param name="last"></param>
    private static void Render(TreeNode node, string prefix, bool last)
    {
        var connector = prefix.Length == 0 ? "" : last ? "└─ " : "├─ ";
        Console.WriteLine(prefix + connector + node.Text);
        var nextPrefix = prefix + (prefix.Length == 0 ? "" : last ? "   " : "│  ");
        for (int i = 0; i < node.Children.Count; i++)
            Render(node.Children[i], nextPrefix, i == node.Children.Count - 1);
    }
}
