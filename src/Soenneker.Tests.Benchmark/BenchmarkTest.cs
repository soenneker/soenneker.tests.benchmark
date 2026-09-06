using System;
using System.Globalization;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
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

    /// <summary>
    /// Creates a configuration that caps the processor count reported by .NET in benchmark child processes.
    /// </summary>
    /// <param name="maxProcessorCount">The maximum logical processor count, for example 1 for low-level measurements.</param>
    /// <remarks>
    /// Applies to out-of-process .NET benchmarks. This limits processor-count-based runtime sizing and benchmark
    /// workloads; it does not set CPU affinity or limit explicitly created threads. The host process is unaffected.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The maximum processor count is less than one.</exception>
    protected BenchmarkTest(int maxProcessorCount) : this()
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxProcessorCount, 1);

        int processorCount = Math.Min(maxProcessorCount, Environment.ProcessorCount);
        DefaultConf.AddJob(Job.Default
            .WithEnvironmentVariable("DOTNET_PROCESSOR_COUNT", processorCount.ToString(CultureInfo.InvariantCulture))
            .AsMutator());
    }
}
