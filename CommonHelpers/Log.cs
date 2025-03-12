using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace CommonHelpers
{
    public static class Log
    {
        public static bool LogToFile = true;
        public static bool LogToFileDebug = false;
        
        private readonly static ILog _logger = LogManager.GetLogger(Instance.ApplicationName);

        private static void Setup()
        {
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var logsFolder = Path.Combine(documentsFolder, "SteamDeckTools", "Logs");

            BasicConfigurator.Configure();
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();

            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };
            patternLayout.ActivateOptions();

            if (LogToFile)
            {
                var roller = new RollingFileAppender
                {
                    AppendToFile = true,
                    File = logsFolder,
                    DatePattern = "dd.MM.yyyy_'"+Instance.ApplicationName+".log'",
                    Layout = patternLayout,
                    MaxSizeRollBackups = 5,
                    MaximumFileSize = "5MB",
                    RollingStyle = RollingFileAppender.RollingMode.Composite,
                    StaticLogFileName = false
                };
                roller.ActivateOptions();
                hierarchy.Root.AddAppender(roller);
            }
            

            hierarchy.Root.Level = Level.Critical;
            hierarchy.Configured = true;
            BasicConfigurator.Configure(hierarchy);
        }

        public static void Trace(string name, object subject)
        {
            Setup();
            _logger.Logger.Log(_logger.GetType(), Level.Trace, subject, null);
        }

        public static void Info(string format, params object?[] arg)
        {
            Setup();
            String line = String.Format(format, arg);
            _logger.Info(line);
        }

        public static void Debug(string format, params object?[] arg)
        {
            Setup();
            String line = String.Format(format, arg);
            _logger.Debug(line);
        }

        public static void Error(string format, params object?[] arg)
        {
            Setup();
            String line = String.Format(format, arg);
            _logger.Error(line);
        }

        public static void Fatal(string type, Object? name, Exception e)
        {
            Setup();
            var message = $"{type}: {name}: Exception: {e.Message}";
            _logger.Fatal(message, e);
        }

        public static void Fatal(string type, Exception e)
        {
            Setup();
            var message = $"{type}: Exception: {e.Message}";
            _logger.Fatal(message, e);
        }
    }
}
