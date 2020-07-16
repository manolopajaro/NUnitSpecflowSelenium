using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitSpecflowSelenium.Test.Configuration.DTO
{
    public class ScenarioDto
    {
        public string Title { get; set; }
        public List<StepDto> Steps { get; set; }
        public string ExecutionStatus { get; set; }
        public Exception TestError { get; set; }
        public string[] Tags { get; set; }
        public Dictionary<string, string> Screenshots { get; set; }

    }
}
