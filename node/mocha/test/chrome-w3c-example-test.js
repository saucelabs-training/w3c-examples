const promise = require('selenium-webdriver');
let expect = require('chai').expect;
let webdriver = require('selenium-webdriver');
/**
    Here we set our script dependencies (e.g. selenium-webdriver and 'assert'), and we also set some key variables such as:
        - our SauceLabs account credentials
        - the SUT (site under test) url
        - any necessary tags we may use to run test reports or filters
        - the WebDriver session (indicated by 'driver')
*/
let username = process.env.SAUCE_USERNAME,
    accessKey = process.env.SAUCE_ACCESS_KEY,
    /* Change the baseURL to your application URL */
    baseUrl = "https://www.saucedemo.com",
    tags = ["w3c", "demoTest", "w3c-chrome-tests", "nodeTest"],
    driver;

/** the selenium-webdriver will eventually deprecate the promise manager, therefore you must resolve your callback methods by either:
 * - chaining your own promises: https://github.com/SeleniumHQ/selenium/wiki/WebDriverJs#option-1-use-classic-promise-chaining
 * - migrating to generators: https://github.com/SeleniumHQ/selenium/wiki/WebDriverJs#option-2-migrate-to-generators
 * - or migrating to async/await (as shown in this example): https://github.com/SeleniumHQ/selenium/wiki/WebDriverJs#option-3-migrate-to-asyncawait
 */

promise.USE_PROMISE_MANAGER = false;
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
            - sauce labs credentials (username and accessKey)
            - selenium version,
            - the build name/number,
            - test name
            - test-level timeouts,
            - test case tags
        For more information on 'beforeEach' consult the docs: https://mochajs.org/api/mocha.suite#beforeEach
    */
    beforeEach(async function () {
        driver = await new webdriver.Builder().withCapabilities({
            "browserName": 'chrome',
            "platformName": 'Windows 10',
            "browserVersion": 'latest',
            /** Google requires "w3c" to be set in "goog:chromeOptions" as true if you're using ChromeDriver version 74 or lower.
             * Based on this commit: https://chromium.googlesource.com/chromium/src/+/2b49880e2481658e0702fd6fe494859bca52b39c
             * ChromeDriver now uses w3c by default from version 75+ so setting this option will no longer be a requirement **/
            "goog:chromeOptions" : { "w3c" : true },
            "sauce:options": {
                "username": username,
                "accessKey": accessKey,
                "maxDuration": 3600,
                "idleTimeout": 1000,
                "seleniumVersion:": '3.141.59',
                "tags": tags,
                "name": "chrome-w3c-" + this.currentTest.title,
                "build": 'w3c-sauce-mocha-tests'
            }
        }).usingServer("https://ondemand.saucelabs.com/wd/hub").build();

        await driver.getSession().then(function (sessionid) {
            driver.sessionID = sessionid.id_;
        });
    });
    
    /**
        'afterEach' is a Mocha test suite hook that allows us to set any postrequisite test method tasks. In the example below we:
            - Use the JavascriptExecutor class to send our test 'result' to Sauce Labs with a "passed" flag
                if the test was successful, or a "failed" flag if the test was unsuccessful.
            - Teardown the RemoteWebDriver session with a 'driver.quit()' command so that the test VM doesn't hang.
        For more information on 'afterEach' consult the docs: https://mochajs.org/api/mocha.suite#afterEach
    */

    afterEach(async function() {
        await driver.executeScript("sauce:job-result=" + (this.currentTest.state));
        await driver.quit();
    });
    
    /**
         * 'it' is the Mocha test case that defines the actual test case, along with the test execution commands.
            In the example below we:
            - Navigate to our SUT (site under test), 'https://www.saucedemo.com' indicated by the 'baseUrl' variable.
            - Store the current page title in a String called 'title'
            - Assert that the page title equals "Swag Labs"
            For more information visit the docs: https://mochajs.org/api/test
    */

    it('get-title-test', async function() {
        await driver.get(baseUrl);
        const title = await driver.getTitle();
        console.log('Page Title is: ' + title);
        expect(title).equals('Swag Labs');
    });
});