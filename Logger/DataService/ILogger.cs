namespace Logger.Service
{
    public interface ILogger
    {
        /// <summary>
        /// Errors and Excptions.
        /// </summary>
        void Error(string fileName, string errorText);

        /// <summary>
        /// Issues that aren't critical yet.
        /// </summary>
        void Warning(string fileName, string warningText);

        /// <summary>
        /// General Informations about application process.
        /// </summary>
        void Info(string fileName, string infoText);

        /// <summary>
        /// Detailed logs for debuging.
        /// </summary>
        void Debug(string fileName, string debugText);

        /// <summary>
        /// Tracing program execution.
        /// </summary>
        void Trace(string fileName, string traceText);

        /// <summary>
        /// Server errors that cause the application to crash.
        /// </summary>
        void Fatal(string fileName, string fetalText);
    }
}
