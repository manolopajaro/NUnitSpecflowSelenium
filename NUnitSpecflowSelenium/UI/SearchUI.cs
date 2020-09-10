using NUnitSpecflowSelenium.Test.UI.Pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace NUnitSpecflowSelenium.Test.UI
{
    public class SearchUI
    {
        private readonly SearchPage SearchPage;
        private SearchResults SearchResults;

        public SearchUI(IWebDriver driver)
        {
            SearchPage = new SearchPage(driver);
        }

        public void Search(string searchCriteria)
        {
            SearchPage.Search(searchCriteria);
            SearchPage.ClickSearch();            
        }

        public void WaitForResults()
        {
            SearchResults = SearchPage.WaitForResults();
        }

        public List<IWebElement> Results()
        {
            return SearchResults.getResults();
        }
    }
}
