using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Models;
using MaktabDataContracts.Requests.Children;
using MaktabDataContracts.Responses.Children;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maktab.Domain.Services
{
     /// <summary>
     /// Children service with comprehensive error handling, validation, and logging.
     /// Production-ready implementation for backend API integration.
     /// </summary>
     public class ChildrenService : BaseService, IChildrenService
     {
          // API Endpoints (properly formatted, not "formated")
          private const string getChildById = @"/api/children/{0}";
          private const string removeChildById = @"/api/children/{0}/delete?ifHardDelete=false";
          private const string getChildrenByFamilyId = @"/api/families/{0}/children";
          private const string addChildByFamilyId = @"/api/families/{0}/children/add";
          private const string removeChildByFamilyId = @"/api/families/{0}/children/delete?ifHardDelete=false";
          private const string isChildrenExistByRamQNumber = @"/api/children/check";

          private readonly ILogger<ChildrenService> _logger;

          public ChildrenService(
               IHttpService httpService,
               ILocalStorageService localStorageService,
               ILogger<ChildrenService> logger)
               : base(httpService, localStorageService)
          {
               _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          }

          /// <summary>
          /// Get child by ID with error handling and validation
          /// </summary>
          public async Task<ChildResponse> GetChildByIdAsync(Guid childId)
          {
               try
               {
                    // Validate input
                    if (childId == Guid.Empty)
                    {
                         _logger.LogWarning("GetChildByIdAsync called with empty GUID");
                         throw new ArgumentException("Child ID cannot be empty", nameof(childId));
                    }

                    _logger.LogInformation("Fetching child {ChildId}", childId);

                    // Make request
                    var formattedUrl = string.Format(getChildById, childId);
                    var result = await _httpService.Get<MaktabApiResult<ChildResponse>>(formattedUrl);

                    // Validate response
                    if (result == null || result.Result == null)
                    {
                         _logger.LogWarning("Child {ChildId} not found", childId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched child {ChildId}", childId);
                    return result.Result;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching child {ChildId}", childId);
                    throw;
               }
               catch (TaskCanceledException ex)
               {
                    _logger.LogError(ex, "Request timeout fetching child {ChildId}", childId);
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching child {ChildId}", childId);
                    throw;
               }
          }

          /// <summary>
          /// Get all children for a family with error handling
          /// </summary>
          public async Task<IEnumerable<ChildResponse>> GetChildrenByFamilyIdAsync(Guid familyId)
          {
               try
               {
                    // Validate input
                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("GetChildrenByFamilyIdAsync called with empty GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    _logger.LogInformation("Fetching children for family {FamilyId}", familyId);

                    // Make request
                    var formattedUrl = string.Format(getChildrenByFamilyId, familyId);
                    var result = await _httpService.Get<List<MaktabApiResult<ChildResponse>>>(formattedUrl);

                    // Validate response and unwrap
                    if (result == null || result.Count == 0)
                    {
                         _logger.LogWarning("No children found for family {FamilyId}", familyId);
                         return new List<ChildResponse>();
                    }

                    var children = result
                         .Where(x => x?.Result != null)
                         .Select(x => x.Result)
                         .ToList();

                    _logger.LogInformation("Successfully fetched {ChildCount} children for family {FamilyId}", children.Count, familyId);
                    return children;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching children for family {FamilyId}", familyId);
                    throw;
               }
               catch (TaskCanceledException ex)
               {
                    _logger.LogError(ex, "Request timeout fetching children for family {FamilyId}", familyId);
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching children for family {FamilyId}", familyId);
                    throw;
               }
          }

          /// <summary>
          /// Add child to family with validation and error handling
          /// </summary>
          public async Task<ChildResponse> AddChildToFamilyAsync(Guid familyId, AddChildRequest addChildRequest)
          {
               try
               {
                    // Validate input
                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("AddChildToFamilyAsync called with empty family GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    if (addChildRequest == null)
                    {
                         _logger.LogWarning("AddChildToFamilyAsync called with null request");
                         throw new ArgumentNullException(nameof(addChildRequest), "Child request cannot be null");
                    }

                    _logger.LogInformation("Adding child to family {FamilyId}", familyId);

                    // Make request
                    var formattedUrl = string.Format(addChildByFamilyId, familyId);
                    var result = await _httpService.Post<MaktabApiResult<ChildResponse>>(formattedUrl, addChildRequest);

                    // Validate response
                    if (result == null || result.Result == null)
                    {
                         _logger.LogError("Server returned null response when adding child");
                         throw new InvalidOperationException("Server did not return child confirmation");
                    }

                    _logger.LogInformation("Successfully added child to family {FamilyId}", familyId);
                    return result.Result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error adding child to family {FamilyId}", familyId);
                    throw;
               }
          }

          /// <summary>
          /// Check if child exists with RAMQ number (backend uses POST)
          /// </summary>
          public async Task<bool> IsChildExistWithRamQNumberAsync(Guid familyId, string ramqNumber)
          {
               try
               {
                    // Validate input
                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("IsChildExistWithRamQNumberAsync called with empty family GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    if (string.IsNullOrWhiteSpace(ramqNumber))
                    {
                         _logger.LogWarning("IsChildExistWithRamQNumberAsync called with null/empty RAMQ number");
                         throw new ArgumentException("RAMQ number cannot be null or empty", nameof(ramqNumber));
                    }

                    _logger.LogInformation("Checking if child exists with RAMQ number for family {FamilyId}", familyId);

                    //  Use POST (backend uses POST for this check)
                    var verificationRequest = new
                    {
                         FamilyId = familyId,
                         RamqNumber = ramqNumber
                    };

                    var result = await _httpService.Post<bool>(isChildrenExistByRamQNumber, verificationRequest);

                    _logger.LogInformation("Child check result for family {FamilyId}: {Exists}", familyId, result);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error checking if child exists with RAMQ number for family {FamilyId}", familyId);
                    throw;
               }
          }

          /// <summary>
          /// Remove child by ID (backend uses POST, not DELETE)
          /// </summary>
          public async Task<bool> RemoveChildByIdAsync(Guid childId)
          {
               try
               {
                    // Validate input
                    if (childId == Guid.Empty)
                    {
                         _logger.LogWarning("RemoveChildByIdAsync called with empty GUID");
                         throw new ArgumentException("Child ID cannot be empty", nameof(childId));
                    }

                    _logger.LogInformation("Removing child {ChildId}", childId);

                    // Use POST (backend uses POST for delete, not DELETE)
                    var formattedUrl = string.Format(removeChildById, childId);
                    var result = await _httpService.Post<bool>(formattedUrl, null);

                    _logger.LogInformation("Successfully removed child {ChildId}", childId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error removing child {ChildId}", childId);
                    throw;
               }
          }

          /// <summary>
          /// Remove all children from family (backend uses POST, not DELETE)
          /// </summary>
          public async Task<bool> RemoveChildFromFamilyAsync(Guid familyId)
          {
               try
               {
                    // Validate input
                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("RemoveChildFromFamilyAsync called with empty GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    _logger.LogInformation("Removing children for family {FamilyId}", familyId);

                    // Use POST (backend uses POST for delete, not DELETE)
                    var formattedUrl = string.Format(removeChildByFamilyId, familyId);
                    var result = await _httpService.Post<bool>(formattedUrl, null);

                    _logger.LogInformation("Successfully removed children for family {FamilyId}", familyId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error removing children for family {FamilyId}", familyId);
                    throw;
               }
          }
     }
}
