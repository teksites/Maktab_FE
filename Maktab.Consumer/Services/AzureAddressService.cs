using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Maktab.Consumer.Services
{
    // ── Result model returned to the UI ──────────────────────────────────────
    public class AzureAddressResult
    {
        public string FormattedAddress { get; init; } = string.Empty;
        public string StreetLine        { get; init; } = string.Empty;
        public string City              { get; init; } = string.Empty;
        public string Province          { get; init; } = string.Empty;
        public string County            { get; init; } = string.Empty;
        public string PostalCode        { get; init; } = string.Empty;
        public string Country           { get; init; } = string.Empty;
    }

    // ── Service interface ─────────────────────────────────────────────────────
    public interface IAzureAddressService
    {
        Task<IReadOnlyList<AzureAddressResult>> SearchAsync(string query, CancellationToken ct = default);
    }

    // ── Service implementation ────────────────────────────────────────────────
    public class AzureAddressService : IAzureAddressService
    {
        private const string BaseUrl      = "https://atlas.microsoft.com/geocode:autocomplete";
        private const string ApiVersion   = "2026-01-01";
        private const string CountryCode  = "CA";
        private const string Language     = "en-CA";
        private const string CanadaBbox   = "-141.0,41.7,-52.6,83.1";

        private readonly HttpClient    _http;
        private readonly string        _subscriptionKey;

        public AzureAddressService(HttpClient http, IConfiguration config)
        {
            _http            = http;
            _subscriptionKey = config["AzureMaps:SubscriptionKey"] ?? string.Empty;
        }

        public async Task<IReadOnlyList<AzureAddressResult>> SearchAsync(string query, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
                return [];

            var qs = HttpUtility.ParseQueryString(string.Empty);
            qs["api-version"]      = ApiVersion;
            qs["query"]            = query;
            qs["bbox"]             = CanadaBbox;
            qs["resultTypes"]      = "Address";
            qs["countryRegion"]    = CountryCode;
            qs["language"]         = Language;
            qs["top"]              = "8";
            qs["subscription-key"] = _subscriptionKey;

            var url = $"{BaseUrl}?{qs}";

            try
            {
                var response = await _http.GetFromJsonAsync<AzureGeoResponse>(url, ct);
                return response?.Features?.Select(Parse).ToList() ?? [];
            }
            catch
            {
                return [];
            }
        }

        // ── Private helpers ───────────────────────────────────────────────────
        private static AzureAddressResult Parse(AzureFeature feature)
        {
            var addr   = feature.Properties?.Address ?? new AzureAddress();
            var street = $"{addr.StreetNumber} {addr.StreetName}".Trim();
            if (string.IsNullOrWhiteSpace(street)) street = addr.AddressLine ?? string.Empty;

            var province = addr.AdminDistricts?.ElementAtOrDefault(0)?.Name ?? string.Empty;
            var county   = addr.AdminDistricts?.ElementAtOrDefault(1)?.Name ?? string.Empty;

            return new AzureAddressResult
            {
                FormattedAddress = addr.FormattedAddress ?? street,
                StreetLine       = street,
                City             = addr.Locality ?? addr.Municipality ?? string.Empty,
                Province         = province,
                County           = county,
                PostalCode       = addr.PostalCode ?? string.Empty,
                Country          = "Canada"
            };
        }

        // ── Internal Azure Maps response DTOs ─────────────────────────────────
        private sealed class AzureGeoResponse
        {
            [JsonPropertyName("features")]
            public List<AzureFeature>? Features { get; set; }
        }

        private sealed class AzureFeature
        {
            [JsonPropertyName("properties")]
            public AzureProperties? Properties { get; set; }
        }

        private sealed class AzureProperties
        {
            [JsonPropertyName("address")]
            public AzureAddress? Address { get; set; }
        }

        private sealed class AzureAddress
        {
            [JsonPropertyName("streetNumber")]
            public string? StreetNumber { get; set; }

            [JsonPropertyName("streetName")]
            public string? StreetName { get; set; }

            [JsonPropertyName("addressLine")]
            public string? AddressLine { get; set; }

            [JsonPropertyName("locality")]
            public string? Locality { get; set; }

            [JsonPropertyName("municipality")]
            public string? Municipality { get; set; }

            [JsonPropertyName("adminDistricts")]
            public List<AdminDistrict>? AdminDistricts { get; set; }

            [JsonPropertyName("postalCode")]
            public string? PostalCode { get; set; }

            [JsonPropertyName("formattedAddress")]
            public string? FormattedAddress { get; set; }
        }

        private sealed class AdminDistrict
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; }
        }
    }
}
