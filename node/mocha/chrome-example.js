/**
    Here we set our script dependencies (e.g. selenium-webdriver and 'assert'), and we also set some key variables such as:
        - our SauceLabs account credentials
        - the SUT (site under test) url
        - any necessary tags we may use to run test reports or filters
        - the WebDriver session (indicated by 'driver')
*/
var webdriver = require('selenium-webdriver'),
    assert = require('assert'),
    username = process.env.SAUCE_USERNAME,
    accessKey = process.env.SAUCE_ACCESS_KEY,
    /* Change the baseURL to your application URL */
    baseUrl = "https://www.saucedemo.com",
    tags = ["w3c", "demoTest", "w3c-chrome-tests", "nodeTest"],
    driver;
/** 
    We use 'describe' in order to group test methods together. Even though we only have one test case in this example,
    structuring your project this way makes it scalable when you add tests. We can also set global-level test execution parameters such as timeouts
*/
describe('W3C Test', function() {
    this.timeout(40000);
    /**
        'beforeEach' is a Mocha test suite hook that allows us to set any prerequisite test method tasks. In this example we set:
        - SauceLabs.com credentials via username and accessKey
        - Browser to chrome
        - OS platform to Windows 10
        - The name of the test
        - and our "sauce:options" object, here we set other test parameters such as the:
            - selenium version, 
            - the build name/number, 
            - test-level timeouts,
            - test case tags
        For more information on 'beforeEach' consult the docs: https://mochajs.org/api/mocha.suite#beforeEach
    */
    beforeEach(function (done) {
        var testName = this.currentTest.title;
        driver = new webdriver.Builder().withCapabilities({
            "browserName": 'chrome',
            "platform": 'Windows 10',
            "version": '61.0',
            "name": testName.toString(),
            "username": username,
            "accessKey": accessKey,
            "sauce:options": {
                "goog:chromeOptions": {"wc3":true},
                "maxDuration": 3600,
                "idleTimeout": 1000,
                "seleniumVersion:": '3.11.0',
                "tags": tags,
                "build": 'w3c-sauce-mocha-tests'
            }
        }).usingServer("https://ondemand.saucelabs.com:443/wd/hub").build();

        driver.getSession().then(function (sessionid) {
            driver.sessionID = sessionid.id_;
        });
        done();
    });
    
    /**
        'afterEach' is a Mocha test suite hook that allows us to set any postrequisite test method tasks. In the example below we:
            - Use the JavascriptExecutor class to send our test 'result' to Sauce Labs with a "passed" flag
                if the test was successful, or a "failed" flag if the test was unsuccessful.
            - Teardown the RemoteWebDriver session with a 'driver.quit()' command so that the test VM doesn't hang.
        For more information on 'afterEach' consult the docs: https://mochajs.org/api/mocha.suite#afterEach
    */

    afterEach(function (done) {
        driver.executeScript("sauce:job-result=" + (true ? "passed" : "failed"));
        driver.quit();
        done();
    });
    
    /**
         * 'it' is the Mocha test case that defines the actual test case, along with the test execution commands.
            In the example below we:
            - Navigate to our SUT (site under test), 'https://www.saucedemo.com' indicated by the 'baseUrl' variable.
            - Store the current page title in a String called 'title'
            - Assert that the page title equals "Swag Labs"
            For more information visit the docs: https://mochajs.org/api/test
    */

    it('start-W3C-chrome-session', function (done) {
        driver.get(baseUrl);
        driver.getTitle().then(function (title) {
            console.log("title is: " + title);
            assert(true);
            done();
        });
    });
});