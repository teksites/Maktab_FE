using MaktabDataContracts.Responses.Addresses;
using Microsoft.Extensions.Localization;

namespace Maktab.Consumer.Services
{
    public interface IAddressFormattingService
    {
        /// <summary>
        /// Formats the street address (unit, apartment, street lines) for display
        /// Suitable for multi-line display
        /// </summary>
        string FormatStreetAddress(AddressResponse address);

        /// <summary>
        /// Formats city, province, and postal code together
        /// </summary>
        string FormatCityProvincePostal(AddressResponse address);

        /// <summary>
        /// Gets a human-readable label for the address type (e.g., "Home Address", "Student Address")
        /// </summary>
        string GetAddressTypeLabel(AddressResponse address);

        /// <summary>
        /// Returns structured address components for flexible display
        /// </summary>
        (string Street, string CityPostal, string Country) FormatAddressStructured(AddressResponse address);
    }

    public class AddressFormattingService : IAddressFormattingService
    {
        private readonly IStringLocalizer<AddressFormattingService> _localizer;

        public AddressFormattingService(IStringLocalizer<AddressFormattingService> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Formats street address components with proper line breaks for readability
        /// Example output:
        /// Unit 2, Apt 5
        /// 123 Main St, 456 Secondary St
        /// </summary>
        public string FormatStreetAddress(AddressResponse address)
        {
            var lines = new List<string>();

            // First line: Unit/Apartment info if present
            var unitInfo = new List<string>();
            if (!string.IsNullOrWhiteSpace(address.UnitNo))
                unitInfo.Add($"{_localizer["Unit"]} {address.UnitNo}");
            if (!string.IsNullOrWhiteSpace(address.ApartmentNo))
                unitInfo.Add($"{_localizer["Apt"]} {address.ApartmentNo}");

            if (unitInfo.Any())
                lines.Add(string.Join(", ", unitInfo));

            // Second line: Main address lines
            var addressLines = new List<string>();
            if (!string.IsNullOrWhiteSpace(address.AddressLine1))
                addressLines.Add(address.AddressLine1);
            if (!string.IsNullOrWhiteSpace(address.AddressLine2))
                addressLines.Add(address.AddressLine2);

            if (addressLines.Any())
                lines.Add(string.Join(", ", addressLines));

            return string.Join("\n", lines);
        }

        /// <summary>
        /// Formats city, province, and postal code in a single line
        /// Example: Toronto, Ontario M1A1A1
        /// </summary>
        public string FormatCityProvincePostal(AddressResponse address)
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(address.City))
                parts.Add(address.City);

            if (!string.IsNullOrWhiteSpace(address.Province))
                parts.Add(address.Province);

            if (!string.IsNullOrWhiteSpace(address.PostalCode))
                parts.Add(address.PostalCode);

            return string.Join(" ", parts);
        }

        /// <summary>
        /// Returns structured address for maximum flexibility in display
        /// Allows caller to format as needed (separate lines, paragraphs, etc.)
        /// </summary>
        public (string Street, string CityPostal, string Country) FormatAddressStructured(AddressResponse address)
        {
            return (
                Street: FormatStreetAddress(address),
                CityPostal: FormatCityProvincePostal(address),
                Country: address.Country ?? string.Empty
            );
        }

        /// <summary>
        /// Gets user-friendly label for address type
        /// </summary>
        public string GetAddressTypeLabel(AddressResponse address)
        {
            if (address.HomeAddress)
                return _localizer["Home Address"];
            
            return address?.AddressType.ToString() ?? _localizer["Address"];
        }
    }
}
