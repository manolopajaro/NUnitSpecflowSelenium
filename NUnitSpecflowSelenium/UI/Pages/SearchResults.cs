using NUnitSpecflowSelenium.Test.Core;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System.Collections.Generic;

namespace NUnitSpecflowSelenium.Test.UI.Pages
{
    public class SearchResults : AbstractPage
    {
        public SearchResults(IWebDriver driver) : base(driver) { }

        private readonly By ResultsLocator = By.ClassName("g");

        List<IWebElement> Results => new List<IWebElement>(FindElements(ResultsLocator));
        IWebElement ShowingResultsForLabel => FindElement(By.XPath(".//span[@class = 'gL9Hy']"));

        public void WaitForResults()
        {
            WaitUntil((IWebDriver)=>
            {
                return Results.Count > 0;
            });
        }

        public IWebElement getFirstResult()
        {
            return Results[0].FindElement(By.CssSelector("a > h3"));
        }

        public List<IWebElement> getResults()
        {
            return this.Results;
        }

        public void clickFirstResult()
        {
            var firstResult = Results[0].FindElement(By.XPath(".//div[1]/a"));
            firstResult.Click();
        }

        public bool IsLabelShowingResultsForDisplayed()
        {
            bool isDisplayed;
            try
            {
                isDisplayed = ShowingResultsForLabel.Displayed;
            }
            catch (ElementNotVisibleException)
            {
                isDisplayed = false;
            }
            return isDisplayed;
        }
    }
}
