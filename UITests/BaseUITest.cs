using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Common.Helpers;
using UITests.Utils;


namespace UITests
{
    // Base class for UI tests that sets up and tears down the WebDriver, logger, and report
    [TestFixture]
    public abstract class BaseUITest
    {
        // Protected properties to allow derived classes to access the WebDriver and WebDriverWait
        protected IWebDriver Driver { get; private set; }
        protected WebDriverWait Wait { get; private set; }

        // Runs once at the start of the test suite to set up logging and reporting
        [OneTimeSetUp]
        public void OneTimeSetUp() {
            ReportingHelper.InitializeReport(Constants.REPORT_TYPE_UI);
            LogsHelper.InitializeLogger("C:\\AG-DATA\\Logs\\ui-test-logs.txt");
            LogsHelper.LogInfo("Report and Logger initialized.");
        }

        // Runs before each test case to set up the WebDriver and start a new report test entry
        [SetUp]
        public void SetUp() {

            // Create a new report entry for the current test
            ReportingHelper.CreateTest(TestContext.CurrentContext.Test.Name);
            LogsHelper.LogInfo($"Starting test: {TestContext.CurrentContext.Test.Name}");

            // Initialize the WebDriver
            Driver = new ChromeDriver();

            // Set up WebDriverWait with a default timeout
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            // Maximize the browser window
            Driver.Manage().Window.Maximize();

            LogsHelper.LogInfo("Browser initialized and maximized.");

        }

        // Runs after each test case to capture test results, screenshots on failure, and cleanup
        [TearDown]
        public void TearDown()
        {
            try
            {
                // Get the outcome of the test case
                var outcome = TestContext.CurrentContext.Result.Outcome.Status;
                LogsHelper.LogInfo($"Test {TestContext.CurrentContext.Test.Name} ended with status: {outcome}");

                // Log and report based on the test outcome
                if (outcome == NUnit.Framework.Interfaces.TestStatus.Passed) {

                    ReportingHelper.LogPass("Test passed.");
                    LogsHelper.LogInfo("Test passed.");
                }
                else if (outcome == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    // Log failure message and capture screenshot for debugging
                    ReportingHelper.LogFail($"Test failed: {TestContext.CurrentContext.Result.Message}");
                    LogsHelper.LogError($"Test failed: {TestContext.CurrentContext.Result.Message}");

                    // Capture and save a screenshot for failed tests
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                    var screenshotDirectory = Path.Combine("C:\\AG-DATA\\", "Screenshots");
                    Directory.CreateDirectory(screenshotDirectory);
                    var screenshotPath = Path.Combine(screenshotDirectory, $"{TestContext.CurrentContext.Test.Name}.png");
                    screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);

                    LogsHelper.LogInfo($"Screenshot taken: {screenshotPath}");

                }
            }
            catch (Exception ex) {
                // Log any exceptions that occur during TearDown
                LogsHelper.LogError($"Error during TearDown: {ex.Message}");
                ReportingHelper.LogFail($"Error during TearDown: {ex.Message}");
            }
            finally
            {
                // Dispose Driver to release resources
                Driver?.Quit();
                Driver?.Dispose();
                LogsHelper.LogInfo("Test ended and browser closed.");
            }
        }

        // Runs once at the end of the test suite to flush the report and dispose the logger
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Write all logged entries to the report file
            ReportingHelper.FlushReport();

            // Dispose of the logger to release any file handles
            LogsHelper.Dispose();
        }
    }
}
