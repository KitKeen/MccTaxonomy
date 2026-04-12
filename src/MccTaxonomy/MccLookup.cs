namespace MccTaxonomy;

/// <summary>
/// O(1) lookup from a 4-digit MCC code to its merchant category.
/// Thread-safe, allocation-free after initialization.
/// Backed by a fixed-size <see cref="MccCategory"/> array indexed by the numeric MCC code,
/// which is faster than any hash-based structure.
/// </summary>
/// <remarks>
/// This class provides static convenience methods that delegate to a built-in default instance.
/// To override or extend the built-in codes, use <see cref="WithCustomCodes"/>.
/// </remarks>
public static class MccLookup
{
    private static readonly MccLookupInstance _default = new(BuildTable());

    // -------------------------------------------------------------------------
    // Static API (convenience — delegates to the built-in default instance)
    // -------------------------------------------------------------------------

    /// <summary>
    /// Returns the <see cref="MccCategory"/> for a given numeric MCC code.
    /// Returns <see cref="MccCategory.Uncategorized"/> if the code is not recognized.
    /// </summary>
    /// <example>
    /// <code>MccLookup.Categorize(5411) // → MccCategory.Supermarkets</code>
    /// </example>
    public static MccCategory Categorize(int mccCode)
        => _default.Categorize(mccCode);

    /// <summary>
    /// Returns the <see cref="MccCategory"/> for a given 4-digit MCC string code.
    /// Returns <see cref="MccCategory.Uncategorized"/> if the code is not recognized.
    /// Leading zeros are handled correctly: "0742" and 742 resolve to the same entry.
    /// </summary>
    /// <example>
    /// <code>MccLookup.Categorize("5411") // → MccCategory.Supermarkets</code>
    /// </example>
    public static MccCategory Categorize(string mccCode)
        => _default.Categorize(mccCode);

    /// <summary>
    /// Tries to get the category for a given numeric MCC code.
    /// Returns <c>false</c> if the code is not in the taxonomy.
    /// </summary>
    public static bool TryGetCategory(int mccCode, out MccCategory category)
        => _default.TryGetCategory(mccCode, out category);

    /// <summary>
    /// Tries to get the category for a given 4-digit MCC string.
    /// Returns <c>false</c> if the code is not in the taxonomy or not a valid integer.
    /// </summary>
    public static bool TryGetCategory(string mccCode, out MccCategory category)
        => _default.TryGetCategory(mccCode, out category);

    /// <summary>
    /// Returns all MCC codes (as zero-padded strings) that belong to the given category.
    /// </summary>
    public static IEnumerable<string> GetCodes(MccCategory category)
        => _default.GetCodes(category);

    /// <summary>
    /// Returns all MCC codes (as integers) that belong to the given category.
    /// </summary>
    public static IEnumerable<int> GetCodeValues(MccCategory category)
        => _default.GetCodeValues(category);

    /// <summary>
    /// Total number of MCC codes in the built-in taxonomy.
    /// </summary>
    public static int Count => _default.Count;

    /// <summary>
    /// Returns a new <see cref="IMccLookup"/> based on the built-in taxonomy
    /// with the specified custom code overrides applied.
    /// The built-in default is not modified (immutable).
    /// </summary>
    /// <param name="overrides">
    /// Numeric MCC codes mapped to <see cref="MccCategory"/> values.
    /// Unknown codes (not covered by this library) can be added here.
    /// Existing codes can be overridden if the built-in mapping is incorrect.
    /// </param>
    /// <returns>A new <see cref="IMccLookup"/> instance with overrides applied.</returns>
    /// <example>
    /// <code>
    /// IMccLookup lookup = MccLookup.WithCustomCodes(new Dictionary&lt;int, MccCategory&gt;
    /// {
    ///     [9999] = MccCategory.Finance,
    ///     [1234] = MccCategory.Retail,
    /// });
    /// </code>
    /// </example>
    public static IMccLookup WithCustomCodes(IReadOnlyDictionary<int, MccCategory> overrides)
        => _default.WithCustomCodes(overrides);

    // -------------------------------------------------------------------------
    // Table construction
    // -------------------------------------------------------------------------

    private static MccCategory[] BuildTable()
    {
        var t = new MccCategory[10_000];

        // Initialize all slots to Uncategorized
        for (var i = 0; i < t.Length; i++)
            t[i] = MccCategory.Uncategorized;

        Set(t, MccCategory.Marketing,
            7311);

        SetRange(t, MccCategory.Airlines, 3000, 3350);
        Set(t, MccCategory.Airlines,
            4415, 4511);

        Set(t, MccCategory.Automotive,
            4784, 5013, 5172, 5271,
            5511, 5521, 5531, 5532, 5533,
            5541, 5542, 5552, 5561, 5571,
            5592, 5598, 5599, 5935, 5983,
            7523, 7524, 7531, 7534, 7535,
            7538, 7542, 7549, 5299, 9752);

        SetRange(t, MccCategory.VehicleRental, 3351, 3439);
        Set(t, MccCategory.VehicleRental,
            3441, 7512, 7513, 7519);

        Set(t, MccCategory.Charity,
            8398, 8641, 8651, 8661, 8675, 8699, 8734, 8743);

        Set(t, MccCategory.Construction,
            1520, 1711, 1731, 1740, 1750, 1761, 1771, 1799,
            5021, 5039, 5044, 5046, 5051, 5065, 5072, 5074,
            5085, 5099, 5111, 5169, 5198, 5199, 5200, 5211,
            5231, 5251, 5261, 5543, 7394);

        SetRange(t, MccCategory.FoodAndDining, 5811, 5814);

        Set(t, MccCategory.DirectSales,
            4761, 5964, 5965, 5966, 5967, 5969);

        Set(t, MccCategory.Education,
            8211, 8220, 8241, 8244, 8249, 8299);

        Set(t, MccCategory.Electronics,
            5045, 5722, 5732, 5946);

        Set(t, MccCategory.Leisure,
            5733, 5735, 5971, 7032, 7829, 7832, 7833, 7841,
            7911, 7922, 7929, 7932, 7933, 7941);
        SetRange(t, MccCategory.Leisure, 7991, 7994);
        SetRange(t, MccCategory.Leisure, 7996, 7999);

        Set(t, MccCategory.Finance,
            4829, 6010, 6011, 6012, 6050, 6051, 6211, 6529, 6530,
            6533, 6535, 6536, 6537, 6538, 6539, 6540, 6611,
            6760, 7276, 7321, 7322, 8931, 9700, 9701, 9702);

        Set(t, MccCategory.Betting,
            7800, 7801, 7802, 7995, 9406, 9754);

        Set(t, MccCategory.Government,
            9211, 9222, 9223, 9311, 9312, 9313, 9314, 9399,
            9401, 9402, 9405, 9034);

        Set(t, MccCategory.Supermarkets,
            5300, 5411, 5412, 5422, 5441, 5451, 5462, 5499,
            9751);

        Set(t, MccCategory.Healthcare,
            5047, 5122, 5292, 5912, 5975, 5976, 5977, 7230,
            7297, 7298, 8011, 8021, 8031, 8041, 8042, 8043,
            8044, 8049, 8050, 8062, 8071, 8099);

        SetRange(t, MccCategory.Accommodation, 3501, 3839);
        Set(t, MccCategory.Accommodation,
            7011, 7012, 7033);

        Set(t, MccCategory.Insurance,
            5960, 6300, 6381, 6399);

        Set(t, MccCategory.DigitalServices,
            4816, 5262, 5734, 5815, 5816, 5817, 5818, 5968,
            7372, 7375, 7379, 7399);

        Set(t, MccCategory.Retail,
            5094, 5131, 5137, 5139, 5192, 5193, 5309, 5310,
            5311, 5331, 5399, 5611, 5621, 5631, 5641, 5651,
            5655, 5661, 5681, 5691, 5697, 5698, 5699, 5712,
            5713, 5714, 5715, 5718, 5719, 5921, 5931, 5932,
            5933, 5937, 5940, 5941, 5942, 5944, 5945, 5947,
            5948, 5949, 5950, 5961, 5970, 5972, 5973, 5974,
            5978, 5992, 5993, 5994, 5995, 5996, 5997, 5998,
            5999, 7296);

        // Codes with leading zeros: "0742" → 742, "0763" → 763, "0780" → 780
        Set(t, MccCategory.GeneralServices,
            742,  763,  780,
            2741, 2791, 2842, 4214, 4215, 4225,
            5962, 5963, 6513, 6532, 7210, 7211, 7216, 7217,
            7221, 7251, 7261, 7273, 7277, 7278, 7280, 7295,
            7299, 7332, 7333, 7338, 7339, 7342, 7349, 7361,
            7392, 7393, 7395, 7622, 7623, 7629, 7631, 7641,
            7692, 7699, 8111, 8351, 8911, 8999, 9950);

        Set(t, MccCategory.OfficeSupplies,
            5943);

        Set(t, MccCategory.Transportation,
            4011, 4111, 4112, 4119, 4121, 4131, 4411, 4457,
            4468, 4582, 4789, 5551, 6236);

        Set(t, MccCategory.TravelAgencies,
            4722, 4723);

        Set(t, MccCategory.Utilities,
            4812, 4813, 4814, 4821, 4899, 4900);

        return t;
    }

    private static void Set(MccCategory[] t, MccCategory cat, params int[] codes)
    {
        foreach (var c in codes)
            t[c] = cat;
    }

    private static void SetRange(MccCategory[] t, MccCategory cat, int from, int to)
    {
        for (var i = from; i <= to; i++)
            t[i] = cat;
    }
}
