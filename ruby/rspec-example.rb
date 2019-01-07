require "selenium-webdriver"
require "rspec"
require "sauce_whisk"

describe "Instant_RSpec_Test_" do
  before(:each) do |test|
    caps = {
        browser_name: 'chrome',
        platform_version: 'Windows 10',
        browser_version: '61.0',
        "goog:chromeOptions": {w3c: true},
        "sauce:options" => {
            name: test.full_description,
            seleniumVersion: '3.11.0',
            username: ENV['SAUCE_USERNAME'],
            accessKey: ENV['SAUCE_ACCESS_KEY']
        }
    }
    @driver = Selenium::WebDriver.for(:remote,
                                     url: 'https://ondemand.saucelabs.com:443/wd/hub',
                                     desired_capabilities: caps)
  end
  it "should_open_chrome" do
    @driver.get('https://www.saucedemo.com')
    puts "title of webpage is: #{@driver.title}"
  end
  after(:each) do |example|
    SauceWhisk::Jobs.change_status(@driver.session_id, !example.exception)
    @driver.quit
  end
end