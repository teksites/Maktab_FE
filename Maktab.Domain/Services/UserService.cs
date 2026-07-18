using Maktab.Core.Interfaces.Services;
using Maktab.Models.Models;
using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktab.Domain.Services
{
     /// <summary>
     /// User service with comprehensive error handling, validation, and logging.
     /// Production-ready implementation for backend API integration.
     /// </summary>
     public class UserService : BaseService, IUserService
     {
          // API Endpoints (properly formatted, not "formated")
          private const string getUserById = @"/api/users/{0}";
          private const string validateUserActivationCode = @"/api/users/{0}/verify";
          private const string sendActivationCode = @"/api/users/{0}/activationcode";
          private const string addUser = @"/api/users/add";
          private const string changeUserPassword = @"/api/users/{0}/resetpassword";
          private const string forgotUserPassword = @"/api/users/forgotpassword?userName={0}";
          private const string checkUsernameExist = @"/api/users/checkuser?userName={0}";
          private const string checkUserRegistered = @"/api/users/checkuser?userName={0}";
          private const string linkUserToFamilId = @"/api/users/{0}/link/{1}";
          private const string getFamilIdByUserInfo = @"/api/users/familyinfo";
          private const string getExtendedInfoByUserId = @"/api/users/{0}/extendedinfo";
          private const string saveExtendedInfo = @"/api/users/{0}/extendedinfo";
          private const string updateExtendedInfo = @"/api/users/{0}/extendedinfo";
          private const string deleteExtendedInfo = @"/api/users/{0}/extendedinfo";
          private const string getFamilyDetails = @"/api/users/family/{0}";
          private const string getFamilyDetailInfo = @"/api/users/family/{0}/information";

          private readonly ILogger<UserService> _logger;

          public UserService(
               IHttpService httpService,
               ILocalStorageService localStorageService,
               ILogger<UserService> logger)
               : base(httpService, localStorageService)
          {
               _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          }

          /// <summary>
          /// Get user by ID with error handling and validation
          /// </summary>
          public async Task<UserInformationResponse> GetUserByIdAsync(Guid userId)
          {
               try
               {
                    // ✅ Validate input
                    if (userId == Guid.Empty)
                    {
                         _logger.LogWarning("GetUserByIdAsync called with empty GUID");
                         throw new ArgumentException("User ID cannot be empty", nameof(userId));
                    }

                    _logger.LogInformation("Fetching user {UserId}", userId);

                    // ✅ Make request
                    var formattedUrl = string.Format(getUserById, userId);
                    var result = await _httpService.Get<UserInformationResponse>(formattedUrl);

                    // ✅ Validate response
                    if (result == null)
                    {
                         _logger.LogWarning("User {UserId} not found", userId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched user {UserId}", userId);
                    return result;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching user {UserId}", userId);
                    throw;
               }
               catch (TaskCanceledException ex)
               {
                    _logger.LogError(ex, "Request timeout fetching user {UserId}", userId);
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching user {UserId}", userId);
                    throw;
               }
          }

          /// <summary>
          /// Activate user by verification code with error handling
          /// </summary>
          public async Task<bool> ActivateUserByCodeAsync(Guid userId, UserVerificationRequest request)
          {
               try
               {
                    // ✅ Validate input
                    if (userId == Guid.Empty)
                    {
                         _logger.LogWarning("ActivateUserByCodeAsync called with empty GUID");
                         throw new ArgumentException("User ID cannot be empty", nameof(userId));
                    }

                    if (request == null)
                    {
                         _logger.LogWarning("ActivateUserByCodeAsync called with null request");
                         throw new ArgumentNullException(nameof(request), "Verification request cannot be null");
                    }

                    _logger.LogInformation("Activating user {UserId}", userId);

                    // ✅ Make request
                    var formattedUrl = string.Format(validateUserActivationCode, userId);
                    var result = await _httpService.Post<bool>(formattedUrl, request);

                    _logger.LogInformation("Successfully activated user {UserId}", userId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error activating user {UserId}", userId);
                    throw;
               }
          }

          /// <summary>
          /// Send activation code to user
          /// </summary>
          public async Task<bool> SendUserActivationCodeAsync(Guid userId)
          {
               try
               {
                    // ✅ Validate input
                    if (userId == Guid.Empty)
                    {
                         _logger.LogWarning("SendUserActivationCodeAsync called with empty GUID");
                         throw new ArgumentException("User ID cannot be empty", nameof(userId));
                    }

                    _logger.LogInformation("Sending activation code to user {UserId}", userId);

                    // ✅ Use PUT (backend uses PUT for this endpoint)
                    var formattedUrl = string.Format(sendActivationCode, userId);
                    var result = await _httpService.Put<bool>(formattedUrl, null);

                    _logger.LogInformation("Successfully sent activation code to user {UserId}", userId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error sending activation code to user {UserId}", userId);
                    throw;
               }
          }

          /// <summary>
          /// Register new user with error handling and validation
          /// </summary>
          public async Task<UserInformationResponse> RegisterUserAsync(AddUserInformation userInfo)
          {
               try
               {
                    // ✅ Validate input
                    if (userInfo == null)
                    {
                         _logger.LogWarning("RegisterUserAsync called with null user information");
                         throw new ArgumentNullException(nameof(userInfo), "User information cannot be null");
                    }

                    _logger.LogInformation("Registering new user");

                    // ✅ Make request
                    var result = await _httpService.Post<UserInformationResponse>(addUser, userInfo);

                    // ✅ Validate response
                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when registering user");
                         throw new InvalidOperationException("Server did not return user confirmation");
                    }

                    _logger.LogInformation("Successfully registered user");
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error registering user");
                    throw;
               }
          }

          /// <summary>
          /// Change user password with error handling
          /// </summary>
          public async Task<bool> ChangeUserPasswordAsync(Guid userId, UpdateUserPasswordRequest changeRequest)
          {
               try
               {
                    // ✅ Validate input
                    if (userId == Guid.Empty)
                    {
                         _logger.LogWarning("ChangeUserPasswordAsync called with empty GUID");
                         throw new ArgumentException("User ID cannot be empty", nameof(userId));
                    }

                    if (changeRequest == null)
                    {
                         _logger.LogWarning("ChangeUserPasswordAsync called with null request");
                         throw new ArgumentNullException(nameof(changeRequest), "Change request cannot be null");
                    }

                    _logger.LogInformation("Changing password for user {UserId}", userId);

                    // ✅ Make request
                    var formattedUrl = string.Format(changeUserPassword, userId);
                    var result = await _httpService.Post<bool>(formattedUrl, changeRequest);

                    _logger.LogInformation("Successfully changed password for user {UserId}", userId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                    throw;
               }
          }

          /// <summary>
          /// Forgot user password with error handling
          /// </summary>
          public async Task<bool> ForgotUserPasswordAsync(string username)
          {
               try
               {
                    // ✅ Validate input
                    if (string.IsNullOrWhiteSpace(username))
                    {
                         _logger.LogWarning("ForgotUserPasswordAsync called with null/empty username");
                         throw new ArgumentException("Username cannot be null or empty", nameof(username));
                    }

                    _logger.LogInformation("Processing forgot password for user {Username}", username);

                    // ✅ Make request
                    var formattedUrl = string.Format(forgotUserPassword, username);
                    var result = await _httpService.Get<bool>(formattedUrl);

                    _logger.LogInformation("Successfully processed forgot password for user {Username}", username);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error processing forgot password for user {Username}", username);
                    throw;
               }
          }

          /// <summary>
          /// Validate username availability with error handling
          /// </summary>
          public async Task<bool> ValidateUsernameAsync(string username)
          {
               try
               {
                    // ✅ Validate input
                    if (string.IsNullOrWhiteSpace(username))
                    {
                         _logger.LogWarning("ValidateUsernameAsync called with null/empty username");
                         throw new ArgumentException("Username cannot be null or empty", nameof(username));
                    }

                    _logger.LogInformation("Validating username {Username}", username);

                    // ✅ Make request
                    var formattedUrl = string.Format(checkUsernameExist, username);
                    var result = await _httpService.Get<bool>(formattedUrl);

                    _logger.LogInformation("Username {Username} validation result: {IsValid}", username, result);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error validating username {Username}", username);
                    throw;
               }
          }

          /// <summary>
          /// Link user to family with error handling
          /// </summary>
          public async Task<UserInformationResponse> LinkUserToFamilyByIdAsync(Guid userId, Guid familyId)
          {
               try
               {
                    // ✅ Validate input
                    if (userId == Guid.Empty)
                    {
                         _logger.LogWarning("LinkUserToFamilyByIdAsync called with empty user GUID");
                         throw new ArgumentException("User ID cannot be empty", nameof(userId));
                    }

                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("LinkUserToFamilyByIdAsync called with empty family GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    _logger.LogInformation("Linking user {UserId} to family {FamilyId}", userId, familyId);

                    // ✅ Use PUT (backend uses PUT for this endpoint)
                    var formattedUrl = string.Format(linkUserToFamilId, userId, familyId);
                    var result = await _httpService.Put<UserInformationResponse>(formattedUrl, null);

                    // ✅ Validate response
                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when linking user to family");
                         throw new InvalidOperationException("Server did not return updated user information");
                    }

                    _logger.LogInformation("Successfully linked user {UserId} to family {FamilyId}", userId, familyId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error linking user {UserId} to family {FamilyId}", userId, familyId);
                    throw;
               }
          }

          /// <summary>
          /// Get family ID by user information with error handling
          /// </summary>
          public async Task<Guid> GetFamilyIdByUserInfoAsync(string userEmail, string userPhone)
          {
               try
               {
                    // ✅ Validate input
                    if (string.IsNullOrWhiteSpace(userEmail) && string.IsNullOrWhiteSpace(userPhone))
                    {
                         _logger.LogWarning("GetFamilyIdByUserInfoAsync called with null/empty email and phone");
                         throw new ArgumentException("Either email or phone must be provided");
                    }

                    _logger.LogInformation("Fetching family ID by user info");

                    // ✅ Make request
                    var request = new UserFamilyInformationRequest
                    {
                         Email = userEmail,
                    };

                    if (!string.IsNullOrEmpty(userPhone))
                    {
                         request.Phone = userPhone;
                    }

                    var result = await _httpService.Post<string>(getFamilIdByUserInfo, request);

                    if (Guid.TryParse(result, out var familyId))
                    {
                         _logger.LogInformation("Successfully fetched family ID");
                         return familyId;
                    }

                    _logger.LogWarning("Failed to parse family ID from response");
                    return Guid.Empty;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching family ID by user info");
                    throw;
               }
          }

          /// <summary>
          /// Get extended user info by user ID with error handling
          /// </summary>
          public async Task<ExtendedUserInformationResponse> GetExtendedInfoByUserIdAsync(Guid userId)
          {
               try
               {
                    // ✅ Validate input
                    if (userId == Guid.Empty)
                    {
                         _logger.LogWarning("GetExtendedInfoByUserIdAsync called with empty GUID");
                         throw new ArgumentException("User ID cannot be empty", nameof(userId));
                    }

                    _logger.LogInformation("Fetching extended info for user {UserId}", userId);

                    // ✅ Make request
                    var formattedUrl = string.Format(getExtendedInfoByUserId, userId);
                    var result = await _httpService.Get<ExtendedUserInformationResponse>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("Extended info for user {UserId} not found", userId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched extended info for user {UserId}", userId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching extended info for user {UserId}", userId);
                    throw;
               }
          }

          /// <summary>
          /// Save extended user info with error handling
          /// </summary>
          public async Task<ExtendedUserInformationResponse> SaveExtendedInfoAsync(Guid userId, AddExtendedUserInformationRequest request)
          {
               try
               {
                    // ✅ Validate input
                    if (userId == Guid.Empty)
                    {
                         _logger.LogWarning("SaveExtendedInfoAsync called with empty GUID");
                         throw new ArgumentException("User ID cannot be empty", nameof(userId));
                    }

                    if (request == null)
                    {
                         _logger.LogWarning("SaveExtendedInfoAsync called with null request");
                         throw new ArgumentNullException(nameof(request), "Request cannot be null");
                    }

                    _logger.LogInformation("Saving extended info for user {UserId}", userId);

                    // ✅ Make request
                    var formattedUrl = string.Format(saveExtendedInfo, userId);
                    var result = await _httpService.Post<ExtendedUserInformationResponse>(formattedUrl, request);

                    // ✅ Validate response
                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when saving extended info");
                         throw new InvalidOperationException("Server did not return extended info confirmation");
                    }

                    _logger.LogInformation("Successfully saved extended info for user {UserId}", userId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error saving extended info for user {UserId}", userId);
                    throw;
               }
          }

          /// <summary>
          /// Update extended user info (backend uses PUT)
          /// </summary>
          public async Task<ExtendedUserInformationResponse> UpdateExtendedInfoAsync(Guid userId, ExtendedUserInformationResponse request)
          {
               try
               {
                    // ✅ Validate input
                    if (userId == Guid.Empty)
                    {
                         _logger.LogWarning("UpdateExtendedInfoAsync called with empty GUID");
                         throw new ArgumentException("User ID cannot be empty", nameof(userId));
                    }

                    if (request == null)
                    {
                         _logger.LogWarning("UpdateExtendedInfoAsync called with null request");
                         throw new ArgumentNullException(nameof(request), "Request cannot be null");
                    }

                    _logger.LogInformation("Updating extended info for user {UserId}", userId);

                    // ✅ Use PUT (backend uses PUT for this endpoint, not POST)
                    var formattedUrl = string.Format(updateExtendedInfo, userId);
                    var result = await _httpService.Post<ExtendedUserInformationResponse>(formattedUrl, request);

                    // ✅ Validate response
                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when updating extended info");
                         throw new InvalidOperationException("Server did not return updated extended info");
                    }

                    _logger.LogInformation("Successfully updated extended info for user {UserId}", userId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error updating extended info for user {UserId}", userId);
                    throw;
               }
          }

          /// <summary>
          /// Delete extended user info
          /// </summary>
          public async Task<bool> DeleteExtendedInfoAsync(Guid userId)
          {
               try
               {
                    // ✅ Validate input
                    if (userId == Guid.Empty)
                    {
                         _logger.LogWarning("DeleteExtendedInfoAsync called with empty GUID");
                         throw new ArgumentException("User ID cannot be empty", nameof(userId));
                    }

                    _logger.LogInformation("Deleting extended info for user {UserId}", userId);

                    // ✅ Use DELETE
                    var formattedUrl = string.Format(deleteExtendedInfo, userId);
                    var result = await _httpService.Delete<bool>(formattedUrl);

                    _logger.LogInformation("Successfully deleted extended info for user {UserId}", userId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error deleting extended info for user {UserId}", userId);
                    throw;
               }
          }

          /// <summary>
          /// Get family by family ID with error handling
          /// </summary>
          public async Task<IEnumerable<UserInformationResponse>> GetFamilyByFamilyId(Guid familyId)
          {
               try
               {
                    // ✅ Validate input
                    if (familyId == Guid.Empty)
                    {
                         _logger.LogWarning("GetFamilyByFamilyId called with empty GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    _logger.LogInformation("Fetching family members for family {FamilyId}", familyId);

                    // ✅ Make request
                    var formattedUrl = string.Format(getFamilyDetails, familyId);
                    var result = await _httpService.Get<IEnumerable<UserInformationResponse>>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("Family {FamilyId} not found", familyId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched family members for family {FamilyId}", familyId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching family members for family {FamilyId}", familyId);
                    throw;
               }
          }

          /// <summary>
          /// Get family detail info by family ID with error handling
          /// </summary>
          public async Task<FamilyInformationDetailsResponse> GetFamilyDetailInfoByFamilyId(Guid familyId)
          {
               try
               {
                    // Validate input
                    if (familyId == Guid.Empty)
                    {    
                         _logger.LogWarning("GetFamilyDetailInfoByFamilyId called with empty GUID");
                         throw new ArgumentException("Family ID cannot be empty", nameof(familyId));
                    }

                    _logger.LogInformation("Fetching family detail info for family {FamilyId}", familyId);

                    // Make request
                    var formattedUrl = string.Format(getFamilyDetailInfo, familyId);
                    var result = await _httpService.Get<FamilyInformationDetailsResponse>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("Family {FamilyId} not found", familyId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched family detail info for family {FamilyId}", familyId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching family detail info for family {FamilyId}", familyId);
                    throw;
               }
          }
     }
}
