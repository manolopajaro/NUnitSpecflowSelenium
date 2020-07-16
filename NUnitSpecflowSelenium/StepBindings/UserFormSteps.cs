using OpenQA.Selenium;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TestContext = NUnitSpecflowSelenium.Test.Core.TestContext;

namespace NUnitSpecflowSelenium.Test.StepBindings
{
    [Binding]
    class UserFormSteps
    {
        private readonly TestContext _testContext;

        public UserFormSteps(TestContext testContext) => _testContext = testContext;

        [Given(@"I start entering user form details like")]
        public void GivenIStartEnteringUserFormDetailsLike(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            _testContext.GetUIDriver().FindElement(By.Id("Initial")).SendKeys((string)data.Initial);
            _testContext.GetUIDriver().FindElement(By.Id("FirstName")).SendKeys((string)data.FirstName);
            _testContext.GetUIDriver().FindElement(By.Id("MiddleName")).SendKeys((string)data.MiddleName);
            _testContext.GetUIDriver().FindElement(By.Name("english")).Click();
        }

        [Given(@"I click submit button")]
        public void GivenIClickSubmitButton()
        {
            _testContext.GetUIDriver().FindElement(By.Name("Save")).Click();
        }

        [Then(@"I verify the entered user form details in the application database")]
        public void GivenIVerifyTheEnteredUserFormDetailsInTheApplicationDatabase(Table table)
        {
            //Mock data collection
            List<AUTDatabase> mockAUTData = new List<AUTDatabase>()
            {
                new AUTDatabase()
                {
                    FirstName = "Karthik",
                    Initial = "k",
                    MiddleName = "k"
                },

                new AUTDatabase()
                {
                    FirstName = "Prashanth",
                    Initial = "k",
                    MiddleName = "k"
                }
            };

            //For verification with single row data
            var result = table.FindAllInSet(mockAUTData);

            //For verification againt Multiple row data
            var resultnew = table.FindAllInSet(mockAUTData);

        }

        [Then(@"I logout of application")]
        public void ThenILogoutOfApplication()
        {
            throw new PendingStepException();
        }


    }

    public class AUTDatabase
    {
        public string Initial { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Gender { get; set; }
    }
}
