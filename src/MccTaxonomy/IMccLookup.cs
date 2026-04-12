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
    /// Returns the <see cref="MccCategory"/> for a given 4-digit MCC string.
    /// Returns <see cref="MccCategory.Uncategorized"/> if the code is not recognized or not a valid integer.
    /// </summary>
    MccCategory Categorize(string mccCode);

    /// <summary>
    /// Tries to get the category for a given numeric MCC code.
    /// Returns <c>false</c> if the code is not in the taxonomy.
    /// </summary>
    bool TryGetCategory(int mccCode, out MccCategory category);

    /// <summary>
    /// Tries to get the category for a given 4-digit MCC string.
    /// Returns <c>false</c> if the code is not in the taxonomy or not a valid integer.
    /// </summary>
    bool TryGetCategory(string mccCode, out MccCategory category);

    /// <summary>
    /// Returns all MCC codes (zero-padded strings) that belong to the given category.
    /// </summary>
    IEnumerable<string> GetCodes(MccCategory category);

    /// <summary>
    /// Returns all MCC codes (as integers) that belong to the given category.
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
    /// </param>
    IMccLookup WithCustomCodes(IReadOnlyDictionary<int, MccCategory> overrides);
}
