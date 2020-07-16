using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TestContext = NUnitSpecflowSelenium.Test.Core.TestContext;

namespace NUnitSpecflowSelenium.Test.UI.StepBindings
{
    [Binding]
    public class LoginSteps
    {

        private readonly TestContext _testContext;
        private LoginUI _loginUI;

        public LoginSteps(TestContext testContext) => _testContext = testContext;

        [Given(@"I enter username and password")]
        public void GivenIEnterUsernameAndPassword(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            _loginUI = new LoginUI(_testContext.GetUIDriver());
            _loginUI.TypeUserAndPass(data.UserName, data.Password);
        }

        [Given(@"I click login")]
        public void GivenIClickLogin()
        {
            _loginUI.ClickLogin();
        }

        [Then(@"I should see user logged in to the application")]
        public void ThenIShouldSeeUserLoggedInToTheApplication()
        {
            var element = _testContext.GetUIDriver().FindElement(By.XPath("//h1[contains(text(),'Execute Automation Selenium')]"));

            //An way to assert multiple properties of single test
            Assert.Multiple(() =>
            {
                //Assert.That(element.Text, Is.Null, "Header text not found !!!");
                Assert.That(element.Text, Is.Null, "Header text not found !!!");
            });
        }


    }
}
