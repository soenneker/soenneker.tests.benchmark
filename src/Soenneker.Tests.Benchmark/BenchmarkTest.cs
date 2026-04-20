using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;

namespace Soenneker.Tests.Benchmark;

/// <summary>
/// An abstract class for benchmarking tests in .NET, integrating BenchmarkDotNet with Xunit's output helper and providing a method to log benchmark summaries asynchronously.
/// </summary>
public abstract class BenchmarkTest
{
    protected ManualConfig DefaultConf { get; }

    protected BenchmarkTest()
    {
        DefaultConf = ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.DisableOptimizationsValidator);
        DefaultConf.SummaryStyle = SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend);
    }
}