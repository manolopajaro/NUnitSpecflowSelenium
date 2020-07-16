using NUnitSpecflowSelenium.Test.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;

namespace NUnitSpecflowSelenium.Test.Configuration
{
    public class DriverFactory
    {
        enum Browser
        {
            Chrome,
            InternetExplorer
        }

        public IWebDriver GetDriver(ConfigurationContext configurationContext, MyBussinessEnvironment environment)
        {
            var env = System.Environment.GetEnvironmentVariable("BROWSER");
            var browser = env ?? configurationContext.AppSetting("Browser");
            IWebDriver driver = null;
            if (browser.Equals(Browser.Chrome.ToString()))
            {
                driver = new ChromeDriver();
            }
            else if (browser.Equals(Browser.InternetExplorer.ToString()))
            {
                driver = new InternetExplorerDriver();
            }
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(environment.ImplicitTimeout);
            driver.Manage().Window.Maximize();
            driver.Manage().Cookies.DeleteAllCookies();

            return driver;
        }
    }
}
