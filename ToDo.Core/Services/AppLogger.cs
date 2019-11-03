using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ToDo.Core.Services
{
    /// <summary>
    /// Logger
    /// </summary>
    public class AppLogger : IAppLogger
    {
        #region methods

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = string.Format("{0}: {1} - {2}", logLevel.ToString(), eventId.Id, formatter(state, exception));
            WriteLog(message);
        }

        public Task<bool> LogBindingStarted(string name)
        {
            return Log(LogLevel.Information, $"Binding started for {name}");
        }

        public Task<bool> LogError(string message)
        {
            return Log(LogLevel.Error, message);
        }

        public Task<bool> LogInfo(string message)
        {
            return Log(LogLevel.Information, message);
        }

        public Task<bool> LogWarning(string message)
        {
            return Log(LogLevel.Warning, message);
        }

        #endregion methods

        #region private_methods

        private Task<bool> Log(LogLevel level, string message)
        {
            WriteLog($"{level} - {message}");
            return Task.FromResult(true);
        }

        private void WriteLog(string logMessage)
        {
            Console.WriteLine(logMessage);
        }

        #endregion private_methods
    }
}