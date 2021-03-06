﻿using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SauceLabsSpecflowDemo
{
    [Binding]
    public sealed class Hooks1
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, Environment.GetEnvironmentVariable("browserName"));
            capabilities.SetCapability(CapabilityType.Version, Environment.GetEnvironmentVariable("version"));
            capabilities.SetCapability(CapabilityType.Platform, Environment.GetEnvironmentVariable("platformOs"));
            capabilities.SetCapability("username", Environment.GetEnvironmentVariable("SAUCE_USERNAME"));
            capabilities.SetCapability("accessKey", Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY"));
            capabilities.SetCapability("name", ScenarioContext.Current.ScenarioInfo.Title);

            RemoteWebDriver webDriver = new RemoteWebDriver(new Uri("https://ondemand.saucelabs.com:443/wd/hub"), capabilities, TimeSpan.FromSeconds(600));
            webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
            ScenarioContext.Current["driver"] = webDriver;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            IWebDriver driver = (IWebDriver)ScenarioContext.Current["driver"];
            bool passed = ScenarioContext.Current.TestError == null;
            ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            driver.Quit();
        }
    }
}

