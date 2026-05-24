namespace Maktab.Core.Interfaces.Models
{
     public class AddressLookupResult
     {
          public string FormattedAddress { get; init; } = string.Empty;
          public string StreetLine { get; init; } = string.Empty;
          public string City { get; init; } = string.Empty;
          public string Province { get; init; } = string.Empty;
          public string County { get; init; } = string.Empty;
          public string PostalCode { get; init; } = string.Empty;
          public string Country { get; init; } = string.Empty;
     }
}
