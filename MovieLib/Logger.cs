using System;
using System.Diagnostics;
using System.IO;

namespace MovieLib
{
    public enum LOGLEVEL
    {
        INFO = 0,
        DEBUG = 1,
        WARN = 3,
        ERROR = 4,
        FATAL = 5
    }

    public delegate void LogEventHandler(LOGLEVEL logLevel, string tag, string message);

    public class Logger
    {
        public static event LogEventHandler LogEvent;
        private const string LOG_FILE = @"MovieDetails.log";

        public static void LogConnectionException(Exception e)
        {
            if (e != null)
                LogEvent(LOGLEVEL.ERROR, "HTTP", e.Message);
        }

        public static void Info(string tag, string message)
        {
            Add(LOGLEVEL.INFO, tag, message);
        }

        public static void Debug(string tag, string message)
        {
            Add(LOGLEVEL.DEBUG, tag, message);
        }

        public static void Warn(string tag, string message)
        {
            Add(LOGLEVEL.WARN, tag, message);
        }

        public static void Error(string tag, string message)
        {
            Add(LOGLEVEL.ERROR, tag, message);
        }

        public static void Fatal(string tag, string message)
        {
            Add(LOGLEVEL.FATAL, tag, message);
        }

        private static void Add(LOGLEVEL logLevel, string tag, string message)
        {
            if (!(LogEvent is null)) // Don't call if we haven't registered any events (in MovieLibTests)
                LogEvent(logLevel, tag, message);
        }

        [Conditional("DEBUG")]
        public static void RegisterConsoleOutput()
        {
            LogEvent += Logger_LogEventConsole;
        }

        private static void Logger_LogEventConsole(LOGLEVEL logLevel, string tag, string message)
        {
            switch (logLevel)   // Set console color based on loglevel
            {
                case LOGLEVEL.ERROR:
                case LOGLEVEL.FATAL:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
            }

            Console.WriteLine(GetLogFormattedString(logLevel, tag, message));
            Console.ForegroundColor = ConsoleColor.White;   // restore console color
        }

        public static void RegisterLogFileOutput()
        {
            LogEvent += Logger_LogEventLogFile;
        }

        private static void Logger_LogEventLogFile(LOGLEVEL logLevel, string tag, string message)
        {
            using (StreamWriter writer = new StreamWriter(LOG_FILE, true))
            {
                writer.WriteLine(GetLogFormattedString(logLevel, tag, message));
            }
        }

        /// <summary>
        /// Returns a formatted Log string.
        /// Example: [27/05:45][WARN][HTTP] Connection Refused
        /// </summary>
        private static string GetLogFormattedString(LOGLEVEL logLevel, string tag, string message)
        {
            string timeString = DateTime.Now.ToString("dd/HH:mm");
            return $"[{timeString}][{logLevel}][{tag}] {message}";
        }
    }
}
