using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace MccTaxonomy.Benchmarks;

/// <summary>
/// Shared config: short run (enough for a hot-path micro-benchmark), server GC,
/// and the in-process toolchain so BenchmarkDotNet does not generate a
/// separate runner project (which would otherwise try to restore every
/// TargetFramework of the referenced library, including TFMs the host SDK
/// may not support).
/// </summary>
public sealed class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(Job.ShortRun
            .WithToolchain(InProcessEmitToolchain.Instance)
            .WithGcServer(true)
            .WithGcConcurrent(true));
    }
}
