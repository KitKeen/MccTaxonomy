namespace MccTaxonomy;

/// <summary>
/// Array-backed implementation of <see cref="IMccLookup"/>.
/// Indexed by the numeric MCC value for O(1), allocation-free lookup.
/// Presence of a code is tracked via a parallel <see cref="bool"/> array so that
/// the "not found" state does not depend on any specific enum value.
/// </summary>
internal sealed class MccLookupInstance : IMccLookup
{
    internal const int TableSize = 10_000;

    private readonly MccCategory[] _codes;
    private readonly bool[] _occupied;
    private readonly int _count;

    // Pre-built per-category index so GetCodes / GetCodeValues run in O(N)
    // over the codes of the requested category instead of O(TableSize).
    private readonly Dictionary<MccCategory, int[]> _codesByCategory;

    internal MccLookupInstance(MccCategory[] codes, bool[] occupied)
    {
        if (codes is null) throw new ArgumentNullException(nameof(codes));
        if (occupied is null) throw new ArgumentNullException(nameof(occupied));
        if (codes.Length != TableSize || occupied.Length != TableSize)
            throw new ArgumentException($"Both arrays must have length {TableSize}.");

        _codes = codes;
        _occupied = occupied;
        _count = ComputeCount(occupied);
        _codesByCategory = BuildByCategoryIndex(codes, occupied);
    }

    /// <inheritdoc/>
    public MccCategory Categorize(int mccCode)
    {
        if ((uint)mccCode >= TableSize || !_occupied[mccCode])
            return MccCategory.Uncategorized;

        return _codes[mccCode];
    }

    /// <inheritdoc/>
    public MccCategory Categorize(string? mccCode)
    {
        if (mccCode is null)
            return MccCategory.Uncategorized;

        return Categorize(mccCode.AsSpan());
    }

    /// <inheritdoc/>
    public MccCategory Categorize(ReadOnlySpan<char> mccCode)
    {
        return TryParseMcc(mccCode, out var code)
            ? Categorize(code)
            : MccCategory.Uncategorized;
    }

    /// <inheritdoc/>
    public bool TryGetCategory(int mccCode, out MccCategory category)
    {
        if ((uint)mccCode < TableSize && _occupied[mccCode])
        {
            category = _codes[mccCode];
            return true;
        }

        category = MccCategory.Uncategorized;
        return false;
    }

    /// <inheritdoc/>
    public bool TryGetCategory(string? mccCode, out MccCategory category)
    {
        if (mccCode is null)
        {
            category = MccCategory.Uncategorized;
            return false;
        }

        return TryGetCategory(mccCode.AsSpan(), out category);
    }

    /// <inheritdoc/>
    public bool TryGetCategory(ReadOnlySpan<char> mccCode, out MccCategory category)
    {
        if (!TryParseMcc(mccCode, out var code))
        {
            category = MccCategory.Uncategorized;
            return false;
        }

        return TryGetCategory(code, out category);
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetCodes(MccCategory category)
    {
        if (!_codesByCategory.TryGetValue(category, out var codes))
            return Array.Empty<string>();

        return FormatCodes(codes);
    }

    /// <inheritdoc/>
    public IEnumerable<int> GetCodeValues(MccCategory category)
    {
        return _codesByCategory.TryGetValue(category, out var codes)
            ? codes
            : Array.Empty<int>();
    }

    /// <inheritdoc/>
    public int Count => _count;

    /// <inheritdoc/>
    public IMccLookup WithCustomCodes(IReadOnlyDictionary<int, MccCategory> overrides)
    {
        if (overrides is null) throw new ArgumentNullException(nameof(overrides));

        var newCodes = (MccCategory[])_codes.Clone();
        var newOccupied = (bool[])_occupied.Clone();

        foreach (var kvp in overrides)
        {
            if ((uint)kvp.Key >= TableSize)
                throw new ArgumentOutOfRangeException(
                    nameof(overrides),
                    $"MCC code {kvp.Key} is outside the valid range [0, {TableSize - 1}].");

            if (kvp.Value == MccCategory.Uncategorized)
            {
                newCodes[kvp.Key] = MccCategory.Uncategorized;
                newOccupied[kvp.Key] = false;
            }
            else
            {
                newCodes[kvp.Key] = kvp.Value;
                newOccupied[kvp.Key] = true;
            }
        }

        return new MccLookupInstance(newCodes, newOccupied);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    /// <summary>
    /// Strict MCC parser: accepts 1-4 ASCII digits only. No signs, no whitespace,
    /// no locale-specific separators. Significantly faster than <c>int.TryParse</c>
    /// on the hot path and available on every target framework (including
    /// netstandard2.0, which does not expose <c>int.TryParse(ReadOnlySpan&lt;char&gt;)</c>).
    /// </summary>
    private static bool TryParseMcc(ReadOnlySpan<char> s, out int value)
    {
        value = 0;
        if (s.Length == 0 || s.Length > 4)
            return false;

        var result = 0;
        for (var i = 0; i < s.Length; i++)
        {
            var c = s[i];
            if (c < '0' || c > '9')
                return false;
            result = result * 10 + (c - '0');
        }

        value = result;
        return true;
    }

    private static int ComputeCount(bool[] occupied)
    {
        var count = 0;
        for (var i = 0; i < occupied.Length; i++)
            if (occupied[i]) count++;
        return count;
    }

    private static Dictionary<MccCategory, int[]> BuildByCategoryIndex(
        MccCategory[] codes, bool[] occupied)
    {
        var buckets = new Dictionary<MccCategory, List<int>>();
        for (var i = 0; i < codes.Length; i++)
        {
            if (!occupied[i]) continue;

            var cat = codes[i];
            if (!buckets.TryGetValue(cat, out var list))
            {
                list = new List<int>();
                buckets[cat] = list;
            }
            list.Add(i);
        }

        var result = new Dictionary<MccCategory, int[]>(buckets.Count);
        foreach (var kvp in buckets)
            result[kvp.Key] = kvp.Value.ToArray();
        return result;
    }

    private static IEnumerable<string> FormatCodes(int[] codes)
    {
        for (var i = 0; i < codes.Length; i++)
            yield return codes[i].ToString("D4");
    }
}
