using NUnitSpecflowSelenium.Test.UI.Pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitSpecflowSelenium.Test.UI
{
    public class LoginUI
    {
        private readonly LoginPage LoginPage;

        public LoginUI(IWebDriver driver)
        {
            LoginPage = new LoginPage(driver);
        }

        public void TypeUserAndPass(string user, string password)
        {
            LoginPage.TypeEmail(user);
            LoginPage.TypePassword(password);            
        }
        public void ClickLogin()
        {
            LoginPage.ClickLogin();
        }
    }
}
