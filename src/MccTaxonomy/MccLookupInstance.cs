namespace MccTaxonomy;

/// <summary>
/// Array-backed implementation of <see cref="IMccLookup"/>.
/// Indexed by the numeric MCC value for O(1), allocation-free lookup.
/// </summary>
internal sealed class MccLookupInstance : IMccLookup
{
    private const int TableSize = 10_000;

    private readonly MccCategory[] _codes;
    private readonly int _count;

    internal MccLookupInstance(MccCategory[] codes)
    {
        _codes = codes;
        _count = ComputeCount(codes);
    }

    /// <inheritdoc/>
    public MccCategory Categorize(int mccCode)
    {
        if ((uint)mccCode >= TableSize)
            return MccCategory.Uncategorized;

        return _codes[mccCode];
    }

    /// <inheritdoc/>
    public MccCategory Categorize(string mccCode)
    {
        if (mccCode is null) throw new ArgumentNullException(nameof(mccCode));

        return int.TryParse(mccCode, out var code)
            ? Categorize(code)
            : MccCategory.Uncategorized;
    }

    /// <inheritdoc/>
    public bool TryGetCategory(int mccCode, out MccCategory category)
    {
        if ((uint)mccCode < TableSize && _codes[mccCode] != MccCategory.Uncategorized)
        {
            category = _codes[mccCode];
            return true;
        }

        category = MccCategory.Uncategorized;
        return false;
    }

    /// <inheritdoc/>
    public bool TryGetCategory(string mccCode, out MccCategory category)
    {
        if (mccCode is null) throw new ArgumentNullException(nameof(mccCode));

        if (!int.TryParse(mccCode, out var code))
        {
            category = MccCategory.Uncategorized;
            return false;
        }

        return TryGetCategory(code, out category);
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetCodes(MccCategory category)
    {
        for (var i = 0; i < TableSize; i++)
        {
            if (_codes[i] == category)
                yield return i.ToString("D4");
        }
    }

    /// <inheritdoc/>
    public IEnumerable<int> GetCodeValues(MccCategory category)
    {
        for (var i = 0; i < TableSize; i++)
        {
            if (_codes[i] == category)
                yield return i;
        }
    }

    /// <inheritdoc/>
    public int Count => _count;

    /// <inheritdoc/>
    public IMccLookup WithCustomCodes(IReadOnlyDictionary<int, MccCategory> overrides)
    {
        if (overrides is null) throw new ArgumentNullException(nameof(overrides));

        var newTable = (MccCategory[])_codes.Clone();

        foreach (var kvp in overrides)
        {
            if ((uint)kvp.Key < TableSize)
                newTable[kvp.Key] = kvp.Value;
        }

        return new MccLookupInstance(newTable);
    }

    private static int ComputeCount(MccCategory[] codes)
    {
        var count = 0;
        for (var i = 0; i < codes.Length; i++)
        {
            if (codes[i] != MccCategory.Uncategorized)
                count++;
        }

        return count;
    }
}
