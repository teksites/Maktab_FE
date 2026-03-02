using Maktab.Domain.Services;
using MaktabDataContracts.Models;
using MaktabDataContracts.Requests.Children;
using MaktabDataContracts.Responses.Children;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktab.Domain.Tests.Services
{
     /// <summary>
     /// Comprehensive unit tests for ChildrenService
     /// Covers error handling, validation, and MaktabApiResult wrapper unwrapping
     /// </summary>
     [TestClass]
     public class ChildrenServiceTests
     {
          private Mock<IHttpService> _mockHttpService;
          private Mock<ILocalStorageService> _mockLocalStorageService;
          private Mock<ILogger<ChildrenService>> _mockLogger;
          private ChildrenService _service;

          [TestInitialize]
          public void Setup()
          {
               _mockHttpService = new Mock<IHttpService>();
               _mockLocalStorageService = new Mock<ILocalStorageService>();
               _mockLogger = new Mock<ILogger<ChildrenService>>();
               _service = new ChildrenService(_mockHttpService.Object, _mockLocalStorageService.Object, _mockLogger.Object);
          }

          #region GetChildByIdAsync Tests

          [TestMethod]
          public async Task GetChildByIdAsync_WithValidId_ReturnsChild()
          {
               // Arrange
               var childId = Guid.NewGuid();
               var expectedChild = new ChildResponse { };
               var wrappedResponse = new MaktabApiResult<ChildResponse> { Result = expectedChild };
               _mockHttpService.Setup(x => x.Get<MaktabApiResult<ChildResponse>>(It.IsAny<string>())).ReturnsAsync(wrappedResponse);

               // Act
               var result = await _service.GetChildByIdAsync(childId);

               // Assert
               Assert.IsNotNull(result);
               Assert.AreEqual(expectedChild, result);
               _mockHttpService.Verify(x => x.Get<MaktabApiResult<ChildResponse>>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetChildByIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetChildByIdAsync(Guid.Empty);
          }

          [TestMethod]
          public async Task GetChildByIdAsync_WithValidId_ReturnsNull_WhenNotFound()
          {
               // Arrange
               var childId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Get<MaktabApiResult<ChildResponse>>(It.IsAny<string>())).ReturnsAsync((MaktabApiResult<ChildResponse>)null);

               // Act
               var result = await _service.GetChildByIdAsync(childId);

               // Assert
               Assert.IsNull(result);
          }

          #endregion

          #region GetChildrenByFamilyIdAsync Tests

          [TestMethod]
          public async Task GetChildrenByFamilyIdAsync_WithValidId_ReturnsChildren()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               var expectedChildren = new List<MaktabApiResult<ChildResponse>>
               {
                    new MaktabApiResult<ChildResponse> { Result = new ChildResponse { } },
                    new MaktabApiResult<ChildResponse> { Result = new ChildResponse { } }
               };
               _mockHttpService.Setup(x => x.Get<List<MaktabApiResult<ChildResponse>>>(It.IsAny<string>())).ReturnsAsync(expectedChildren);

               // Act
               var result = await _service.GetChildrenByFamilyIdAsync(familyId);

               // Assert
               Assert.IsNotNull(result);
               var resultList = new List<ChildResponse>(result);
               Assert.AreEqual(2, resultList.Count);
               _mockHttpService.Verify(x => x.Get<List<MaktabApiResult<ChildResponse>>>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetChildrenByFamilyIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetChildrenByFamilyIdAsync(Guid.Empty);
          }

          [TestMethod]
          public async Task GetChildrenByFamilyIdAsync_WithValidId_ReturnsEmpty_WhenNotFound()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Get<List<MaktabApiResult<ChildResponse>>>(It.IsAny<string>())).ReturnsAsync((List<MaktabApiResult<ChildResponse>>)null);

               // Act
               var result = await _service.GetChildrenByFamilyIdAsync(familyId);

               // Assert
               Assert.IsNotNull(result);
               var resultList = new List<ChildResponse>(result);
               Assert.AreEqual(0, resultList.Count);
          }

          #endregion

          #region AddChildToFamilyAsync Tests

          [TestMethod]
          public async Task AddChildToFamilyAsync_WithValidData_ReturnsChild()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               var request = new AddChildRequest { };
               var expectedChild = new ChildResponse { };
               var wrappedResponse = new MaktabApiResult<ChildResponse> { Result = expectedChild };
               _mockHttpService.Setup(x => x.Post<MaktabApiResult<ChildResponse>>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(wrappedResponse);

               // Act
               var result = await _service.AddChildToFamilyAsync(familyId, request);

               // Assert
               Assert.IsNotNull(result);
               Assert.AreEqual(expectedChild, result);
               _mockHttpService.Verify(x => x.Post<MaktabApiResult<ChildResponse>>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task AddChildToFamilyAsync_WithEmptyFamilyId_ThrowsArgumentException()
          {
               // Arrange
               var request = new AddChildRequest { };

               // Act
               await _service.AddChildToFamilyAsync(Guid.Empty, request);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task AddChildToFamilyAsync_WithNullRequest_ThrowsArgumentNullException()
          {
               // Arrange
               var familyId = Guid.NewGuid();

               // Act
               await _service.AddChildToFamilyAsync(familyId, null);
          }

          [TestMethod]
          [ExpectedException(typeof(InvalidOperationException))]
          public async Task AddChildToFamilyAsync_WithNullResponse_ThrowsInvalidOperationException()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               var request = new AddChildRequest { };
               _mockHttpService.Setup(x => x.Post<MaktabApiResult<ChildResponse>>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync((MaktabApiResult<ChildResponse>)null);

               // Act
               await _service.AddChildToFamilyAsync(familyId, request);
          }

          #endregion

          #region IsChildExistWithRamQNumberAsync Tests

          [TestMethod]
          public async Task IsChildExistWithRamQNumberAsync_WithValidData_ReturnsBool()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               var ramqNumber = "1234567890";
               _mockHttpService.Setup(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

               // Act
               var result = await _service.IsChildExistWithRamQNumberAsync(familyId, ramqNumber);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task IsChildExistWithRamQNumberAsync_WithEmptyFamilyId_ThrowsArgumentException()
          {
               // Act
               await _service.IsChildExistWithRamQNumberAsync(Guid.Empty, "1234567890");
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task IsChildExistWithRamQNumberAsync_WithNullRamqNumber_ThrowsArgumentException()
          {
               // Arrange
               var familyId = Guid.NewGuid();

               // Act
               await _service.IsChildExistWithRamQNumberAsync(familyId, null);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task IsChildExistWithRamQNumberAsync_WithEmptyRamqNumber_ThrowsArgumentException()
          {
               // Arrange
               var familyId = Guid.NewGuid();

               // Act
               await _service.IsChildExistWithRamQNumberAsync(familyId, string.Empty);
          }

          #endregion

          #region RemoveChildByIdAsync Tests

          [TestMethod]
          public async Task RemoveChildByIdAsync_WithValidId_ReturnsBool()
          {
               // Arrange
               var childId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

               // Act
               var result = await _service.RemoveChildByIdAsync(childId);

               // Assert
               Assert.IsTrue(result);
               // Verify POST is used (backend uses POST, not DELETE)
               _mockHttpService.Verify(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
               _mockHttpService.Verify(x => x.Delete<bool>(It.IsAny<string>()), Times.Never);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task RemoveChildByIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.RemoveChildByIdAsync(Guid.Empty);
          }

          #endregion

          #region RemoveChildFromFamilyAsync Tests

          [TestMethod]
          public async Task RemoveChildFromFamilyAsync_WithValidId_ReturnsBool()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

               // Act
               var result = await _service.RemoveChildFromFamilyAsync(familyId);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task RemoveChildFromFamilyAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.RemoveChildFromFamilyAsync(Guid.Empty);
          }

          #endregion
     }
}
