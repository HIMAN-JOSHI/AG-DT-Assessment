using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;


namespace Common.Helpers
{
    /// <summary>
    /// Helper class for configuring and managing application logging using Serilog.
    /// Provides methods to initialize logging, retrieve logger instances, and log messages.
    /// </summary>
    public class LogsHelper
    {
        // Stores the service provider for dependency injection of logging services
        private static IServiceProvider? _serviceProvider;

        // Factory for creating logger instances
        private static ILoggerFactory? _loggerFactory;      

        /// <summary>
        /// Initializes the logger configuration with the specified log file path.
        /// </summary>
        /// <param name="logFilePath">The path where log files will be created.</param>
        public static void InitializeLogger(String logFilePath)
        {
            // Configure Serilog to write log entries to a file with daily rolling intervals
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Create an ILoggerFactory with Serilog as the logging provider
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                // Clear default logging providers
                builder.ClearProviders();

                // Add Serilog as the logging provider
                builder.AddSerilog();      
            });

            // Set up a service provider for dependency injection of the logger factory
            _serviceProvider = new ServiceCollection()
                .AddSingleton(_loggerFactory)
                .AddLogging()
                .BuildServiceProvider();
        }

        /// <summary>
        /// Gets a logger instance of the specified type
        /// </summary>
        /// <typeparam name="T">The class type for which the logger is created.</typeparam>
        /// <returns>An ILogger instance for the specified type.</returns>
        public static ILogger<T> GetLogger<T>()
        {
            // Ensure the logger has been initialized; else throw an exception
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Logger has not been initialized. Call LogsHelper.InitializeLogger() first.");
            }
            return _loggerFactory.CreateLogger<T>();
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log as information.</param>
        public static void LogInfo(String message) => Log.Information(message);

        /// <summary>
        /// Logs an error message, with optional exception details.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="exception">The exception to log (optional).</param>
        public static void LogError(String message, Exception? exception = null)
        {
            // Log error with or without exception details
            if (exception == null)
            {
                Log.Error(message);
            }
            else
            {
                Log.Error(exception, message);
            }
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public static void LogWarning(String message) => Log.Warning(message);

        /// <summary>
        /// Disposes of logging resources
        /// </summary>
        public static void Dispose()
        {
            // Dispose of the logger factory
            _loggerFactory?.Dispose();

            // Close and flush the Serilog logger
            Log.CloseAndFlush();        
        }
    }

}
