using LauncherLes1.View.Resources.Script;
using System.IO;

namespace Launcher.View.Resources.Script
{
    public static partial class Loges
    {
        private static readonly object _lockObj = new object();
        private static string _currentLogDate;
        private static string _currentLogPath;

        public static void ExceptionEventApp(object sender, UnhandledExceptionEventArgs ueea)
        {
            if (ueea.ExceptionObject is Exception e)
            {
                LoggingProcess(LogLevel.ERROR, e.ToString());
            }
        }

        public static void LoggingProcess(LogLevel level, string message, string page = null, string targetsite = null, string stacktrace = null)
        {
            lock (_lockObj)
            {
                EnsureLogDirectory();
                string todayDate = DateTime.Today.ToString("yyyy-MM-dd");

                if (ShouldCreateNewLog(todayDate))
                {
                    CreateNewLogFile(todayDate);
                }

                WriteLogEntry(level, message, page, targetsite, stacktrace);
            }
        }

        private static void EnsureLogDirectory()
        {
            if (!Directory.Exists(Paths.log))
            {
                Directory.CreateDirectory(Paths.log);
            }
        }

        private static bool ShouldCreateNewLog(string todayDate)
        {
            return _currentLogDate != todayDate ||
                   string.IsNullOrEmpty(_currentLogPath) ||
                   !File.Exists(_currentLogPath);
        }

        private static void CreateNewLogFile(string date)
        {
            _currentLogPath = Path.Combine(Paths.log, $"log-{date}.txt");
            _currentLogDate = date;

            if (!File.Exists(_currentLogPath))
            {
                File.WriteAllText(_currentLogPath,
                    $"=== Log started {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n\n" +
                    "INFO: INFORMATION\n" +
                    "WARN: WARNING\n" +
                    "ERROR: ERROR\n\n");
            }
        }

        private static void WriteLogEntry(LogLevel level, string message, string page = null, string targetsite = null, string stacktrace = null)
        {
            string entry = $"[{DateTime.Now:HH:mm:ss}]-[{level}:] {message} - index page error - {page} {targetsite} {stacktrace} {Environment.NewLine}";
            File.AppendAllText(_currentLogPath, entry);
        }

        public static string GetPageInfo(Exception ex)
        {
            if (ex != null && ex.StackTrace != null)
            {
                return GetPageFromStackTrace(ex.StackTrace);
            }
            return "Информация о странице не доступна";
        }

        public static string GetPageFromStackTrace(string stackTrace)
        {
            return stackTrace;
        }
    }

    public enum LogLevel
    {
        INFO = 0,
        WARN = 1,
        ERROR = 2
    }
}
