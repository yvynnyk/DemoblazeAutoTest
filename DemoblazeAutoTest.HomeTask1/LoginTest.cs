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
        }

        [Test]
        public void VerifyValidLogin()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            testUrl = "https://www.demoblaze.com/index.html";
            userName = "userTest001";
            userPassword = "testPassword";

            driver.Navigate().GoToUrl(testUrl);

            IWebElement buttonLogin =  driver.FindElement(By.Id("login2"));
            //other options: 
                // xPath       =  //ul//li//a[@data-target='#logInModal'] or //a[text()='Log in']
                // cssSelector =  #login2 or a[data-target='#logInModal']
            buttonLogin.Click();

            wait.Until(drv => drv.FindElement(By.Id("logInModal")));
            //other options: 
                // xPath       =  //div[@aria-labelledby="logInModalLabel"]
                // cssSelector =  #login2 or a[data-target='#logInModal']
            
            IWebElement inputUsername = driver.FindElement(By.Id("loginusername"));
            //other options:
                //xPath (complex) = //form//div[@class='form-group']//label[@for='log-name']/following-sibling::input
                //cssSelector     = input[id="loginusername"]
            inputUsername.Clear();
            inputUsername.SendKeys(userName);
            
            IWebElement inputPassword = driver.FindElement(By.Id("loginpassword"));
            //other options:
                //xPath (complex) = //label[@for="log-pass"]/parent::div/child::input
                //cssSelector     = input[id='sign-password']
            inputPassword.Clear();
            inputPassword.SendKeys(userPassword);

            IWebElement buttonLoginSubmit = driver.FindElement(By.CssSelector("button.btn.btn-primary[onclick='logIn()']"));
            //other option:
                //XPath = //div[@class='modal-footer']//button[@onclick='logIn()']
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
