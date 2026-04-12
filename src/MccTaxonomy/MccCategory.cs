namespace MccTaxonomy;

/// <summary>
/// Merchant Category Code (MCC) categories based on ISO 18245.
/// Groups ~900 individual MCC codes into 27 human-readable categories.
/// </summary>
public enum MccCategory
{
    /// <summary>Advertising and marketing agencies.</summary>
    Marketing,

    /// <summary>Airlines and air carriers.</summary>
    Airlines,

    /// <summary>Automotive dealers, parts stores and repair services.</summary>
    Automotive,

    /// <summary>Car and vehicle rental agencies.</summary>
    VehicleRental,

    /// <summary>Charitable and social service organizations.</summary>
    Charity,

    /// <summary>Construction contractors and building supplies.</summary>
    Construction,

    /// <summary>Restaurants, fast food outlets and dining.</summary>
    FoodAndDining,

    /// <summary>Direct marketing, catalog and mail-order merchants.</summary>
    DirectSales,

    /// <summary>Educational institutions and services.</summary>
    Education,

    /// <summary>Electronics retailers, computers and software.</summary>
    Electronics,

    /// <summary>Entertainment, recreation and leisure activities.</summary>
    Leisure,

    /// <summary>Financial services, banking and investment.</summary>
    Finance,

    /// <summary>Gambling, casinos and betting services.</summary>
    Betting,

    /// <summary>Government agencies and public services.</summary>
    Government,

    /// <summary>Grocery stores and supermarkets.</summary>
    Supermarkets,

    /// <summary>Healthcare providers, pharmacies and medical services.</summary>
    Healthcare,

    /// <summary>Hotels, motels and short-term accommodation.</summary>
    Accommodation,

    /// <summary>Insurance services and underwriters.</summary>
    Insurance,

    /// <summary>Digital goods, software subscriptions and online services.</summary>
    DigitalServices,

    /// <summary>Retail stores and general merchandise shopping.</summary>
    Retail,

    /// <summary>General and professional services (cleaning, repairs, consulting, etc.).</summary>
    GeneralServices,

    /// <summary>Office and stationery supplies.</summary>
    OfficeSupplies,

    /// <summary>Transportation providers: rail, taxi, ferry, shipping.</summary>
    Transportation,

    /// <summary>Travel agencies and tour operators.</summary>
    TravelAgencies,

    /// <summary>Utility providers and telecommunications.</summary>
    Utilities,

    /// <summary>MCC code not matched by any known category.</summary>
    Uncategorized,
}
