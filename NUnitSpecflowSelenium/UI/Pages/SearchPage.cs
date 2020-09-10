using NUnitSpecflowSelenium.Test.Core;
using OpenQA.Selenium;
using System;

namespace NUnitSpecflowSelenium.Test.UI.Pages
{
    public class SearchPage : AbstractPage
    {
        public SearchPage(IWebDriver driver) : base(driver) { }

        IWebElement SearchTxtBox => FindElement(By.Name("q"));
        IWebElement SearchBtn => FindElement(By.Name("btnK"));
        IWebElement FeelingLuckyBtn => FindElement(By.Name("btnI"));

        public SearchResults Search(string criteria)
        {
            SearchTxtBox.SendKeys(criteria);
            return new SearchResults(Driver);
        }

        public void ClickSearch()
        {
            SearchBtn.Click();
        }

        public void ClickFeelingLucky()
        {
            FeelingLuckyBtn.Click();
        }

        public SearchResults WaitForResults()
        {
            SearchResults searchResults = new SearchResults(Driver);
            searchResults.WaitForResults();
            return searchResults;
        }

        public SearchSuggestionBox WaitForSuggestions()
        {
            var googleSearchSuggestionBox = new SearchSuggestionBox(Driver);
            googleSearchSuggestionBox.WaitForSuggestions();
            return googleSearchSuggestionBox;
        }
    }
}
