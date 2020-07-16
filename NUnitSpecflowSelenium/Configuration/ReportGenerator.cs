using NUnitSpecflowSelenium.Test.Configuration.DTO;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace NUnitSpecflowSelenium.Test.Configuration
{
    public class ReportGenerator
    {
        public string BuildScenarioReport(FeatureDto featureDto)
        {
            StringBuilder sb = new StringBuilder("<html>");
            sb.Append("<body>");
            sb.Append($"<h2>Feature: {featureDto.Title}<h2>");
            sb.Append($"<p>{featureDto.Description}</p>");
            sb.Append($"<h3>Tags:</h3>");
            sb.Append($"<ol>");
            foreach (string tag in featureDto.Tags)
            {
                sb.Append($"<li>{tag}<li>");
            }
            sb.Append("</ol>");
            sb.Append($"<table>");
            sb.Append($"<tbody>");
            foreach (ScenarioDto scenario in featureDto.Scenarios.Values)
            {
                sb.Append($"<tr>");
                sb.Append(
                    $"<td style='border:1px solid black;'>" +
                        $"<h1><strong>{scenario.Title}</strong></h1>" +
                        $"<table>");
                foreach (StepDto step in scenario.Steps)
                {
                    sb.Append($"<tr><td>{step.Title}</td></tr>");
                }
                sb.Append($"</table>");
                if (scenario.ExecutionStatus == "TestError")
                {
                    sb.Append($"<strong style='color: red;'>Execution Status: {scenario.ExecutionStatus}</strong></br>");
                    sb.Append($"<strong style='color: red;'>Error: {scenario.TestError.Message}</strong></br>");
                }
                else
                {
                    sb.Append($"<strong>Execution Status: {scenario.ExecutionStatus}</strong></br>");
                }
                if (scenario.Screenshots != null)
                {
                    sb.Append($"<p><h1>Screenshots</h1></p>" +
                    $"<p>");
                    foreach (KeyValuePair<string, string> entry in scenario.Screenshots)
                    {
                        sb.Append($"{entry.Key}<img src='{entry.Value}'>");
                    }
                    sb.Append($"</p>");
                }
                sb.Append($"</td>");
                sb.Append($"</tr>");

            }
            sb.Append($"</tbody>");
            sb.Append($"</table>");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        public string BuildFullReport(FeatureDto featureDto)
        {
            StringBuilder sb = new StringBuilder("<html>");
            sb.Append(GetScenarioInfo(featureDto));
            sb.Append("</html>");
            return sb.ToString();
        }

        public string AddScenarioToFullReport(string fullReport, FeatureDto featureDto)
        {
            StringBuilder sb = new StringBuilder(fullReport[0..^7]);
            sb.Append(GetScenarioInfo(featureDto));
            sb.Append("</html>");
            return sb.ToString();
        }

        private string GetScenarioInfo(FeatureDto featureDto)
        {
            StringBuilder sb = new StringBuilder();
            var passed = 0;
            var failed = 0;
            foreach (ScenarioDto scenarios in featureDto.Scenarios.Values)
            {
                if (scenarios.ExecutionStatus == ScenarioExecutionStatus.TestError.ToString())
                {
                    failed++;
                }
                else if (scenarios.ExecutionStatus == ScenarioExecutionStatus.OK.ToString())
                {
                    passed++;
                }
            }
            sb.Append($"<p><a href='{featureDto.ReportPath}'>{featureDto.Title}</a>&nbsp; Results: {featureDto.Scenarios.Count} Executed. {passed} Passed. {failed} Failed.</p></br>");
            return sb.ToString();
        }
    }
}
