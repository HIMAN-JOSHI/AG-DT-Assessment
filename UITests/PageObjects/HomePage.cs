using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using UITests.Helpers;
using UITests.Utils;

namespace UITests.PageObjects
{
    // Page Object class representing the Home Page in the UI
    public class HomePage
    {
        // Private field to hold the WebDriver instance for interacting with the browser
        private readonly IWebDriver _driver;

        // Private field to hold the Actions instance, used for advanced user interactions
        private readonly Actions _actions;

        // Constructor initializes the page object with a WebDriver instance and an Actions instance
        public HomePage(IWebDriver driver)
        {
            _driver = driver;
            _actions = new Actions(_driver);
        }

        // Property to locate the "Solutions" menu element using XPath retrieved from configuration
        private IWebElement SolutionsMenu => _driver.FindElement(By.XPath(ConfigHelper.GetPath(Constants.HOME_PAGE, Constants.PATH_SOLUTIONS_MENU)));

        // Property to locate the "Market Intelligence" submenu element using XPath retrieved from configuration
        private IWebElement MarketIntelligenceSubMenu => _driver.FindElement(By.XPath(ConfigHelper.GetPath(Constants.HOME_PAGE, Constants.PATH_MARKET_INTELLIGENCE_SUB_MENU)));

        // Method to navigate to the Market Intelligence Page by hovering over the Solutions menu and clicking the submenu
        public void OpenMarketIntelligencePage()
        {
            // Use Actions to move to the Solutions menu and click the Market Intelligence submenu, then perform the action
            _actions.MoveToElement(SolutionsMenu).Click(MarketIntelligenceSubMenu).Perform();
        }
    }
}