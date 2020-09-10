using NUnit.Framework;
using NUnitSpecflowSelenium.Test.UI;
using System;
using TechTalk.SpecFlow;

namespace NUnitSpecflowSelenium.Test.StepBindings
{
    [Binding]
    public class SearchSteps
    {
        private readonly Core.TestContext _testContext;
        private SearchUI _searchUI;

        public SearchSteps(Core.TestContext testContext) => _testContext = testContext;


        [Given("^The user searches for (.*)")]
        public void UserSearchesFor(String searchCriteria)
        {
            _searchUI = new SearchUI(_testContext.GetUIDriver());
            _searchUI.Search(searchCriteria);
        }

        [When("^Displaying search results")]
        public void DisplayingSearchResult()
        {
            _searchUI.WaitForResults();
        }

        [Then("^the number of results must not exceed (.*)")]
        public void ResultsMustNotExceed(int limit)
        {
            var numberOfResultsInPage = _searchUI.Results().Count;
            Assert.LessOrEqual(numberOfResultsInPage, limit, "Exceeded number of results per page");
        }
    }
}
