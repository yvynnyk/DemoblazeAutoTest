using NUnit.Framework;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DemoblazeAutoTest.HomeTask1
{
    [TestFixture]
    public class LoginTest
    {
        IWebDriver driverChrome;
        WebDriverWait wait;
        string userName;
        string userPassword;

        [OneTimeSetUp]
        public void Setup()
        {
            driverChrome = new ChromeDriver();
            driverChrome.Manage().Window.Maximize();
            userName = "userTest001";
            userPassword = "testPassword";
            wait = new WebDriverWait(driverChrome, TimeSpan.FromSeconds(5));
        }

        [Test]
        public void VerifyValidLogin()
        {
            driverChrome.Url = "https://www.demoblaze.com/index.html";

            IWebElement buttonLogin =  driverChrome.FindElement(By.Id("login2"));
            //other options: 
                // xPath       =  //ul//li//a[@data-target='#logInModal'] or //a[text()='Log in']
                // cssSelector =  #login2 or a[data-target='#logInModal']
            buttonLogin.Click();

            IWebElement modalWindowLogin = wait.Until(e => e.FindElement(By.Id("logInModal")));
            //other options: 
                // xPath       =  //div[@aria-labelledby="logInModalLabel"]
                // cssSelector =  #login2 or a[data-target='#logInModal']
            
            IWebElement inputUsername = driverChrome.FindElement(By.Id("loginusername"));
            //other options:
                //xPath (complex) = //form//div[@class='form-group']//label[@for='log-name']/following-sibling::input
                //cssSelector     = input[id="loginusername"]
            inputUsername.Clear();
            inputUsername.SendKeys("userTest001");
            
            IWebElement inputPassword = driverChrome.FindElement(By.Id("loginpassword"));
            //other options:
                //xPath (complex) = //label[@for="log-pass"]/parent::div/child::input
                //cssSelector     = input[id='sign-password']
            inputPassword.Clear();
            inputPassword.SendKeys(userPassword);

            IWebElement buttonLoginSubmit = driverChrome.FindElement(By.CssSelector("button.btn.btn-primary[onclick='logIn()']"));
            //other option:
                //XPath = //div[@class='modal-footer']//button[@onclick='logIn()']
            buttonLoginSubmit.Click();

            IWebElement logoutButton = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("logout2")));
            //other options:
                //xPath (complex) = //a[@onclick="logOut()"]
                //cssSelector     = a[id$='out2']

            IWebElement welcomeUsername = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nameofuser")));

            Assert.Multiple(() =>
            {
                Assert.That(logoutButton.Displayed, Is.True, "Logout is not displayed");
                Assert.That(welcomeUsername.Text.Split()[1], Is.EqualTo(userName), "Actual username differs from expected");
            });
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            driverChrome.Quit();
        }
    }
}
