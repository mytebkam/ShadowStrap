using System;
using System.IO;

namespace ShadowStrap.Integrations.Logging
{
    public static class DebugLogger
    {
        private static readonly string LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ShadowStrap", "debug_log.txt");

        public static void Log(string message, string level = "INFO")
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            try
            {
                File.AppendAllLines(LogPath, new[] { logEntry });
            }
            catch
            {
                // Fail silently in production, but recorded during debug
            }
        }
    }
}
