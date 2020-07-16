using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
namespace NUnitSpecflowSelenium.Test
{
    public class AssemblyInfo
    {
    }
}
