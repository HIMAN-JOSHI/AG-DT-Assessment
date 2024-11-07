using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;


namespace Common.Helpers
{
    // Helper class for managing test reporting using ExtentReports
    public static class ReportingHelper
    {
        // Private static field to hold the instance of ExtentReports
        private static ExtentReports? _extent;

        // Private static field to hold the instance of ExtentTest
        private static ExtentTest? _test;

        /// <summary>
        /// Initializes the Extent Report with the specified report type.
        /// </summary>
        /// <param name="reportType">The type of report (e.g., "API", "UI") to distinguish reports.</param>
        public static void InitializeReport(String reportType) {

            // Define the directory where the report files will be saved
            var reportDirectory = Path.Combine("C:\\AG-DATA\\Reports", reportType);
            Directory.CreateDirectory(reportDirectory);

            // Define the full path for the HTML report file with a filename based on the report type
            var reportPath = Path.Combine(reportDirectory, $"{reportType}_ExtentReport.html");

            // Create an ExtentHtmlReporter for generating HTML reports at the specified path
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            htmlReporter.Config.DocumentTitle = $"{reportType} Automation Test Report";
            htmlReporter.Config.ReportName = $"{reportType} Test Execution Report";

            // Initialize the ExtentReports instance and attach the HTML reporter for report generation
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
        }

        /// <summary>
        /// Creates a test with the specified name for logging test information.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <returns>An object of ExtentTest</returns>
        public static ExtentTest CreateTest(String testName) { 
        
            _test = _extent?.CreateTest(testName);
            return _test;
        }

        /// <summary>
        /// Logs an informational message to the current test.
        /// </summary>
        /// <param name="message"></param>
        public static void LogInfo(String message) => _test?.Info(message);

        /// <summary>
        /// Logs a pass message to the current test.
        /// </summary>
        /// <param name="message"></param>
        public static void LogPass(String message) => _test?.Pass(message);

        /// <summary>
        /// Logs a fail message to the current test.
        /// </summary>
        /// <param name="message"></param>
        public static void LogFail(String message) => _test?.Fail(message);

        /// <summary>
        /// Flushes and saves the report.
        /// </summary>
        public static void FlushReport() => _extent?.Flush();
    }
}
