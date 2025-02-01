using System;
using System.IO;
using System.Reflection;

namespace Logger.Service
{
    public class Logger : ILogger
    {
        private readonly string _baseLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly object _lock = new object();

        public Logger() { }

        public void Error(string fileName, string errorText) => WriteLog(LoggerTypes.Errors, fileName, errorText);
        public void Warning(string fileName, string warningText) => WriteLog(LoggerTypes.Warnings, fileName, warningText);
        public void Info(string fileName, string infoText) => WriteLog(LoggerTypes.Info, fileName, infoText);
        public void Debug(string fileName, string debugText) => WriteLog(LoggerTypes.Debugs, fileName, debugText);
        public void Trace(string fileName, string traceText) => WriteLog(LoggerTypes.Traces, fileName, traceText);
        public void Fatal(string fileName, string fatalText) => WriteLog(LoggerTypes.Fatals, fileName, fatalText);

        #region Private Methods

        private void WriteLog(string logType, string fileName, string logText)
        {
            string logTypeFolderPath = Path.Combine(_baseLogPath, logType);
            string todayFolderPath = Path.Combine(logTypeFolderPath, DateTime.Now.ToString("yyyy-MM-dd"));

            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            string formattedVersion = $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}.{assemblyVersion.Revision:D4}";


            Files_Helper.CreateDirectory(todayFolderPath);

            fileName = $"{fileName}_({formattedVersion})_{DateTime.Now:yyyy-MM-dd-hh-tt}_log.txt";
            string logFilePath = Path.Combine(todayFolderPath, fileName);

            lock (_lock)
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    writer.WriteLine($"{DateTime.Now:hh:mm:ss tt} ===>");
                    writer.WriteLine(logText);
                    writer.WriteLine(AddSeparator());
                }
            }
        }

        private string AddSeparator() => $"===================================================================================" +
            $"==========================================={Environment.NewLine}{Environment.NewLine}";

        #endregion
    }
}
