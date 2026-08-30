using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;

namespace Soenneker.Tests.Benchmark;

/// <summary>
/// Provides a reusable BenchmarkDotNet configuration for benchmark test classes.
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
