using NUnitSpecflowSelenium.Test.Core;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace NUnitSpecflowSelenium.Test.UI.Pages
{
    public class SearchSuggestionBox : AbstractPage
    {
        public SearchSuggestionBox(IWebDriver driver) : base(driver) { }

        private readonly By SuggestionListLocator = By.ClassName("UUbT9");

        IWebElement SuggestionList => FindElement(SuggestionListLocator);

        public void WaitForSuggestions()
        {
            WaitUntil(ExpectedConditions.ElementIsVisible(SuggestionListLocator));
        }

        public void ClickFirstSuggestion()
        {
            var firstSuggestion = SuggestionList.FindElement(By.XPath(".//li[1]"));
            firstSuggestion.Click();
        }
    }
}
