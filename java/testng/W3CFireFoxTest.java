import org.openqa.selenium.WebDriver;
import org.testng.ITestResult;
import org.testng.annotations.*;
import org.testng.asserts.*;
import org.openqa.selenium.MutableCapabilities;
import org.openqa.selenium.firefox.FirefoxOptions;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.remote.RemoteWebDriver;
import org.openqa.selenium.JavascriptExecutor;

import java.lang.reflect.Method;
import java.net.MalformedURLException;
import java.net.URL;

public class W3CFirefoxTest {
    protected WebDriver driver;
    
    /**
         * @BeforeMethod is a TestNG annotation that defines specific prerequisite test method behaviors.
            In the example below we:
            - Define Environment Variables for Sauce Credentials ("SAUCE_USERNAME" and "SAUCE_ACCESS_KEY")
            - Define Firefox Options such as W3C protocol
            - Define the "sauce:options" capabilities, indicated by the "sauceOpts" MutableCapability object
            - Define the WebDriver capabilities, indicated by the "caps" DesiredCapabilities object
            - Define the service URL for communicating with SauceLabs.com indicated by "sauceURL" string
            - Set the URL to sauceURl
            - Set the driver instance to a RemoteWebDriver
            - Pass "url" and "caps" as parameters of the RemoteWebDriver
            For more information visit the docs: http://static.javadoc.io/org.testng/testng/6.9.4/org/testng/annotations/BeforeMethod.html
    */
    @BeforeMethod
    public void setup(Method method) throws MalformedURLException {
        String username = System.getenv("SAUCE_USERNAME");
        String accessKey = System.getenv("SAUCE_ACCESS_KEY");
        String methodName = method.getName();

       /** The MutableCapabilities class  came into existence with Selenium 3.6.0 and acts as the parent class for 
        all browser implementations--including the FirefoxOptions class extension.
        Fore more information see: https://seleniumhq.github.io/selenium/docs/api/java/org/openqa/selenium/MutableCapabilities.html */
        
        MutableCapabilities sauceOpts = new MutableCapabilities();
        sauceOpts.setCapability("name", methodName);
        sauceOpts.setCapability("seleniumVersion", "3.141.59");
        sauceOpts.setCapability("username", username);
        sauceOpts.setCapability("accessKey", accessKey);
        sauceOpts.setCapability("tags", "w3c-firefox-tests")

        /** FirefoxOptions allows us to set browser-specific behavior such as profile settings, headless capabilities, insecure tls certs,
         and in this example--the W3C protocol
         For more information see: https://seleniumhq.github.io/selenium/docs/api/java/org/openqa/selenium/firefox/FirefoxOptions.html */

        FirefoxOptions foxOpts = new FirefoxOptions();
        options.setCapability("w3c", true);
        options.setCapability("browserName", "firefox");
        options.setCapability("browserVersion", "64.0");
        options.setCapability("platformName", "windows 10");
        option.setCapability("sauce:options", sauceOpts);

        /** Finally, we pass our DesiredCapabilities object 'caps' as a parameter of our RemoteWebDriver instance */
        String sauceUrl = "https://ondemand.saucelabs.com:443/wd/hub";
        URL url = new URL(sauceUrl);
        driver = new RemoteWebDriver(url, caps);
    }
    
    /**
         * @Test is a TestNG annotation that defines the actual test case, along with the test execution commands.
            In the example below we:
            - Navigate to our SUT (site under test), 'https://www.saucedemo.com'
            - Store the current page title in a String called 'getTitle'
            - Assert that the page title equals "Swag Labs"
            For more information visit the docs: http://static.javadoc.io/org.testng/testng/6.9.4/org/testng/annotations/Test.html
    */
    @Test
    public void w3cFirefoxTest() throws AssertionError {
        driver.navigate().to("https://www.saucedemo.com");
        String getTitle = driver.getTitle();
        Assert.assertEquals(getTitle, "Swag Labs");
    }
    /**
         * @AfterMethod is a TestNG annotation that defines any postrequisite test method tasks .
            In the example below we:
            - Pass the ITestResult class results to a parameter called 'result'
            - Use the JavascriptExecutor class to send our test 'result' to Sauce Labs with a "passed" flag
                if the test was successful, or a "failed" flag if the test was unsuccessful.
            - Teardown the RemoteWebDriver session with a 'driver.quit()' command so that the test VM doesn't hang.
            For more information visit the docs: http://static.javadoc.io/org.testng/testng/6.9.4/org/testng/annotations/AfterMethod.html
    */
    @AfterMethod
    public void teardown(ITestResult result) {
        ((JavascriptExecutor)driver).executeScript("sauce:job-result=" + (result.isSuccess() ? "passed" : "failed"));
        driver.quit();
    }
}