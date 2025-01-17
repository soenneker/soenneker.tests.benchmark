using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using Soenneker.Tests.Unit;
using Soenneker.Utils.AutoBogus;
using Xunit;

namespace Soenneker.Tests.Benchmark;

/// <summary>
/// An abstract class for benchmarking tests in .NET, integrating BenchmarkDotNet with Xunit's output helper and providing a method to log benchmark summaries asynchronously.
/// </summary>
public abstract class BenchmarkTest : UnitTest
{
    protected ManualConfig DefaultConf { get; }

    protected ITestOutputHelper OutputHelper { get; set; }

    protected BenchmarkTest(ITestOutputHelper outputHelper, AutoFaker? autoFaker = null) : base(outputHelper, autoFaker)
    {
        OutputHelper = outputHelper;

        DefaultConf = ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.DisableOptimizationsValidator);
        DefaultConf.SummaryStyle = SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend);
    }
}