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
            //TODO Add common functionality for each test setup
        }

        /// <summary>
        /// The Test attribute is one way of marking a method inside a TestFixture class as a test.
        /// For more information: https://github.com/nunit/docs/wiki/Test-Attribute
        /// </summary>
        [Test]
        public void W3CChromeTest()
        {
            var chromeOptions = new ChromeOptions()
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10",
                UseSpecCompliantProtocol = true
            };
            var sauceOptions = new Dictionary<string, object>
            {
                ["username"] = _sauceUsername,
                ["accessKey"] = _sauceAccessKey,
                ["name"] = TestContext.CurrentContext.Test.Name
            };

            chromeOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);

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

            // Set up the new Sauce Options for C#
            // For more information: https://wiki.saucelabs.com/display/DOCS/Selenium+W3C+Capabilities+Support+-+Beta
            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("username", _sauceUsername);
            sauceOptions.Add("accessKey", _sauceAccessKey);
            sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);

            // seleniumVersion is REQUIRED for any browser other than Chrome
            sauceOptions.Add("seleniumVersion", "3.141.59");

            ffOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);

            // Sauce Lab's endpoint
            var uri = new Uri("http://ondemand.saucelabs.com/wd/hub");

            // Instantiate the driver with the Uri and browser options
            _driver = new RemoteWebDriver(uri, ffOptions);

            _driver.Navigate().GoToUrl("https://www.saucelabs.com");
            StringAssert.Contains("Sauce Labs", _driver.Title);
        }
        
        /// <summary>
        /// The Test attribute is one way of marking a method inside a TestFixture class as a test.
        /// For more information: https://github.com/nunit/docs/wiki/Test-Attribute
        /// </summary>
        [Test]
        public void W3CInternetExplorerTest()
        {
            // Set up the browser options
            var ieOptions = new InternetExplorerOptions();
            ieOptions.PlatformName = "Windows 10";
            ieOptions.BrowserVersion = "latest";

            // Set up the new Sauce Options for C#
            // For more information: https://wiki.saucelabs.com/display/DOCS/Selenium+W3C+Capabilities+Support+-+Beta
            var sauceOptions = new Dictionary<string, object>()
            {
                ["username"] = _sauceUsername,
                ["accessKey"] = _sauceAccessKey,
                ["name"] = TestContext.CurrentContext.Test.Name,
               
                // Required for Internet Explorer
                ["iedriverVersion"] = "3.12.0",

                // seleniumVersion is REQUIRED for any browser other than Chrome
                ["seleniumVersion"] = "3.141.59"
            };

            ieOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);

            // Sauce Lab's endpoint
            var uri = new Uri("http://ondemand.saucelabs.com/wd/hub");

            // Instantiate the driver with the Uri and browser options
            _driver = new RemoteWebDriver(uri, ieOptions);

            _driver.Navigate().GoToUrl("https://www.google.com");
            Assert.Pass();
        }

        /// <summary>
        /// This attribute is used inside a TestFixture to provide a common set of functions that are performed after each test method.
        /// For more infomration: https://github.com/nunit/docs/wiki/TearDown-Attribute
        /// </summary>
        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            //Checks the status of the test and passes the result to the Sauce Labs job
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            
            _driver?.Quit();
        }
    }
}
