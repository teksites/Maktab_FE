using Maktab.Core.Interfaces.Models;
using MaktabDataContracts.Responses.Addresses;

namespace Maktab.Core.Interfaces.Services
{
     public interface IAddressLookupService
     {
          Task<IReadOnlyList<AddressLookupResult>> SearchAdderssAsync(string query, CancellationToken ct = default);

          Task<AddressResponse> GetAddressFromLookupResultAsync(AddressLookupResult result);
     }
}
