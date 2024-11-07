using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using UITests.Helpers;
using UITests.Utils;

namespace UITests.PageObjects
{
    // Page Object class representing the Contact Page
    public class ContactPage
    {
        // Private field to hold the WebDriver instance for interacting with the browser
        private readonly IWebDriver _driver;

        // Private field to hold the WebDriverWait instance for handling dynamic waits
        private readonly WebDriverWait _wait;

        // Constructor initializes the page object with a WebDriver instance and sets up a WebDriverWait with a 10-second timeout
        public ContactPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // Method to check if the contact form is displayed on the Contact Page
        // Uses a configuration-based XPath to locate the form element
        public bool IsContactFormDisplayed()
        {
            // Find the contact form element using the configured XPath and return its visibility status
            var contactForm = _driver.FindElement(By.XPath(ConfigHelper.GetPath(Constants.CONTACT_PAGE, Constants.PATH_CONTACT_FORM)));
            return contactForm.Displayed;
        }

        // Method to check if the Contact Page has successfully loaded by verifying the URL contains "contact"
        public bool IsLoaded()
        {
            // Wait until the URL contains "contact" to confirm page loading
            return _wait.Until(drv => drv.Url.Contains("contact"));
        }
    }
}