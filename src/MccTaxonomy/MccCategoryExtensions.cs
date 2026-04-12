namespace MccTaxonomy;

/// <summary>
/// Extension methods for <see cref="MccCategory"/>.
/// </summary>
public static class MccCategoryExtensions
{
    /// <summary>
    /// Returns a human-readable description for the given <see cref="MccCategory"/>.
    /// </summary>
    public static string GetDescription(this MccCategory category) => category switch
    {
        MccCategory.Marketing      => "Advertising and Marketing Services",
        MccCategory.Airlines       => "Airlines and Air Carriers",
        MccCategory.Automotive     => "Automotive Parts, Dealers and Services",
        MccCategory.VehicleRental  => "Car and Vehicle Rental",
        MccCategory.Charity        => "Charitable and Social Service Organizations",
        MccCategory.Construction   => "Construction, Home Improvement and Building Supplies",
        MccCategory.FoodAndDining  => "Restaurants, Fast Food and Dining",
        MccCategory.DirectSales    => "Direct Marketing and Catalog Merchants",
        MccCategory.Education      => "Educational Services and Schools",
        MccCategory.Electronics    => "Electronics, Computers and Software",
        MccCategory.Leisure        => "Entertainment, Recreation and Leisure",
        MccCategory.Finance        => "Financial Services, Banking and Investment",
        MccCategory.Betting        => "Gambling, Casinos and Betting",
        MccCategory.Government     => "Government Services and Agencies",
        MccCategory.Supermarkets   => "Grocery Stores and Supermarkets",
        MccCategory.Healthcare     => "Healthcare, Medical and Dental Services",
        MccCategory.Accommodation  => "Hotels, Motels and Lodging",
        MccCategory.Insurance      => "Insurance Services",
        MccCategory.DigitalServices => "Digital Goods, Software and Online Services",
        MccCategory.Retail         => "Retail Stores and General Shopping",
        MccCategory.GeneralServices => "General and Professional Services",
        MccCategory.OfficeSupplies => "Office and Stationery Supplies",
        MccCategory.Transportation => "Transportation, Transit and Shipping",
        MccCategory.TravelAgencies => "Travel Agencies and Tour Operators",
        MccCategory.Utilities      => "Utilities and Telecommunications",
        MccCategory.Uncategorized  => "Uncategorized",
        _                          => "Unknown",
    };
}
