using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace NUnitSpecflowSelenium.Test.Core
{
    public abstract class AbstractPage
    {
        protected AbstractPage(IWebDriver driver)
        {
            Driver = driver;
        }

        protected IWebDriver Driver { get; }

        public IWebElement FindElement(By locator)
        {
            return Driver.FindElement(locator);
        }

        public E WaitUntil<E>(Func<IWebDriver, E> condition)
        {
            return new WebDriverWait(Driver, TimeSpan.FromSeconds(30)).Until(condition);
        }

        public string GetValueJS(IWebElement element)
        {
            var text = ((IJavaScriptExecutor)Driver).ExecuteScript("return arguments[0].value", element).ToString();
            return text;
        }
    }
}
