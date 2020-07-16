using NUnitSpecflowSelenium.Test.Core;
using OpenQA.Selenium;

namespace NUnitSpecflowSelenium.Test.UI.Pages
{
    public class LoginPage : AbstractPage
    {

        public LoginPage(IWebDriver driver) : base(driver) { }

        IWebElement TxtUserName => FindElement(By.Name("UserName"));
        IWebElement TxtPassword => FindElement(By.Name("Password"));
        IWebElement BtnLogin => FindElement(By.Name("Login"));

        public void TypeEmail(string user)
        {
            TxtUserName.SendKeys(user);
        }
        public void TypePassword(string password)
        {
            TxtPassword.SendKeys(password);
        }
        public void ClickLogin()
        {
            BtnLogin.Submit();
        }
    }
}
