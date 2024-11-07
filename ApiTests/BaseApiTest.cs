using ApiTests.Helpers;
using ApiTests.Services;
using ApiTests.Utilities;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Common.Helpers;
using TestContext = NUnit.Framework.TestContext;

namespace ApiTests
{
    /// <summary>
    /// Base class for API tests, providing setup, teardown, and logging functionality.
    /// </summary>
    public abstract class BaseApiTest
    {
        // Dependencies for API helper and logging
        public required ApiHelper ApiHelper { get; set; }
        public required ILogger<PostService> Logger { get; set; }

        /// <summary>
        /// One-time setup: Initializes logger and report, and creates ApiHelper instance.
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Initialize logger and reporting
            LogsHelper.InitializeLogger("C:\\AG-DATA\\Logs\\api-test-logs.txt");
            ReportingHelper.InitializeReport(Constants.REPORT_TYPE_API);

            // Set up logger and API helper for tests
            Logger = LogsHelper.GetLogger<PostService>();
            ApiHelper = new ApiHelper(Constants.BASE_URL, Logger);
        }

        /// <summary>
        /// Base setup: Creates a new test entry in the report for each test case.
        /// </summary>
        [SetUp]
        public void BaseSetup()
        {
            // Log the start of the test case
            ReportingHelper.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        /// <summary>
        /// Tear down: Logs test outcome (pass/fail) and any related messages.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Log teardown process
            Logger.LogInformation("TearDown after test execution.");

            // Log test result (pass or fail)
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;
            if (outcome == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                ReportingHelper.LogPass("Test passed");
            }
            else if (outcome == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                ReportingHelper.LogFail("Test failed: " + TestContext.CurrentContext.Result.Message);
            }
        }

        /// <summary>
        /// One-time teardown: Flushes the report and disposes of resources.
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Finalize report and clean up resources
            ReportingHelper.FlushReport();
            LogsHelper.Dispose();
        }
    }
}
