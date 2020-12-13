using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;

namespace AutomationTest
{
    class TestScripts
    {

        IWebDriver driver;
        [SetUp]
        public void Initilize()
        {
            ChromeOptions op = new ChromeOptions();
            op.AddArgument("--start-maximized");
            driver = new ChromeDriver(op);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl("http://automationpractice.com/index.php");

        }
        [Test]
        public void DeleteItemFromBasket()
        {
            driver.Navigate().GoToUrl("http://automationpractice.com/index.php?controller=cart&add=1&id_product=1&token=e817bb0705dd58da8db074c69f729fd8");
            //  System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
            driver.FindElement(By.ClassName("cart_quantity_delete")).Click();
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
            IWebElement test = driver.FindElement(By.ClassName("alert-warning"));
            Assert.IsTrue(test.Text.Contains("Your shopping cart is empty."));

        }
        [Test]
        public void SummerDresses()
        {
            driver.Navigate().GoToUrl("http://automationpractice.com/index.php");
            Actions actions = new Actions(driver);
            IWebElement mainMenu = driver.FindElement(By.XPath("//*[@id='block_top_menu']/ul/li[1]"));
            actions.MoveToElement(mainMenu);

            IWebElement subMenu = driver.FindElement(By.XPath("//*[@id='block_top_menu']/ul/li[1]/ul/li[2]/ul/li[3]/a"));
            actions.MoveToElement(subMenu);
            actions.Click().Build().Perform();


            //to validate that the summer dresses page is opened
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
            String URL = driver.Url;
            Assert.AreEqual(URL, "http://automationpractice.com/index.php?id_category=11&controller=category");

        }

        [Test]
        public void ChangePriceRange()
        {

            driver.Navigate().GoToUrl("http://automationpractice.com/index.php?id_category=11&controller=category");
            string text = driver.FindElement(By.Id("layered_price_range")).Text;
            //slider price change validation
            while (text != "$16.00 - $20.00")
            {
                driver.FindElement(By.XPath("//*[@id='layered_price_slider']/a[2]")).SendKeys(Keys.Left);
                text = driver.FindElement(By.Id("layered_price_range")).Text;
            }
            driver.FindElement(By.XPath("//*[@id='layered_price_slider']/a[2]")).SendKeys(Keys.Enter);

            //validate if the page has loaded the results
            //find the loading text which when disappereared will tell the items has been refreshed
            //give it 20s avg time if its still here
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
            IWebElement test = driver.FindElement(By.ClassName("product_list"));
            //page should be showing results after 20s not loading...
            Assert.IsFalse(test.Text.Contains("Loading..."));

            //validation 3
            //items returned are within price range
            //apply highest first sorting and compare the price of 1st item 
            //apply lowerst first sorting and compare the price of 1st item to check the bound
            driver.FindElement(By.Id("selectProductSort")).Click();
            //lowest first
            driver.FindElement(By.XPath("//*[@id='selectProductSort']/option[2]")).Click();
            //to wait for results to load
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
            if (CheckResults(16, 0) == false)
                Assert.Fail();
            //hightest sorting
            driver.FindElement(By.Id("selectProductSort")).Click();
            driver.FindElement(By.XPath("//*[@id='selectProductSort']/option[3]")).Click();
            //to wait for results to load
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
            if (CheckResults(20, 1) == false)
                Assert.Fail();

        }
        [Test]
        public void RegisterUserWithInvalidEmail()
        {
            driver.FindElement(By.ClassName("login")).Click();
            driver.FindElement(By.Id("email_create")).SendKeys("test@test" + Keys.Enter);
            //validation of correct email address
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 5));
            IWebElement test = driver.FindElement(By.Id("create_account_error"));
            Assert.IsTrue(test.Text.Contains("Invalid email address."));
        }
        [Test]
        public void RegisterUserWithNoData()
        {
            driver.FindElement(By.ClassName("login")).Click();
            driver.FindElement(By.Id("email_create")).SendKeys("faizi@tes.com" + Keys.Enter);
            //validate page with no inputs
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 5));
            driver.FindElement(By.Id("submitAccount")).Click();
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 5));
            IWebElement test = driver.FindElement(By.ClassName("alert-danger"));
            Assert.IsTrue(test.Text.Contains("errors"));
        }
        [Test]
        public void RegisterUserWithinvalidName()
        {
            driver.FindElement(By.ClassName("login")).Click();
            driver.FindElement(By.Id("email_create")).SendKeys("faizi@tes.com" + Keys.Enter);

            driver.FindElement(By.Id("id_gender1")).Click();
            driver.FindElement(By.Id("customer_firstname")).SendKeys("1111");
            driver.FindElement(By.Id("customer_lastname")).SendKeys("Ahmad");
            driver.FindElement(By.Id("passwd")).SendKeys("FaizanAhmadPass");
            driver.FindElement(By.Id("days")).SendKeys("29");
            driver.FindElement(By.Id("months")).SendKeys("May");
            driver.FindElement(By.Id("years")).SendKeys("1996");
            driver.FindElement(By.Id("newsletter")).Click();
            driver.FindElement(By.Id("optin")).Click();
            driver.FindElement(By.Id("company")).SendKeys("Answer Digital");
            driver.FindElement(By.Id("address1")).SendKeys("Birmingham");
            driver.FindElement(By.Id("city")).SendKeys("Birmingham");
            driver.FindElement(By.Id("id_state")).SendKeys("Alabama");
            driver.FindElement(By.Id("postcode")).SendKeys("12345");
            driver.FindElement(By.Id("other")).SendKeys("checking user registration");
            driver.FindElement(By.Id("phone")).SendKeys("1111111111");
            driver.FindElement(By.Id("phone_mobile")).SendKeys("07375118751");
            driver.FindElement(By.Id("alias")).Click();
            driver.FindElement(By.Id("alias")).SendKeys(" alias address");
            driver.FindElement(By.Id("submitAccount")).Click();
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 5));
            IWebElement test = driver.FindElement(By.ClassName("alert-danger"));
            Assert.IsTrue(test.Text.Contains("error"));
        }

        //Same above function can be used to validate all required input fields with corrosponding data


        [Test]
        public void RegisterUser()
        {


            driver.FindElement(By.ClassName("login")).Click();
            driver.FindElement(By.Id("email_create")).SendKeys("faizi@test8.com" + Keys.Enter);



            //validating with correct information
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 5));
            driver.FindElement(By.Id("id_gender1")).Click();
            driver.FindElement(By.Id("customer_firstname")).SendKeys("Faizan");
            driver.FindElement(By.Id("customer_lastname")).SendKeys("Ahmad");
            driver.FindElement(By.Id("passwd")).SendKeys("FaizanAhmadPass");
            driver.FindElement(By.Id("days")).SendKeys("29");
            driver.FindElement(By.Id("months")).SendKeys("May");
            driver.FindElement(By.Id("years")).SendKeys("1996");
            driver.FindElement(By.Id("newsletter")).Click();
            driver.FindElement(By.Id("optin")).Click();
            driver.FindElement(By.Id("company")).SendKeys("Answer Digital");
            driver.FindElement(By.Id("address1")).SendKeys("Birmingham");
            driver.FindElement(By.Id("city")).SendKeys("Birmingham");
            driver.FindElement(By.Id("id_state")).SendKeys("Alabama");
            driver.FindElement(By.Id("postcode")).SendKeys("12345");
            driver.FindElement(By.Id("other")).SendKeys("checking user registration");
            driver.FindElement(By.Id("phone")).SendKeys("1111111111");
            driver.FindElement(By.Id("phone_mobile")).SendKeys("07375118751");
            driver.FindElement(By.Id("alias")).Click();
            driver.FindElement(By.Id("alias")).SendKeys(" alias address");
            driver.FindElement(By.Id("submitAccount")).Click();

            //to validate we are on account page
            //System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
            String URL = driver.Url;
            Assert.AreEqual(URL, "http://automationpractice.com/index.php?controller=my-account");

            //validate user's name on top
            String temp = driver.FindElement(By.ClassName("header_user_info")).Text;
            Assert.IsTrue(temp.Equals("Faizan Ahmad"));

        }
        [Test]
        public void BroswerOurStores()
        {

            //search in search bar with radius to see the stores from inputted address
            //take and save screenshot after searching
            //take screenshot and verify 

            driver.FindElement(By.XPath("//*[@id='block_various_links_footer']/ul/li[4]/a")).Click();
            //click ok on the error popup on maps
            driver.FindElement(By.XPath("//*[@id='map']/div[2]/table/tr/td[2]/button")).Click();
            driver.FindElement(By.Id("addressInput")).Clear();
            driver.FindElement(By.Id("addressInput")).SendKeys("West Palm Beach");
            driver.FindElement(By.Id("radiusSelect")).SendKeys("100");


            driver.FindElement(By.Id("map")).Click();
            //scroll through map
            Actions actionProvider = new Actions(driver);
            for (int i = 0; i <= 1; i++)
            {
                IAction keyMinus = actionProvider.SendKeys(Keys.Subtract).Build();
                keyMinus.Perform();
            }

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,-250)", "");
            driver.FindElement(By.XPath("//*[@id='map']/div/div/div[1]/div[3]/div/div[4]/div/div/div/div/button")).Click();
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 5));

            //screenshot taking validation
            Screenshot scs = ((ITakesScreenshot)driver).GetScreenshot();
            scs.SaveAsFile("screenshotImg", ScreenshotImageFormat.Png);
            scs.ToString();

        }
        public Boolean CheckResults(double j, int lowHighBound)
        {
            //if lowHighBound =0 check for lower bound
            //if lowHighBound =1 check for high bound
            IWebElement temp = driver.FindElement(By.XPath("//*[@id='center_column']/ul/li[1]/div/div[2]/div[1]/span[1]"));
            float test = float.Parse(temp.Text.Trim(new Char[] { '$' }));
            if (lowHighBound == 0)
            {
                if (test > j)
                    return false;
                else
                    return true;
            }
            else if (lowHighBound == 1)
            {
                if (test < j)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
        [TearDown]
        public void EndTest()
        {
            driver.Close();
        }


    }
}
