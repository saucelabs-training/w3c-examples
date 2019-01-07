import org.junit.jupiter.api.*;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.MutableCapabilities;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.remote.RemoteWebDriver;

import java.net.MalformedURLException;
import java.net.URL;

public class BaseSauce {
    protected WebDriver driver;
    public Boolean result;

    @BeforeEach
    public void setup(TestInfo testInfo) throws MalformedURLException {
        String username = System.getenv("SAUCE_USERNAME");
        String accessKey = System.getenv("SAUCE_ACCESS_KEY");
        String methodName = testInfo.getDisplayName();

        ChromeOptions chromeOpts = new ChromeOptions();
        chromeOpts.setExperimentalOption("w3c", true);

        MutableCapabilities sauceOpts = new MutableCapabilities();
        sauceOpts.setCapability("name", methodName);
        sauceOpts.setCapability("seleniumVersion", "3.11.0");
        sauceOpts.setCapability("user", username);
        sauceOpts.setCapability("accessKey", accessKey);

        DesiredCapabilities caps = new DesiredCapabilities();
        caps.setCapability(ChromeOptions.CAPABILITY,  chromeOpts);
        caps.setCapability("sauce:options", sauceOpts);
        caps.setCapability("browserName", "googlechrome");
        caps.setCapability("browserVersion", "61.0");
        caps.setCapability("platformName", "windows 10");

        String sauceUrl = "https://"+username+":"+accessKey+"@ondemand.saucelabs.com/wd/hub";
        URL url = new URL(sauceUrl);
        driver = new RemoteWebDriver(url, caps);
    }

    @Tag("sample-junit-jupiter-test")
    @Test
    @DisplayName("browserTest()")
    public void browserTest() throws AssertionError {
        driver.navigate().to("https://www.saucedemo.com");
        String getTitle = driver.getTitle();
        Assertions.assertEquals(getTitle, "Swag Labs");
        if (getTitle.equals("Swag Labs")) {
            result = true;
        }else result = false;
    }

    @AfterEach
    public void teardown() {
        ((JavascriptExecutor) driver).executeScript("sauce:job-result=" + (result ? "passed" : "failed"));
        driver.quit();
    }
}
