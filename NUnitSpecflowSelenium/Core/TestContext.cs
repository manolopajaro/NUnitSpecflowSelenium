using NUnitSpecflowSelenium.Test.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow;

namespace NUnitSpecflowSelenium.Test.Core
{
    public class TestContext : SpecFlowContext
    {
        public MyBussinessEnvironment Environment;
        public string UniqueId => DateTime.Now.ToString("yyyy-MM-ddTHHmmss", CultureInfo.InvariantCulture);
        private readonly string UniqueFolderId = DateTime.Now.ToString("yyyy-MM-ddTHHmmss", CultureInfo.InvariantCulture);
        public ConfigurationContext ConfigurationContext { get; private set; }

        public TestContext(ConfigurationContext configurationContext)
        {
            ConfigurationContext = configurationContext ?? throw new ArgumentNullException("ConfigurationContext not present");
            InitEnvironment(configurationContext);
        }

        /// <summary> 
        /// This method initializes the WebDriver, opens the browser and goes to the URL specified in appsettings.json
        /// </summary>
        /// <remarks>
        /// This method is to be called in a BeforeScenario Hook in order to have the Webdriver instance ready to use.
        /// </remarks>
        public void InitUIDriver()
        {
            if (!TryGetValue(out IWebDriver _))
            {
                Set(new DriverFactory().GetDriver(ConfigurationContext, Environment));
                Set(new WebDriverWait(GetUIDriver(), TimeSpan.FromSeconds(Environment.ExplicitTimeout)));

                GetUIDriver().Navigate().GoToUrl(Environment.Url);
            }

        }

        /// <summary>
        /// Retrieves the current instance of WebDriver
        /// </summary>
        public IWebDriver GetUIDriver()
        {
            TryGetValue(out IWebDriver driver);
            return driver;

        }

        /// <summary>
        /// Utility method to perform waits whenever the current thread needs it
        /// </summary>
        public WebDriverWait Wait()
        {
            return Get<WebDriverWait>();
        }

        private string TakeScreenshot(string title, string prefix = null)
        {
            string screenshotFilePath = "";
            if (GetUIDriver() is ITakesScreenshot takesScreenshot)
            {
                screenshotFilePath = OutputFilePath(string.Format("{0}{1}_{2}.png",
                  prefix ?? "",
                  title,
                  DateTime.Now.ToString("s")));

                var screenshot = takesScreenshot.GetScreenshot();
                screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);
            }
            return screenshotFilePath;
        }

        public void TakeScreenshotToContext(FeatureContext FeatureContext, string fileNameKey = null, string prefix = null)
        {
            var featureTitle = FeatureContext.FeatureInfo.Title;
            featureTitle = featureTitle.Length > 10 ? featureTitle.Substring(0, 10) : featureTitle;
            var screenshotPath = TakeScreenshot(featureTitle, prefix);
            if (!FeatureContext.TryGetValue("ScenarioScreenshots", out Dictionary<string, string> screenshots))
            {
                screenshots = new Dictionary<string, string>();
                FeatureContext.Add("ScenarioScreenshots", screenshots);
            }
            screenshots.Add(fileNameKey ?? screenshotPath.Split("\\")[^1], screenshotPath);
        }

        private void InitEnvironment(ConfigurationContext configurationContext)
        {
            Environment = configurationContext.Environment;
        }

        //TODO review nunit test context TestDirectory https://docs.nunit.org/articles/nunit/writing-tests/TestContext.html
        public string TestDataDirectory
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "TestData");
            }
        }

        public string UniqueOutputDirectory
        {
            get
            {
                var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), UniqueFolderId);
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);
                return outputFolder;
            }
        }

        public string OutputFilePath(string fileName)
        {

            return Path.GetFullPath(Path.Combine(UniqueOutputDirectory, ToPath(fileName)));
        }

        /// <summary>
        /// Makes string path-compatible, ie removes characters not allowed in path and replaces whitespace with '_'
        /// </summary>
        private string ToPath(string s)
        {
            var builder = new StringBuilder(s);
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                builder.Replace(invalidChar.ToString(), "");
            }
            builder.Replace(' ', '_');
            return builder.ToString();
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    var driver = GetUIDriver();
                    if (driver != null)
                    {
                        driver.Dispose();
                    }
                }
                base.Dispose();
                disposedValue = true;
            }
        }

        protected override void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
