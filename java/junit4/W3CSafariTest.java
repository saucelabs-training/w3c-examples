package tests.base;

import org.junit.Assert;
import org.junit.Test;
import org.junit.After;
import org.junit.Before;
import org.junit.Rule;
import org.junit.rules.TestName;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.MutableCapabilities;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.safari.SafariOptions;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.remote.RemoteWebDriver;

import java.net.MalformedURLException;
import java.net.URL;

public class W3CSafariTest  {
    protected WebDriver driver;
    public Boolean result;
    
     /**
         * @Rule is a JUnit 4 annotation that defines specific test method behaviors.
            For more information visit the wiki: https://github.com/junit-team/junit4/wiki/rules
    */

    @Rule
    public TestName testName = new TestName() {
        public String getMethodName() {
            return String.format("%s", super.getMethodName());
        }
    };
    
     /**
         * @Before is a JUnit 4 annotation that defines specific prerequisite test method behaviors.
            In the example below we:
            - Define Environment Variables for Sauce Credentials ("SAUCE_USERNAME" and "SAUCE_ACCESS_KEY")
            - Define Safari Options such as W3C protocol
            - Define the "sauce:options" capabilities, indicated by the "sauceOpts" MutableCapability object
            - Define the WebDriver capabilities, indicated by the "caps" DesiredCapabilities object
            - Define the service URL for communicating with SauceLabs.com indicated by "sauceURL" string
            - Set the URL to sauceURl
            - Set the driver instance to a RemoteWebDriver
            - Pass "url" and "caps" as parameters of the RemoteWebDriver
            For more information visit the docs: https://junit.org/junit4/javadoc/4.12/org/junit/Before.html
    */

    @Before
    public void setup() throws MalformedURLException {
        String username = System.getenv("SAUCE_USERNAME");
        String accessKey = System.getenv("SAUCE_ACCESS_KEY");
        String methodName = testName.getMethodName();
        
        /** ChomeOptions allows us to set browser-specific behavior such as profile settings, headless capabilities, insecure tls certs, 
        and in this example--the W3C protocol 
        For more information see: https://seleniumhq.github.io/selenium/docs/api/java/org/openqa/selenium/safari/SafariOptions.html */
        
        SafariOptions safariOpts = new SafariOptions();
        safariOpts.setCapability("w3c", true);
        
        /** The MutableCapabilities class  came into existence with Selenium 3.6.0 and acts as the parent class for 
        all browser implementations--including the SafariOptions class extension.
        Fore more information see: https://seleniumhq.github.io/selenium/docs/api/java/org/openqa/selenium/MutableCapabilities.html */
        
        MutableCapabilities sauceOpts = new MutableCapabilities();
        sauceOpts.setCapability("name", methodName);
        sauceOpts.setCapability("seleniumVersion", "3.141.59");
        sauceOpts.setCapability("username", username);
        sauceOpts.setCapability("accessKey", accessKey);
        sauceOpts.setCapability("tags", "w3c-safari-tests");
        
        /** Below we see the use of our other capability objects, 'safariOpts' and 'sauceOpts', 
        defined in SafariOptions.CAPABILITY and sauce:options respectively.
        */
        DesiredCapabilities caps = new DesiredCapabilities();
        caps.setCapability(SafariOptions.CAPABILITY, safariOpts);
        caps.setCapability("sauce:options", sauceOpts);
        caps.setCapability("browserName", "safari");
        caps.setCapability("browserVersion", "12.0");
        caps.setCapability("platformName", "macOS 10.14");
        
        /** Finally, we pass our DesiredCapabilities object 'caps' as a parameter of our RemoteWebDriver instance */
        String sauceUrl = "https://ondemand.saucelabs.com:443/wd/hub";
        URL url = new URL(sauceUrl);
        driver = new RemoteWebDriver(url, caps);
    }

     /**
         * @Test is a JUnit 4 annotation that defines the actual test case, along with the test execution commands.
            In the example below we:
            - Navigate to our SUT (site under test), 'https://www.saucedemo.com'
            - Store the current page title in a String called 'getTitle'
            - Assert that the page title equals "Swag Labs"
            - Use and If/Else block to determine String match
            For more information visit the docs: https://junit.org/junit4/javadoc/4.12/org/junit/Test.html
    */
   @Test
    public void browserTest() throws AssertionError {
        driver.navigate().to("https://www.saucedemo.com");
        String getTitle = driver.getTitle();
        Assert.assertEquals(getTitle, "Swag Labs");
        if (getTitle.equals("Swag Labs")) {
            result = true;
        }else result = false;
    }

    /**
         * @After is a JUnit 4 annotation that defines any postrequisite test method tasks .
            In the example below we:
            - Use the JavascriptExecutor class to send our test results to Sauce Labs with a "passed" flag
                if the test was successful, or a"failed" flag if the test was unsuccessful.
            - Teardown the RemoteWebDriver session with a 'driver.quit()' command so that the test VM doesn't hang.
            For more information visit the docs: https://junit.org/junit4/javadoc/4.12/org/junit/After.html
    */
    @After
    public void teardown() {
        ((JavascriptExecutor) driver).executeScript("sauce:job-result=" + (result ? "passed" : "failed"));
        System.out.println(result);
        driver.quit();
    }
}

