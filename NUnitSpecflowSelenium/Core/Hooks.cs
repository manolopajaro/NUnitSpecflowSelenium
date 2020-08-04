using AventStack.ExtentReports.Reporter;
using BoDi;
using NUnitSpecflowSelenium.Test.Configuration;
using NUnitSpecflowSelenium.Test.Configuration.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using TechTalk.SpecFlow;

namespace NUnitSpecflowSelenium.Test.Core
{
    [Binding]
    public class Hooks
    {
        #region Driver Init & Dispose
        [BeforeScenario]
        public void InitUIDriver(TestContext testContext, ScenarioContext scenarioContext)
        {
            if (scenarioContext.ScenarioInfo.Tags.Contains("UI"))
            {
                testContext.InitUIDriver();
            }
        }

        [AfterScenario(Order = 9999)]
        public void DisposeUIDriver(TestContext testContext, ScenarioContext scenarioContext)
        {
            if (scenarioContext.ScenarioInfo.Tags.Contains("UI"))
            {
                testContext.GetUIDriver().Dispose();
                testContext.Clear();
            }

        }
        #endregion

        #region Report generation

        [BeforeTestRun]
        public static void RegisterExtentReport(IObjectContainer objectContainer, TestContext testContext)
        {
            var report = new ExtentHtmlReporter(testContext.UniqueOutputDirectory);
            var extent = new AventStack.ExtentReports.ExtentReports();
            extent.AttachReporter(report);

            objectContainer.RegisterInstanceAs(extent);
        }

        [BeforeFeature]
        public static void SetUpFeature(FeatureContext featureContext, TestContext testContext)
        {
            var featureTitle = featureContext.FeatureInfo.Title;
            var reportPath = featureTitle.Length > 30 ? featureTitle.Substring(0, 30) : featureTitle;
            FeatureDto featureDto = new FeatureDto
            {
                Title = featureTitle,
                Description = featureContext.FeatureInfo.Description,
                Tags = featureContext.FeatureInfo.Tags,
                ReportPath = testContext.OutputFilePath($"{reportPath}.html"),
                ReportDirectory = testContext.UniqueOutputDirectory,
                Scenarios = new Dictionary<string, ScenarioDto>()
            };
            featureContext.Set(featureDto);
        }

        [BeforeScenario]
        public void RecordScenario(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            if (featureContext.TryGetValue(out FeatureDto featureDto))
            {
                ScenarioDto scenario = new ScenarioDto
                {
                    Title = GetScenarioTitle(scenarioContext),
                    Steps = new List<StepDto>(),
                    Tags = scenarioContext.ScenarioInfo.Tags
                };
                featureDto.Scenarios.Add(scenario.Title, scenario);
            }
        }

        public static string GetScenarioTitle(ScenarioContext scenarioContext)
        {
            StringBuilder scenarioTitle = new StringBuilder();
            scenarioTitle.Append(scenarioContext.ScenarioInfo.Title);
            var arguments = scenarioContext.ScenarioInfo.Arguments.GetEnumerator();
            while (arguments.MoveNext())
            {
                scenarioTitle.Append($" ({arguments.Key}: {arguments.Value})");
            }
            return scenarioTitle.ToString();
        }

        [BeforeStep]
        public void RecordStep(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            var stepContext = scenarioContext.StepContext;
            if (featureContext.TryGetValue(out FeatureDto featureDto) && featureDto.Scenarios.TryGetValue(GetScenarioTitle(scenarioContext), out ScenarioDto scenario))
            {
                StepDto step = new StepDto()
                {
                    Title = $"{stepContext.StepInfo.StepDefinitionType} {stepContext.StepInfo.Text}"
                };
                scenario.Steps.Add(step);
            }
        }

        [AfterScenario]
        public void AttachStepScreenshot(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            if (featureContext.TryGetValue("ScenarioScreenshots", out Dictionary<string, string> screenshots) && (featureContext.TryGetValue(out FeatureDto featureDto) && featureDto.Scenarios.TryGetValue(GetScenarioTitle(scenarioContext), out ScenarioDto scenario)))
            {
                scenario.Screenshots = screenshots;
            }
        }

        [AfterScenario]
        public void RecordExecutionStatus(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            if (featureContext.TryGetValue(out FeatureDto featureDto) && featureDto.Scenarios.TryGetValue(GetScenarioTitle(scenarioContext), out ScenarioDto scenario))
            {
                scenario.ExecutionStatus = scenarioContext.ScenarioExecutionStatus.ToString();
                scenario.TestError = scenarioContext.TestError;
            }
        }

        [AfterFeature]
        public static void PublishReport(FeatureContext featureContext, ReportGenerator reportGenerator)
        {
            if (featureContext.TryGetValue(out FeatureDto featureDto))
            {
                File.WriteAllText(featureDto.ReportPath, reportGenerator.BuildScenarioReport(featureDto));
                var fullReportPath = Path.Combine(featureDto.ReportDirectory, "index.html");
                if (File.Exists(fullReportPath))
                {
                    File.WriteAllText(fullReportPath, reportGenerator.AddScenarioToFullReport(File.ReadAllText(fullReportPath), featureDto));
                }
                else
                {
                    File.WriteAllText(fullReportPath, reportGenerator.BuildFullReport(featureDto));
                }
            }
        }

        [AfterTestRun]
        public static void FlushExtentReport(AventStack.ExtentReports.ExtentReports extent)
        {
            extent.Flush();
        }
        #endregion

        [AfterScenario(Order = 0), Scope(Tag = "UI")]
        public void TakeScreenshotAfterExecution(TestContext testContext, ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            var screenshotConfiguration = testContext.ConfigurationContext.ScreenshotConfiguration;
            if (screenshotConfiguration != ScreenshotConfiguration.NONE)
            {
                if (scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError && (screenshotConfiguration == ScreenshotConfiguration.FAIL || screenshotConfiguration == ScreenshotConfiguration.ALL))
                {
                    testContext.TakeScreenshotToContext(featureContext, null, "ERROR");
                }
                else if (scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.OK && (screenshotConfiguration == ScreenshotConfiguration.PASS || screenshotConfiguration == ScreenshotConfiguration.ALL))
                {
                    testContext.TakeScreenshotToContext(featureContext, null, "PASS");
                }
            }
        }
    }
}
