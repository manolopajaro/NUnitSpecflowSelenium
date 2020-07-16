using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitSpecflowSelenium.Test.Configuration.DTO
{
    public class FeatureDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
        public string ReportPath { get; set; }
        public string ReportDirectory { get; set; }
        public Dictionary<string, ScenarioDto> Scenarios { get; set; }

    }
}
