namespace MccTaxonomy;

/// <summary>
/// Defines a lookup service that maps numeric MCC codes to merchant categories.
/// </summary>
public interface IMccLookup
{
    /// <summary>
    /// Returns the <see cref="MccCategory"/> for a given numeric MCC code.
    /// Returns <see cref="MccCategory.Uncategorized"/> if the code is not recognized.
    /// </summary>
    MccCategory Categorize(int mccCode);

    /// <summary>
    /// Returns the <see cref="MccCategory"/> for a given MCC string (1-4 ASCII digits,
    /// leading zeros allowed). Returns <see cref="MccCategory.Uncategorized"/> if the
    /// string is <c>null</c>, empty, contains non-digit characters, is longer than
    /// 4 characters, or does not correspond to any recognized code.
    /// </summary>
    MccCategory Categorize(string? mccCode);

    /// <summary>
    /// Returns the <see cref="MccCategory"/> for a given MCC character span (1-4 ASCII digits,
    /// leading zeros allowed). Returns <see cref="MccCategory.Uncategorized"/> if the
    /// span is empty, contains non-digit characters, is longer than 4 characters,
    /// or does not correspond to any recognized code. This overload is allocation-free.
    /// </summary>
    MccCategory Categorize(ReadOnlySpan<char> mccCode);

    /// <summary>
    /// Tries to get the category for a given numeric MCC code.
    /// Returns <c>false</c> if the code is not in the taxonomy.
    /// </summary>
    bool TryGetCategory(int mccCode, out MccCategory category);

    /// <summary>
    /// Tries to get the category for a given MCC string (1-4 ASCII digits).
    /// Returns <c>false</c> if the string is <c>null</c>, malformed, or not present in the taxonomy.
    /// </summary>
    bool TryGetCategory(string? mccCode, out MccCategory category);

    /// <summary>
    /// Tries to get the category for a given MCC character span (1-4 ASCII digits).
    /// Returns <c>false</c> if the span is malformed or not present in the taxonomy.
    /// This overload is allocation-free.
    /// </summary>
    bool TryGetCategory(ReadOnlySpan<char> mccCode, out MccCategory category);

    /// <summary>
    /// Returns all MCC codes (zero-padded 4-digit strings) that belong to the given category.
    /// Runs in O(N) where N is the number of codes in the category (not the full table).
    /// </summary>
    IEnumerable<string> GetCodes(MccCategory category);

    /// <summary>
    /// Returns all MCC codes (as integers) that belong to the given category.
    /// Runs in O(N) where N is the number of codes in the category (not the full table).
    /// </summary>
    IEnumerable<int> GetCodeValues(MccCategory category);

    /// <summary>
    /// Total number of recognized MCC codes in this lookup.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Returns a new <see cref="IMccLookup"/> with the specified custom code overrides applied.
    /// The current instance is not modified (immutable).
    /// </summary>
    /// <param name="overrides">
    /// Numeric MCC codes to <see cref="MccCategory"/> mappings to add or override.
    /// Setting a code to <see cref="MccCategory.Uncategorized"/> removes it from the taxonomy.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="overrides"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when any key is outside the valid MCC range [0, 9999].
    /// </exception>
    IMccLookup WithCustomCodes(IReadOnlyDictionary<int, MccCategory> overrides);
}
