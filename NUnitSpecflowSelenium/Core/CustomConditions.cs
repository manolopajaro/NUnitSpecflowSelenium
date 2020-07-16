using OpenQA.Selenium;
using System;

namespace NUnitSpecflowSelenium.Test.Core
{
    public static class CustomConditions
    {
        public static Func<IWebDriver, bool> ImageIsVisible(By locator)
        {
            return (driver) =>
            {
                try
                {
                    var el = driver.FindElement(locator);
                    var staus = Boolean.Parse(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete", el).ToString());
                    return staus;
                }
                catch (Exception)
                {
                    return false;
                }
            };
        }
    }
}
