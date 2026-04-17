using BenchmarkDotNet.Attributes;

namespace MccTaxonomy.Benchmarks;

/// <summary>
/// Compares the array-backed <see cref="MccLookup"/> against a hash-based baseline
/// (<see cref="Dictionary{TKey,TValue}"/>) on the three call shapes exposed by the API:
/// <c>int</c>, <c>string</c> and <c>ReadOnlySpan&lt;char&gt;</c>.
/// </summary>
[MemoryDiagnoser]
[Config(typeof(BenchmarkConfig))]
public class LookupBenchmarks
{
    private const int Iterations = 128;

    // Representative mix: well-known codes across several categories + one misses.
    private static readonly int[] IntCodes =
    {
        5411, 5812, 7311, 4511, 3100, 5541, 7512, 8398, 5211, 5964,
        8211, 5732, 7832, 6010, 7800, 9211, 5912, 7011, 6300, 5816,
        5311, 7210, 5943, 4121, 4722, 4900, 9999, 1234, 4444, 8888,
    };

    private static readonly string[] StringCodes =
        IntCodes.Select(c => c.ToString("D4")).ToArray();

    private Dictionary<int, MccCategory> _dictBaseline = null!;

    [GlobalSetup]
    public void Setup()
    {
        _dictBaseline = new Dictionary<int, MccCategory>();
        foreach (MccCategory cat in Enum.GetValues(typeof(MccCategory)))
        {
            if (cat == MccCategory.Uncategorized) continue;
            foreach (var code in MccLookup.GetCodeValues(cat))
                _dictBaseline[code] = cat;
        }
    }

    [Benchmark(Baseline = true, Description = "Dictionary<int,MccCategory>.TryGetValue")]
    public int DictionaryBaseline()
    {
        var sum = 0;
        for (var i = 0; i < Iterations; i++)
        {
            foreach (var code in IntCodes)
            {
                _dictBaseline.TryGetValue(code, out var cat);
                sum += (int)cat;
            }
        }
        return sum;
    }

    [Benchmark(Description = "MccLookup.Categorize(int)")]
    public int ArrayInt()
    {
        var sum = 0;
        for (var i = 0; i < Iterations; i++)
        {
            foreach (var code in IntCodes)
                sum += (int)MccLookup.Categorize(code);
        }
        return sum;
    }

    [Benchmark(Description = "MccLookup.Categorize(string)")]
    public int ArrayString()
    {
        var sum = 0;
        for (var i = 0; i < Iterations; i++)
        {
            foreach (var code in StringCodes)
                sum += (int)MccLookup.Categorize(code);
        }
        return sum;
    }

    [Benchmark(Description = "MccLookup.Categorize(ReadOnlySpan<char>)")]
    public int ArraySpan()
    {
        var sum = 0;
        for (var i = 0; i < Iterations; i++)
        {
            foreach (var code in StringCodes)
                sum += (int)MccLookup.Categorize(code.AsSpan());
        }
        return sum;
    }
}
