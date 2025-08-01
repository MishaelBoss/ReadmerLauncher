﻿using System.IO;

namespace Launcher.View.Resources.Script
{
    public static partial class Loges
    {
        private static readonly object _lockObj = new object();
        private static string _currentLogDate;
        private static string _currentLogPath;

        public static void ExceptionEventApp(object sender, UnhandledExceptionEventArgs ueea)
        {
            EnsureLogDirectory();

            if (ueea.ExceptionObject is Exception e)
            {
                LoggingProcess(LogLevel.ERROR, e.ToString());
            }
        }

        public static void LoggingProcess(LogLevel level, string message = null, Exception ex = null)
        {
            lock (_lockObj)
            {
                EnsureLogDirectory();
                string todayDate = DateTime.Today.ToString("yyyy-MM-dd");

                if (ShouldCreateNewLog(todayDate))
                {
                    CreateNewLogFile(todayDate);
                }

                WriteLogEntry(level, message, ex);
            }
        }

        private static void EnsureLogDirectory()
        {
            if (!Directory.Exists(Paths.saved)) Directory.CreateDirectory(Paths.saved);
            if (!Directory.Exists(Paths.log)) Directory.CreateDirectory(Paths.log);
            if (!Directory.Exists(Paths.crashes)) Directory.CreateDirectory(Paths.crashes);
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

        private static void WriteLogEntry(LogLevel level, string message = null, Exception ex = null)
        {
            string entry = $"[{DateTime.Now:HH:mm:ss}]-[{level}:] {message} {ex.Message} - index page error - {GetPageInfo(ex)} {Environment.NewLine}";
            File.AppendAllText(_currentLogPath, entry);
        }

        private static string GetPageInfo(Exception ex)
        {
            if (ex != null && ex.StackTrace != null)
            {
                return GetPageFromStackTrace(ex.StackTrace);
            }
            return "Информация о странице не доступна";
        }

        private static string GetPageFromStackTrace(string stackTrace)
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
