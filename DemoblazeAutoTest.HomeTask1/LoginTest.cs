using NUnit.Framework;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.IO;

namespace DemoblazeAutoTest.HomeTask1
{
    [TestFixture]
    public class LoginTest
    {
        IWebDriver driver;
        string testUrl;
        WebDriverWait wait;
        string userName;
        string userPassword;

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            testUrl = "https://www.demoblaze.com/index.html";
            userName = "userTest001";
            userPassword = "testPassword";
        }

        [Test]
        public void VerifyValidLogin()
        {
            driver.Navigate().GoToUrl(testUrl);

            IWebElement buttonLogin =  driver.FindElement(By.Id("login2"));
            //other options: 
            buttonLogin = driver.FindElement(By.XPath("//ul//li//a[@data-target='#logInModal']"));
            buttonLogin = driver.FindElement(By.XPath("//a[text()='Log in']"));
            buttonLogin = driver.FindElement(By.CssSelector("#login2"));
            buttonLogin = driver.FindElement(By.CssSelector("a[data-target='#logInModal']"));

            buttonLogin.Click();

            wait.Until(drv => drv.FindElement(By.Id("logInModal")));
            //other option: 
            wait.Until(drv => drv.FindElement(By.XPath("//div[@aria-labelledby='logInModalLabel']")));

            IWebElement inputUsername = driver.FindElement(By.Id("loginusername"));
            //other options:
            inputUsername = driver.FindElement(By.XPath("//form//div[@class='form-group']//label[@for='log-name']/following-sibling::input"));
            inputUsername = driver.FindElement(By.CssSelector("input[id='loginusername']"));
            
            inputUsername.Clear();
            inputUsername.SendKeys(userName);
            
            IWebElement inputPassword = driver.FindElement(By.Id("loginpassword"));
            //other options:
            inputPassword = driver.FindElement(By.XPath("//label[@for='log-pass']/parent::div/child::input"));
            inputPassword = driver.FindElement(By.CssSelector("input[id='loginpassword']"));
            
            inputPassword.Clear();
            inputPassword.SendKeys(userPassword);

            IWebElement buttonLoginSubmit = driver.FindElement(By.CssSelector("button.btn.btn-primary[onclick='logIn()']"));
            //other option:
            buttonLoginSubmit = driver.FindElement(By.XPath("//div[@class='modal-footer']//button[@onclick='logIn()']"));
            
            buttonLoginSubmit.Click();

            IWebElement welcomeUsername = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nameofuser")));

            Assert.Multiple(() =>
            {
                Assert.That(driver.FindElement(By.Id("logout2")).Displayed, Is.True, "Logout is not displayed");
                Assert.That(welcomeUsername.Text, Is.EqualTo("Welcome " + userName), "Actual username differs from expected");
            });
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}
