using Maktab.Domain.Services;
using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktab.Domain.Tests.Services
{
     /// <summary>
     /// Comprehensive unit tests for UserService
     /// Covers error handling, validation, and HTTP verb usage
     /// </summary>
     [TestClass]
     public class UserServiceTests
     {
          private Mock<IHttpService> _mockHttpService;
          private Mock<ILocalStorageService> _mockLocalStorageService;
          private Mock<ILogger<UserService>> _mockLogger;
          private UserService _service;

          [TestInitialize]
          public void Setup()
          {
               _mockHttpService = new Mock<IHttpService>();
               _mockLocalStorageService = new Mock<ILocalStorageService>();
               _mockLogger = new Mock<ILogger<UserService>>();
               _service = new UserService(_mockHttpService.Object, _mockLocalStorageService.Object, _mockLogger.Object);
          }

          #region GetUserByIdAsync Tests

          [TestMethod]
          public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
          {
               // Arrange
               var userId = Guid.NewGuid();
               var expectedUser = new UserInformationResponse { };
               _mockHttpService.Setup(x => x.Get<UserInformationResponse>(It.IsAny<string>())).ReturnsAsync(expectedUser);

               // Act
               var result = await _service.GetUserByIdAsync(userId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<UserInformationResponse>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetUserByIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetUserByIdAsync(Guid.Empty);
          }

          [TestMethod]
          public async Task GetUserByIdAsync_WithValidId_ReturnsNull_WhenNotFound()
          {
               // Arrange
               var userId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Get<UserInformationResponse>(It.IsAny<string>())).ReturnsAsync((UserInformationResponse)null);

               // Act
               var result = await _service.GetUserByIdAsync(userId);

               // Assert
               Assert.IsNull(result);
          }

          #endregion

          #region RegisterUserAsync Tests

          [TestMethod]
          public async Task RegisterUserAsync_WithValidData_ReturnsRegisteredUser()
          {
               // Arrange
               var userInfo = new AddUserInformation { };
               var expectedUser = new UserInformationResponse { };
               _mockHttpService.Setup(x => x.Post<UserInformationResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedUser);

               // Act
               var result = await _service.RegisterUserAsync(userInfo);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Post<UserInformationResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task RegisterUserAsync_WithNullData_ThrowsArgumentNullException()
          {
               // Act
               await _service.RegisterUserAsync(null);
          }

          [TestMethod]
          [ExpectedException(typeof(InvalidOperationException))]
          public async Task RegisterUserAsync_WithNullResponse_ThrowsInvalidOperationException()
          {
               // Arrange
               var userInfo = new AddUserInformation { };
               _mockHttpService.Setup(x => x.Post<UserInformationResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync((UserInformationResponse)null);

               // Act
               await _service.RegisterUserAsync(userInfo);
          }

          #endregion

          #region SendUserActivationCodeAsync Tests

          [TestMethod]
          public async Task SendUserActivationCodeAsync_WithValidId_ReturnsBool()
          {
               // Arrange
               var userId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Put<bool>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

               // Act
               var result = await _service.SendUserActivationCodeAsync(userId);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Put<bool>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task SendUserActivationCodeAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.SendUserActivationCodeAsync(Guid.Empty);
          }

          #endregion

          #region ActivateUserByCodeAsync Tests

          [TestMethod]
          public async Task ActivateUserByCodeAsync_WithValidData_ReturnsBool()
          {
               // Arrange
               var userId = Guid.NewGuid();
               var request = new UserVerificationRequest { };
               _mockHttpService.Setup(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

               // Act
               var result = await _service.ActivateUserByCodeAsync(userId, request);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task ActivateUserByCodeAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Arrange
               var request = new UserVerificationRequest { };

               // Act
               await _service.ActivateUserByCodeAsync(Guid.Empty, request);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task ActivateUserByCodeAsync_WithNullRequest_ThrowsArgumentNullException()
          {
               // Arrange
               var userId = Guid.NewGuid();

               // Act
               await _service.ActivateUserByCodeAsync(userId, null);
          }

          #endregion

          #region ChangeUserPasswordAsync Tests

          [TestMethod]
          public async Task ChangeUserPasswordAsync_WithValidData_ReturnsBool()
          {
               // Arrange
               var userId = Guid.NewGuid();
               var request = new UpdateUserPasswordRequest { };
               _mockHttpService.Setup(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

               // Act
               var result = await _service.ChangeUserPasswordAsync(userId, request);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task ChangeUserPasswordAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Arrange
               var request = new UpdateUserPasswordRequest { };

               // Act
               await _service.ChangeUserPasswordAsync(Guid.Empty, request);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task ChangeUserPasswordAsync_WithNullRequest_ThrowsArgumentNullException()
          {
               // Arrange
               var userId = Guid.NewGuid();

               // Act
               await _service.ChangeUserPasswordAsync(userId, null);
          }

          #endregion

          #region LinkUserToFamilyByIdAsync Tests

          [TestMethod]
          public async Task LinkUserToFamilyByIdAsync_WithValidIds_ReturnsUser()
          {
               // Arrange
               var userId = Guid.NewGuid();
               var familyId = Guid.NewGuid();
               var expectedUser = new UserInformationResponse { };
               _mockHttpService.Setup(x => x.Put<UserInformationResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedUser);

               // Act
               var result = await _service.LinkUserToFamilyByIdAsync(userId, familyId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Put<UserInformationResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task LinkUserToFamilyByIdAsync_WithEmptyUserId_ThrowsArgumentException()
          {
               // Arrange
               var familyId = Guid.NewGuid();

               // Act
               await _service.LinkUserToFamilyByIdAsync(Guid.Empty, familyId);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task LinkUserToFamilyByIdAsync_WithEmptyFamilyId_ThrowsArgumentException()
          {
               // Arrange
               var userId = Guid.NewGuid();

               // Act
               await _service.LinkUserToFamilyByIdAsync(userId, Guid.Empty);
          }

          #endregion

          #region UpdateExtendedInfoAsync Tests

          [TestMethod]
          public async Task UpdateExtendedInfoAsync_WithValidData_ReturnsBool()
          {
               // Arrange
               var userId = Guid.NewGuid();
               var request = new ExtendedUserInformationResponse { };
               var expectedResponse = new ExtendedUserInformationResponse { };
               _mockHttpService.Setup(x => x.Put<ExtendedUserInformationResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedResponse);

               // Act
               var result = await _service.UpdateExtendedInfoAsync(userId, request);

               // Assert
               Assert.IsNotNull(result);
               // Verify PUT is used (not POST)
               _mockHttpService.Verify(x => x.Put<ExtendedUserInformationResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
               _mockHttpService.Verify(x => x.Post<ExtendedUserInformationResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task UpdateExtendedInfoAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Arrange
               var request = new ExtendedUserInformationResponse { };

               // Act
               await _service.UpdateExtendedInfoAsync(Guid.Empty, request);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task UpdateExtendedInfoAsync_WithNullRequest_ThrowsArgumentNullException()
          {
               // Arrange
               var userId = Guid.NewGuid();

               // Act
               await _service.UpdateExtendedInfoAsync(userId, null);
          }

          #endregion

          #region DeleteExtendedInfoAsync Tests

          [TestMethod]
          public async Task DeleteExtendedInfoAsync_WithValidId_ReturnsBool()
          {
               // Arrange
               var userId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Delete<bool>(It.IsAny<string>())).ReturnsAsync(true);

               // Act
               var result = await _service.DeleteExtendedInfoAsync(userId);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Delete<bool>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task DeleteExtendedInfoAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.DeleteExtendedInfoAsync(Guid.Empty);
          }

          #endregion

          #region GetExtendedInfoByUserIdAsync Tests

          [TestMethod]
          public async Task GetExtendedInfoByUserIdAsync_WithValidId_ReturnsExtendedInfo()
          {
               // Arrange
               var userId = Guid.NewGuid();
               var expectedInfo = new ExtendedUserInformationResponse { };
               _mockHttpService.Setup(x => x.Get<ExtendedUserInformationResponse>(It.IsAny<string>())).ReturnsAsync(expectedInfo);

               // Act
               var result = await _service.GetExtendedInfoByUserIdAsync(userId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<ExtendedUserInformationResponse>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetExtendedInfoByUserIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetExtendedInfoByUserIdAsync(Guid.Empty);
          }

          #endregion

          #region GetFamilyByFamilyId Tests

          [TestMethod]
          public async Task GetFamilyByFamilyId_WithValidId_ReturnsFamilyUsers()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               var expectedUsers = new List<UserInformationResponse> { new UserInformationResponse { } };
               _mockHttpService.Setup(x => x.Get<IEnumerable<UserInformationResponse>>(It.IsAny<string>())).ReturnsAsync(expectedUsers);

               // Act
               var result = await _service.GetFamilyByFamilyId(familyId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<IEnumerable<UserInformationResponse>>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetFamilyByFamilyId_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetFamilyByFamilyId(Guid.Empty);
          }

          #endregion

          #region ValidateUsernameAsync Tests

          [TestMethod]
          public async Task ValidateUsernameAsync_WithValidUsername_ReturnsBool()
          {
               // Arrange
               var username = "testuser";
               _mockHttpService.Setup(x => x.Get<bool>(It.IsAny<string>())).ReturnsAsync(true);

               // Act
               var result = await _service.ValidateUsernameAsync(username);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Get<bool>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task ValidateUsernameAsync_WithNullUsername_ThrowsArgumentException()
          {
               // Act
               await _service.ValidateUsernameAsync(null);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task ValidateUsernameAsync_WithEmptyUsername_ThrowsArgumentException()
          {
               // Act
               await _service.ValidateUsernameAsync(string.Empty);
          }

          #endregion

          #region ForgotUserPasswordAsync Tests

          [TestMethod]
          public async Task ForgotUserPasswordAsync_WithValidUsername_ReturnsBool()
          {
               // Arrange
               var username = "testuser";
               _mockHttpService.Setup(x => x.Get<bool>(It.IsAny<string>())).ReturnsAsync(true);

               // Act
               var result = await _service.ForgotUserPasswordAsync(username);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Get<bool>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task ForgotUserPasswordAsync_WithNullUsername_ThrowsArgumentException()
          {
               // Act
               await _service.ForgotUserPasswordAsync(null);
          }

          #endregion

          #region SaveExtendedInfoAsync Tests

          [TestMethod]
          public async Task SaveExtendedInfoAsync_WithValidData_ReturnsExtendedInfo()
          {
               // Arrange
               var userId = Guid.NewGuid();
               var request = new AddExtendedUserInformationRequest { };
               var expectedInfo = new ExtendedUserInformationResponse { };
               _mockHttpService.Setup(x => x.Post<ExtendedUserInformationResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedInfo);

               // Act
               var result = await _service.SaveExtendedInfoAsync(userId, request);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Post<ExtendedUserInformationResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task SaveExtendedInfoAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Arrange
               var request = new AddExtendedUserInformationRequest { };

               // Act
               await _service.SaveExtendedInfoAsync(Guid.Empty, request);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task SaveExtendedInfoAsync_WithNullRequest_ThrowsArgumentNullException()
          {
               // Arrange
               var userId = Guid.NewGuid();

               // Act
               await _service.SaveExtendedInfoAsync(userId, null);
          }

          #endregion
     }
}
