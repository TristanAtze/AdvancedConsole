using System;
using Microsoft.Extensions.Logging;

namespace AdvancedConsole;

/// <summary>
/// Simple console helpers bound to ILogger (needs Microsoft.Extensions.Logging.Abstractions).
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Log information message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    public static void LogInfo(this ILogger logger, string message)
    {
        ConsoleX.WithColor(ConsoleX.Theme.Info, null, () => logger.LogInformation(message));
    }
    /// <summary>
    /// Log success message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    public static void LogSuccess(this ILogger logger, string message)
    {
        ConsoleX.WithColor(ConsoleX.Theme.Success, null, () => logger.LogInformation(message));
    }
    /// <summary>
    /// Log warning message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    public static void LogWarning(this ILogger logger, string message)
    {
        ConsoleX.WithColor(ConsoleX.Theme.Warning, null, () => logger.LogWarning(message));
    }
    /// <summary>
    /// Log error message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    public static void LogError(this ILogger logger, string message)
    {
        ConsoleX.WithColor(ConsoleX.Theme.Error, null, () => logger.LogError(message));
    }
    /// <summary>
    /// Log success message in a box.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    public static void LogSuccessBox(this ILogger logger, string message)
    {
        Box.Write(message, BoxStyle.Rounded, ConsoleX.Theme.Success);
        logger.LogInformation(message);
    }
    /// <summary>
    /// Log information message in a box.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    public static void LogInfoBox(this ILogger logger, string message)
    {
        Box.Write(message, BoxStyle.Rounded, ConsoleX.Theme.Info);
        logger.LogInformation(message);
    }
    /// <summary>
    /// Log warning message in a box.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    public static void LogWarningBox(this ILogger logger, string message)
    {
        Box.Write(message, BoxStyle.Rounded, ConsoleX.Theme.Warning);
        logger.LogWarning(message);
    }

    /// <summary>
    /// Log error message in a box.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="ex"></param>
    public static void LogErrorBox(this ILogger logger, string message, Exception? ex = null)
    {
        Box.Write(message, BoxStyle.Double, ConsoleX.Theme.Error);
        if (ex is null) logger.LogError(message);
        else logger.LogError(ex, message);
    }
}
