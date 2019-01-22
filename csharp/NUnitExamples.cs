using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace SeleniumNunit.SimpleExamples
{
    [TestFixture]
    [Category("Selenium 4 tests")]
    public class NUnitExamples
    {
        private IWebDriver _driver;
        private string _sauceUsername;
        private string _sauceAccessKey;
        private Dictionary<string, object> _sauceOptions = new Dictionary<string, object>();

        /// <summary>
        /// This attribute is to identify methods that are called once prior to executing any of the tests in a fixture. 
        /// For more information: https://github.com/nunit/docs/wiki/OneTimeSetUp-Attribute
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //TODO please supply your Sauce Labs user name in an environment variable
            _sauceUsername = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);

            //TODO please supply your own Sauce Labs access Key in an environment variable
            _sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);
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
            _sauceOptions.Add("username", _sauceUsername);
            _sauceOptions.Add("accessKey", _sauceAccessKey);
            _sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);

            // Required for any browser other than Chrome
            _sauceOptions.Add("seleniumVersion", "3.141.59");
        }

        [Test]
        public void W3CChromeTest()
        {
            var chromeOptions = new ChromeOptions()
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10",
                UseSpecCompliantProtocol = true
            };

            chromeOptions.AddAdditionalCapability("sauce:options", _sauceOptions, true);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.saucelabs.com/wd/hub"),
                chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
            _driver.Navigate().GoToUrl("https://www.google.com");
            Assert.Pass();
        }

        /// <summary>
        /// The Test attribute is one way of marking a method inside a TestFixture class as a test.
        /// For more information: https://github.com/nunit/docs/wiki/Test-Attribute
        /// </summary>
        [Test]
        public void W3CFirefoxTest()
        {
            // Set up the browser options
            var ffOptions = new FirefoxOptions();
            ffOptions.PlatformName = "Windows 10";
            ffOptions.BrowserVersion = "latest";
            ffOptions.AddAdditionalCapability("sauce:options", _sauceOptions, true);

            // Sauce Lab's endpoint
            var uri = new Uri("http://ondemand.saucelabs.com/wd/hub");

            // Instantiate the driver with the Uri and browser options
            _driver = new RemoteWebDriver(uri, ffOptions);
            
            _driver.Navigate().GoToUrl("https://www.saucelabs.com");
            StringAssert.Contains("Sauce Labs", driver.Title);
        }

        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            _driver?.Quit();
        }
    }
}
