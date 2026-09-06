[![](https://img.shields.io/nuget/v/soenneker.tests.benchmark.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.tests.benchmark/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.tests.benchmark/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.tests.benchmark/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.tests.benchmark.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.tests.benchmark/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.tests.benchmark/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.tests.benchmark/actions/workflows/codeql.yml)

# Soenneker.Tests.Benchmark

A small base class that supplies a consistent BenchmarkDotNet `ManualConfig` for benchmark test classes.

## Installation

```bash
dotnet add package Soenneker.Tests.Benchmark
```

## Usage

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Soenneker.Tests.Benchmark;

[MemoryDiagnoser]
public sealed class ParsingBenchmarks : BenchmarkTest
{
    private const string Value = "123456";

    [Benchmark(Baseline = true)]
    public int Parse() => int.Parse(Value);

    [Benchmark]
    public bool TryParse() => int.TryParse(Value, out _);

    public void Run()
    {
        BenchmarkRunner.Run<ParsingBenchmarks>(DefaultConf);
    }
}
```

`DefaultConf` starts from BenchmarkDotNet's default configuration, disables the optimizations validator, and displays baseline ratios using trend wording. It is protected so a derived benchmark decides how and when to invoke `BenchmarkRunner`.

The parameterless constructor does not choose jobs, runtimes, warmup counts, diagnosers, or exporters. Add those through BenchmarkDotNet attributes or by extending `DefaultConf` in the derived class.

### Low-level measurements on machines with many cores

Use the processor cap when runtime sizing or workloads based on `Environment.ProcessorCount` make low-level benchmarks unnecessarily expensive:

```csharp
public sealed class LowLevelBenchmarks : BenchmarkTest
{
    public LowLevelBenchmarks() : base(maxProcessorCount: 1)
    {
    }

    // Benchmark methods and runner as above.
}
```

The cap sets `DOTNET_PROCESSOR_COUNT` in benchmark child processes to the smaller of the requested maximum and the host's available processor count. It uses a BenchmarkDotNet job mutator so it also applies to jobs supplied through attributes without adding another benchmark run. Values less than one are rejected.

This requires out-of-process .NET benchmarks. It controls the processor count seen by .NET, not CPU affinity or explicitly created threads, and does not change the test host's environment. It does not impose a time limit or reduce warmup/measurement iterations. Keep the parameterless constructor for benchmarks intended to measure full-machine parallelism.

Disabling the optimizations validator allows benchmarks to run from configurations BenchmarkDotNet would normally reject, but unoptimized builds can produce misleading results. Run performance measurements in Release unless the benchmark intentionally measures another configuration.
