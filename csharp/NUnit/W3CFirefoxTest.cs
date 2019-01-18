using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace SeleniumNunit.SimpleExamples
{
    [TestFixture]
    public class W3CFireFoxTest
    {
        private IWebDriver driver;
        private string sauceUsername;
        private string sauceAccessKey;

        /// <summary>
        /// This attribute is to identify methods that are called once prior to executing any of the tests in a fixture. 
        /// For more information: https://github.com/nunit/docs/wiki/OneTimeSetUp-Attribute
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //Loads the Sauce username from the environmental variables
            sauceUsername = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);

            //Loads the Sauce access key from the environmental variables
            sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);
        }

        /// <summary>
        /// This attribute is used inside a TestFixture to provide a common set of functions that are performed just before each test method is called.
        /// For more information: https://github.com/nunit/docs/wiki/SetUp-Attribute
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Set up the new Sauce Options for C#
            // For more information: https://wiki.saucelabs.com/display/DOCS/Selenium+W3C+Capabilities+Support+-+Beta
            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("username", sauceUsername);
            sauceOptions.Add("accessKey", sauceAccessKey);
            sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);

            // Required for any browser other than Chrome
            sauceOptions.Add("seleniumVersion", "3.11.0");

            // Set up the browser options
            var ffOptions = new FirefoxOptions();
            ffOptions.PlatformName = "Windows 10";
            ffOptions.BrowserVersion = "latest";
            ffOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);

            // Sauce Lab's endpoint
            var uri = new Uri("http://ondemand.saucelabs.com/wd/hub");

            // Instantiate the driver with the Uri and browser options
            driver = new RemoteWebDriver(uri, ffOptions);
        }

        /// <summary>
        /// The Test attribute is one way of marking a method inside a TestFixture class as a test.
        /// For more information: https://github.com/nunit/docs/wiki/Test-Attribute
        /// </summary>
        [Test]
        [Category("W3C Firefox Tests")]
        public void W3CFirefox()
        {
            driver.Navigate().GoToUrl("https://www.saucelabs.com");
            StringAssert.Contains("Sauce Labs", driver.Title);
        }

        /// <summary>
        /// This attribute is used inside a TestFixture to provide a common set of functions that are performed after each test method.
        /// For more infomration: https://github.com/nunit/docs/wiki/TearDown-Attribute
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            //Checks the status of the test and passes the result to the Sauce Lab's job
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-result=" + (status == TestStatus.Passed ? "passed" : "failed"));

            driver?.Quit();
        }
    }
}
