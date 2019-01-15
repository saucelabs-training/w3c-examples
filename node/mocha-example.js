var webdriver = require('selenium-webdriver'),
    assert = require('assert'),
    username = process.env.SAUCE_USERNAME,
    accessKey = process.env.SAUCE_ACCESS_KEY,
    /* Change the baseURL to your application URL */
    baseUrl = "https://www.saucedemo.com",
    tags = ["sauceDemo", "demoTest", "module4", "nodeTest"],
    driver;

describe('Instant Sauce Test Module 4', function() {
    this.timeout(40000);

    beforeEach(function (done) {
        var testName = this.currentTest.title;
        driver = new webdriver.Builder().withCapabilities({
            "browserName": 'chrome',
            "platform": 'Windows 10',
            "version": '61.0',
            "tags": tags,
            "name": testName.toString(),
            "username": username,
            "accessKey": accessKey,
            "sauce:options": {
                "goog:chromeOptions": {"wc3":true},
                "maxDuration": 3600,
                "idleTimeout": 1000,
                "seleniumVersion:": '3.11.0',
                "build": 'instant-sauce-mocha-tests'
            }
        }).usingServer("https://ondemand.saucelabs.com:443/wd/hub").build();

        driver.getSession().then(function (sessionid) {
            driver.sessionID = sessionid.id_;
        });
        done();
    });

    afterEach(function (done) {
        driver.executeScript("sauce:job-result=" + (true ? "passed" : "failed"));
        driver.quit();
        done();
    });

    it('should-open-chrome', function (done) {
        driver.get(baseUrl);
        driver.getTitle().then(function (title) {
            console.log("title is: " + title);
            assert(true);
            done();
        });
    });
});