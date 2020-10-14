using System;
using System.Data;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using AudenTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AudenTest.Steps
{
    [Binding]
    public class Steps
    {
        public IWebDriver driver;

        [TestInitialize]
        [Given(@"a web browser is at the Auden loan page")]
        public void GivenAWebBrowserIsAtTheAudenLoanPage()
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(@"C:\Users\User\source\repos\AudenTest\AudenTest\chromedriver_win32", "chromedriver.exe");
            ChromeDriverService service = chromeDriverService;
            driver = new ChromeDriver(service);

            //Navigate to URL https://www.auden.co.uk/credit/shorttermloan
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            driver.Navigate().GoToUrl("https://www.auden.co.uk/credit/shorttermloan");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            
            var session = driver.Manage().Cookies.AllCookies.ToArray();
            foreach (Cookie cookie in session)
            {
                driver.Manage().Cookies.DeleteCookieNamed(cookie.Name);
                driver.Manage().Cookies.AddCookie(cookie);
            }

            session = driver.Manage().Cookies.AllCookies.ToArray();

            Assert.IsTrue(driver.Title.ToLower().Contains("Auden|A socially responsible financial service"));            
        }

        [TestMethod]
        [When(@"I select the loan_amount and repayment day as weekend")]
        public void WhenISelectTheLoan_AmountAndRepaymentDayAsWeekend(Table table)
        {
            var dataTable = TableExtension.ToDataTable(table);
            foreach (DataRow row in dataTable.Rows)
            {
                //selecting the loan amount from the slider
                driver.FindElement(By.Id(".loan-amount__range-slider__input")).SendKeys(row.ItemArray[0].ToString());
                //selecting the repayment date from the date picker
                driver.FindElement(By.XPath(".//*[@id='monthly']")).SendKeys(row.ItemArray[1].ToString());
            }
        }

        [TestCleanup]
        [Then(@"the loan amount and the first repayment day are shown as Friday")]
        public void ThenTheLoanAmountAndTheFirstRepaymentDayAreShownAsFriday()
        {
            //Checking the repayment date
            var element = driver.FindElement(By.ClassName(".loan - schedule__tab__panel__detail__tag__text']/a"));
            Assert.IsTrue(element.Displayed);
            Assert.AreEqual(element.Text.ToLower(), "Expected text".ToLower());

            Assert.IsTrue(driver.FindElement(By.ClassName("")).Displayed);

            //checking the loan amount value
            var element2 = driver.FindElement(By.ClassName(".loan - summary__column__amount__value']/a"));
            Assert.IsTrue(element2.Displayed);
            Assert.AreEqual(element2.Text.ToLower(), "Expected text".ToLower());

            //existing the browser
            driver.Quit();
        }
    }
}
