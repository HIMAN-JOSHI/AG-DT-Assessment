using Common.Helpers;
using OpenQA.Selenium.DevTools;
using UITests.PageObjects;
using UITests.Utils;

namespace UITests
{
    // Test fixture for Market Intelligence workflow tests
    [TestFixture]
    public class MarketIntelligenceTests : BaseUITest
    {
        // Private fields to hold page object instances
        private HomePage _homePage;
        private MarketIntelligencePage _marketIntelligencePage;
        private ContactPage _contactPage;

        // Setup method that runs before each test
        [SetUp]
        public void Setup()
        {
            // Initialize page objects with the current WebDriver instance
            _homePage = new HomePage(Driver);
            _marketIntelligencePage = new MarketIntelligencePage(Driver); ;
            _contactPage = new ContactPage(Driver);

            // Log the initialization of page objects for debugging purposes
            LogsHelper.LogInfo("Page objects initialized.");
        }

        // Test case for validating the workflow in the Market Intelligence page
        [Test, Description("Verifies that clicking on 'Let's Get Started' on the MarketIntelligence page navigates to the 'Contact' page.")]
        public void MarketIntelligenceWorkflow_ValidateContactPageLoad()
        {
            // Navigate to the main page URL specified in constants
            Driver.Navigate().GoToUrl(Constants.AG_DATA_URL);
            LogsHelper.LogInfo($"Navigated to {Constants.AG_DATA_URL}");

            // Navigate to the Market Intelligence page via the 'Solutions' menu
            _homePage.OpenMarketIntelligencePage();
            LogsHelper.LogInfo("Clicked on 'Solutions' > 'Market Intelligence'");

            // Get and validate that there are headings in the "Ways You Benefit" section
            var benefitHeadings = _marketIntelligencePage.GetBenefitHeadings(); ;
            Assert.IsTrue(benefitHeadings.Count > 0, "Expected headings in 'Ways You Benefit' section.");
            LogsHelper.LogInfo($"Benefit headings count: {benefitHeadings.Count}");

            // Click the "Let's Get Started" button to proceed
            _marketIntelligencePage.ClickGetStartedButton();
            LogsHelper.LogInfo("'Let's Get Started' button clicked.");

            // Validate that the Contact page loads successfully
            Assert.IsTrue(_contactPage.IsLoaded(), "Expected 'Contact' page to be displayed.");
            LogsHelper.LogInfo("'Contact' page loaded successfully.");

            // Check that the Contact form is visible on the Contact page
            Assert.IsTrue(_contactPage.IsContactFormDisplayed(), "Expected 'Contact' form to be displayed.");
            LogsHelper.LogInfo("'Contact' form displayed successfully.");
            
        }
    }
}