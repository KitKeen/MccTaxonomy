using BenchmarkDotNet.Attributes;

namespace MccTaxonomy.Benchmarks;

/// <summary>
/// Measures the pre-built per-category index (<c>GetCodeValues</c> / <c>GetCodes</c>).
/// Before the refactor these scanned the full 10 000-slot table on every call;
/// afterwards they should be O(N) over the requested category only.
/// </summary>
[MemoryDiagnoser]
[Config(typeof(BenchmarkConfig))]
public class CollectionBenchmarks
{
    [Params(MccCategory.Supermarkets, MccCategory.Retail, MccCategory.Accommodation)]
    public MccCategory Category;

    [Benchmark]
    public int GetCodeValues_Count()
    {
        var count = 0;
        foreach (var _ in MccLookup.GetCodeValues(Category))
            count++;
        return count;
    }

    [Benchmark]
    public int GetCodes_Count()
    {
        var count = 0;
        foreach (var _ in MccLookup.GetCodes(Category))
            count++;
        return count;
    }
}
