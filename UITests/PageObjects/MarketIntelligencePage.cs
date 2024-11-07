using OpenQA.Selenium;
using UITests.Helpers;
using UITests.Utils;

namespace UITests.PageObjects
{
    // Page Object class representing the Market Intelligence Page in the UI
    public class MarketIntelligencePage
    {
        // Private field to hold the WebDriver instance
        private readonly IWebDriver _driver;

        // Constructor initializes the page object with a WebDriver instance
        public MarketIntelligencePage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Property to locate the "Get Started" button using its XPath
        private IWebElement GetStartedButton => _driver.FindElement(By.XPath("//*[@id=\"prefooter\"]/a"));

        // Method to retrieve the headings from the "Benefits" section of the Market Intelligence Page
        public List<String> GetBenefitHeadings()
        {
            // Retrieve XPath for the section containing headings from configuration
            var sectionXPath = ConfigHelper.GetPath(Constants.MARKET_INTELLIGENCE_PAGE, Constants.PATH_GET_BENEFIT_HEADINGS);
            var divWithHeadingsXPath = ConfigHelper.GetPath(Constants.MARKET_INTELLIGENCE_PAGE, Constants.PATH_DIV_WITH_HEADINGS);

            // Locate the section containing the divs with headings
            var section = _driver.FindElement(By.XPath(sectionXPath));

            // Find all div elements with h3 headings inside the section
            var divsWithHeadings = section.FindElements(By.XPath(divWithHeadingsXPath));

            // Initialize a list to store the extracted heading text (i.e., Benefits)
            List<String> headings = new List<String>();

            // Iterate through each h3 element to extract text
            foreach (var headingElement in divsWithHeadings)
            {
                String headingText = headingElement.Text;
                headings.Add(headingText);
            }

            // Return the list of extracted headings
            return headings;
        }

        // Method to click the "Get Started" button on the page
        public void ClickGetStartedButton()
        {
            GetStartedButton.Click();
        }
    }
}
