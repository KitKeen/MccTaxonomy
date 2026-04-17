using Xunit;

namespace MccTaxonomy.Tests;

public class MccLookupTests
{
    // -------------------------------------------------------------------------
    // Categorize(string) — existing coverage
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData("5411", MccCategory.Supermarkets)]
    [InlineData("5812", MccCategory.FoodAndDining)]
    [InlineData("7311", MccCategory.Marketing)]
    [InlineData("4511", MccCategory.Airlines)]
    [InlineData("3100", MccCategory.Airlines)]
    [InlineData("5541", MccCategory.Automotive)]
    [InlineData("7512", MccCategory.VehicleRental)]
    [InlineData("8398", MccCategory.Charity)]
    [InlineData("5211", MccCategory.Construction)]
    [InlineData("5964", MccCategory.DirectSales)]
    [InlineData("8211", MccCategory.Education)]
    [InlineData("5732", MccCategory.Electronics)]
    [InlineData("7832", MccCategory.Leisure)]
    [InlineData("6010", MccCategory.Finance)]
    [InlineData("7800", MccCategory.Betting)]
    [InlineData("9211", MccCategory.Government)]
    [InlineData("5912", MccCategory.Healthcare)]
    [InlineData("7011", MccCategory.Accommodation)]
    [InlineData("3600", MccCategory.Accommodation)]
    [InlineData("6300", MccCategory.Insurance)]
    [InlineData("5816", MccCategory.DigitalServices)]
    [InlineData("5311", MccCategory.Retail)]
    [InlineData("7210", MccCategory.GeneralServices)]
    [InlineData("5943", MccCategory.OfficeSupplies)]
    [InlineData("4121", MccCategory.Transportation)]
    [InlineData("4722", MccCategory.TravelAgencies)]
    [InlineData("4900", MccCategory.Utilities)]
    public void Categorize_String_KnownCode_ReturnsExpectedCategory(string mcc, MccCategory expected)
    {
        Assert.Equal(expected, MccLookup.Categorize(mcc));
    }

    [Fact]
    public void Categorize_String_UnknownCode_ReturnsUncategorized()
    {
        Assert.Equal(MccCategory.Uncategorized, MccLookup.Categorize("0000"));
    }

    [Fact]
    public void Categorize_String_Null_ReturnsUncategorized()
    {
        // Null is treated as "unknown input" — consistent with the tolerant policy
        // of the rest of the string/span API (no exceptions for malformed input).
        Assert.Equal(MccCategory.Uncategorized, MccLookup.Categorize((string?)null));
    }

    [Fact]
    public void TryGetCategory_String_Null_ReturnsFalse()
    {
        Assert.False(MccLookup.TryGetCategory((string?)null, out var category));
        Assert.Equal(MccCategory.Uncategorized, category);
    }

    [Theory]
    [InlineData(" 5411")]   // leading whitespace
    [InlineData("5411 ")]   // trailing whitespace
    [InlineData("+5411")]   // sign
    [InlineData("-1")]      // negative
    [InlineData("54A1")]    // non-digit
    [InlineData("54111")]   // too long
    [InlineData("")]        // empty
    public void Categorize_String_Malformed_ReturnsUncategorized(string input)
    {
        Assert.Equal(MccCategory.Uncategorized, MccLookup.Categorize(input));
        Assert.False(MccLookup.TryGetCategory(input, out _));
    }

    // -------------------------------------------------------------------------
    // Categorize(ReadOnlySpan<char>)
    // -------------------------------------------------------------------------

    [Fact]
    public void Categorize_Span_KnownCode_ReturnsExpectedCategory()
    {
        Assert.Equal(MccCategory.Supermarkets, MccLookup.Categorize("5411".AsSpan()));
        Assert.Equal(MccCategory.Supermarkets, MccLookup.Categorize("05411".AsSpan(1, 4)));
    }

    [Fact]
    public void Categorize_Span_LeadingZero_ResolvesCorrectly()
    {
        Assert.Equal(MccLookup.Categorize(742), MccLookup.Categorize("0742".AsSpan()));
    }

    [Fact]
    public void Categorize_Span_Empty_ReturnsUncategorized()
    {
        Assert.Equal(MccCategory.Uncategorized, MccLookup.Categorize(ReadOnlySpan<char>.Empty));
    }

    [Fact]
    public void TryGetCategory_Span_KnownCode_ReturnsTrueAndCategory()
    {
        var found = MccLookup.TryGetCategory("5411".AsSpan(), out var category);
        Assert.True(found);
        Assert.Equal(MccCategory.Supermarkets, category);
    }

    [Fact]
    public void Categorize_String_LeadingZero_ResolvesCorrectly()
    {
        // "0742" (Veterinary) should resolve to the same entry as int 742
        Assert.Equal(MccLookup.Categorize("0742"), MccLookup.Categorize(742));
        Assert.NotEqual(MccCategory.Uncategorized, MccLookup.Categorize("0742"));
    }

    // -------------------------------------------------------------------------
    // Categorize(int) — new int API
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(5411, MccCategory.Supermarkets)]
    [InlineData(5812, MccCategory.FoodAndDining)]
    [InlineData(7311, MccCategory.Marketing)]
    [InlineData(4511, MccCategory.Airlines)]
    [InlineData(3100, MccCategory.Airlines)]
    [InlineData(5541, MccCategory.Automotive)]
    [InlineData(7512, MccCategory.VehicleRental)]
    [InlineData(8398, MccCategory.Charity)]
    [InlineData(5211, MccCategory.Construction)]
    [InlineData(5964, MccCategory.DirectSales)]
    [InlineData(8211, MccCategory.Education)]
    [InlineData(5732, MccCategory.Electronics)]
    [InlineData(7832, MccCategory.Leisure)]
    [InlineData(6010, MccCategory.Finance)]
    [InlineData(7800, MccCategory.Betting)]
    [InlineData(9211, MccCategory.Government)]
    [InlineData(5912, MccCategory.Healthcare)]
    [InlineData(7011, MccCategory.Accommodation)]
    [InlineData(3600, MccCategory.Accommodation)]
    [InlineData(6300, MccCategory.Insurance)]
    [InlineData(5816, MccCategory.DigitalServices)]
    [InlineData(5311, MccCategory.Retail)]
    [InlineData(7210, MccCategory.GeneralServices)]
    [InlineData(5943, MccCategory.OfficeSupplies)]
    [InlineData(4121, MccCategory.Transportation)]
    [InlineData(4722, MccCategory.TravelAgencies)]
    [InlineData(4900, MccCategory.Utilities)]
    public void Categorize_Int_KnownCode_ReturnsExpectedCategory(int mcc, MccCategory expected)
    {
        Assert.Equal(expected, MccLookup.Categorize(mcc));
    }

    [Fact]
    public void Categorize_Int_UnknownCode_ReturnsUncategorized()
    {
        Assert.Equal(MccCategory.Uncategorized, MccLookup.Categorize(0));
    }

    [Fact]
    public void Categorize_Int_OutOfRange_ReturnsUncategorized()
    {
        Assert.Equal(MccCategory.Uncategorized, MccLookup.Categorize(-1));
        Assert.Equal(MccCategory.Uncategorized, MccLookup.Categorize(10_000));
        Assert.Equal(MccCategory.Uncategorized, MccLookup.Categorize(99_999));
    }

    [Fact]
    public void Categorize_Int_And_String_ReturnSameResult()
    {
        Assert.Equal(MccLookup.Categorize(5411), MccLookup.Categorize("5411"));
        Assert.Equal(MccLookup.Categorize(742),  MccLookup.Categorize("0742"));
    }

    // -------------------------------------------------------------------------
    // TryGetCategory
    // -------------------------------------------------------------------------

    [Fact]
    public void TryGetCategory_String_KnownCode_ReturnsTrueAndCategory()
    {
        var found = MccLookup.TryGetCategory("5411", out var category);
        Assert.True(found);
        Assert.Equal(MccCategory.Supermarkets, category);
    }

    [Fact]
    public void TryGetCategory_String_UnknownCode_ReturnsFalse()
    {
        Assert.False(MccLookup.TryGetCategory("0000", out _));
    }

    [Fact]
    public void TryGetCategory_Int_KnownCode_ReturnsTrueAndCategory()
    {
        var found = MccLookup.TryGetCategory(5411, out var category);
        Assert.True(found);
        Assert.Equal(MccCategory.Supermarkets, category);
    }

    [Fact]
    public void TryGetCategory_Int_UnknownCode_ReturnsFalse()
    {
        Assert.False(MccLookup.TryGetCategory(0, out _));
    }

    // -------------------------------------------------------------------------
    // GetCodes / GetCodeValues
    // -------------------------------------------------------------------------

    [Fact]
    public void GetCodes_Supermarkets_ContainsExpectedCodes()
    {
        var codes = MccLookup.GetCodes(MccCategory.Supermarkets).ToList();
        Assert.Contains("5411", codes);
        Assert.Contains("5499", codes);
    }

    [Fact]
    public void GetCodeValues_Supermarkets_ContainsExpectedCodes()
    {
        var codes = MccLookup.GetCodeValues(MccCategory.Supermarkets).ToList();
        Assert.Contains(5411, codes);
        Assert.Contains(5499, codes);
    }

    [Fact]
    public void GetCodes_And_GetCodeValues_AreConsistent()
    {
        var stringCodes = MccLookup.GetCodes(MccCategory.Airlines).Select(int.Parse).OrderBy(x => x).ToList();
        var intCodes    = MccLookup.GetCodeValues(MccCategory.Airlines).OrderBy(x => x).ToList();
        Assert.Equal(stringCodes, intCodes);
    }

    // -------------------------------------------------------------------------
    // Count
    // -------------------------------------------------------------------------

    [Fact]
    public void Count_IsGreaterThanZero()
    {
        Assert.True(MccLookup.Count > 800, $"Expected >800 codes, got {MccLookup.Count}");
    }

    // -------------------------------------------------------------------------
    // Range coverage
    // -------------------------------------------------------------------------

    [Fact]
    public void Airlines_Range_CoversFullRange()
    {
        // 3000–3350 = 351 codes
        for (var i = 3000; i <= 3350; i++)
            Assert.Equal(MccCategory.Airlines, MccLookup.Categorize(i));
    }

    [Fact]
    public void Hotels_Range_CoversFullRange()
    {
        // 3501–3839 = 339 codes
        for (var i = 3501; i <= 3839; i++)
            Assert.Equal(MccCategory.Accommodation, MccLookup.Categorize(i));
    }

    // -------------------------------------------------------------------------
    // WithCustomCodes
    // -------------------------------------------------------------------------

    [Fact]
    public void WithCustomCodes_Override_ReturnsUpdatedCategory()
    {
        var custom = MccLookup.WithCustomCodes(new Dictionary<int, MccCategory>
        {
            [9999] = MccCategory.Finance,
        });

        Assert.Equal(MccCategory.Finance, custom.Categorize(9999));
        Assert.Equal(MccCategory.Finance, custom.Categorize("9999"));
    }

    [Fact]
    public void WithCustomCodes_DoesNotModifyDefault()
    {
        _ = MccLookup.WithCustomCodes(new Dictionary<int, MccCategory>
        {
            [9999] = MccCategory.Finance,
        });

        // The static default must remain unchanged
        Assert.Equal(MccCategory.Uncategorized, MccLookup.Categorize(9999));
    }

    [Fact]
    public void WithCustomCodes_CanChain()
    {
        var custom = MccLookup
            .WithCustomCodes(new Dictionary<int, MccCategory> { [9998] = MccCategory.Retail })
            .WithCustomCodes(new Dictionary<int, MccCategory> { [9997] = MccCategory.Education });

        Assert.Equal(MccCategory.Retail,     custom.Categorize(9998));
        Assert.Equal(MccCategory.Education,  custom.Categorize(9997));
    }

    [Fact]
    public void WithCustomCodes_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            MccLookup.WithCustomCodes(null!));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10_000)]
    [InlineData(int.MaxValue)]
    public void WithCustomCodes_OutOfRangeKey_Throws(int invalidCode)
    {
        var overrides = new Dictionary<int, MccCategory> { [invalidCode] = MccCategory.Finance };
        Assert.Throws<ArgumentOutOfRangeException>(() => MccLookup.WithCustomCodes(overrides));
    }

    [Fact]
    public void WithCustomCodes_SetUncategorized_RemovesExistingCode()
    {
        // 5411 is a built-in Supermarkets code; overriding with Uncategorized removes it.
        var custom = MccLookup.WithCustomCodes(new Dictionary<int, MccCategory>
        {
            [5411] = MccCategory.Uncategorized,
        });

        Assert.Equal(MccCategory.Uncategorized, custom.Categorize(5411));
        Assert.False(custom.TryGetCategory(5411, out _));
        Assert.Equal(MccLookup.Count - 1, custom.Count);
    }

    // -------------------------------------------------------------------------
    // Enum stability — Uncategorized is pinned to 0
    // -------------------------------------------------------------------------

    [Fact]
    public void Uncategorized_IsPinnedToZero()
    {
        // Lookup presence tracking does not depend on this anymore (a parallel
        // bool[] is used), but the enum value is part of the public API and
        // downstream code may rely on `default(MccCategory) == Uncategorized`.
        Assert.Equal(0, (int)MccCategory.Uncategorized);
        Assert.Equal(MccCategory.Uncategorized, default(MccCategory));
    }

    // -------------------------------------------------------------------------
    // GetDescription
    // -------------------------------------------------------------------------

    [Fact]
    public void GetDescription_AllCategoriesHaveNonEmptyDescription()
    {
        foreach (MccCategory cat in Enum.GetValues(typeof(MccCategory)))
        {
            var desc = cat.GetDescription();
            Assert.False(string.IsNullOrWhiteSpace(desc),
                $"MccCategory.{cat} has no description");
        }
    }

    [Fact]
    public void GetDescription_Supermarkets_ReturnsCorrectDescription()
    {
        Assert.Equal("Grocery Stores and Supermarkets", MccCategory.Supermarkets.GetDescription());
    }
}
