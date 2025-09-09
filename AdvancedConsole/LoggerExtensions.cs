using System;
using Microsoft.Extensions.Logging;

namespace AdvancedConsole;

/// <summary>
/// Simple console helpers bound to ILogger (needs Microsoft.Extensions.Logging.Abstractions).
/// </summary>
public static class LoggerExtensions
{
    public static void LogSuccess(this ILogger logger, string message)
    {
        ConsoleX.WithColor(ConsoleX.Theme.Success, null, () => logger.LogInformation(message));
    }

    public static void LogWarningBox(this ILogger logger, string message)
    {
        Box.Write(message, BoxStyle.Rounded, ConsoleX.Theme.Warning);
        logger.LogWarning(message);
    }

    public static void LogErrorBox(this ILogger logger, string message, Exception? ex = null)
    {
        Box.Write(message, BoxStyle.Double, ConsoleX.Theme.Error);
        if (ex is null) logger.LogError(message);
        else logger.LogError(ex, message);
    }
}
