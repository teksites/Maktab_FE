using Maktab.Domain.Services;
using MaktabDataContracts.Requests.OtherContacts;
using MaktabDataContracts.Responses.OtherContacts;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktab.Domain.Tests.Services
{
     /// <summary>
     /// Comprehensive unit tests for ContactService
     /// Covers error handling, validation, and HTTP verb usage (POST for delete/update)
     /// </summary>
     [TestClass]
     public class ContactServiceTests
     {
          private Mock<IHttpService> _mockHttpService;
          private Mock<ILocalStorageService> _mockLocalStorageService;
          private Mock<ILogger<ContactService>> _mockLogger;
          private ContactService _service;

          [TestInitialize]
          public void Setup()
          {
               _mockHttpService = new Mock<IHttpService>();
               _mockLocalStorageService = new Mock<ILocalStorageService>();
               _mockLogger = new Mock<ILogger<ContactService>>();
               _service = new ContactService(_mockHttpService.Object, _mockLocalStorageService.Object, _mockLogger.Object);
          }

          #region GetContactById Tests

          [TestMethod]
          public async Task GetContactById_WithValidId_ReturnsContact()
          {
               // Arrange
               var contactId = Guid.NewGuid();
               var expectedContact = new OtherContactResponse { };
               _mockHttpService.Setup(x => x.Get<OtherContactResponse>(It.IsAny<string>())).ReturnsAsync(expectedContact);

               // Act
               var result = await _service.GetContactById(contactId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<OtherContactResponse>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetContactById_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetContactById(Guid.Empty);
          }

          [TestMethod]
          public async Task GetContactById_WithValidId_ReturnsNull_WhenNotFound()
          {
               // Arrange
               var contactId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Get<OtherContactResponse>(It.IsAny<string>())).ReturnsAsync((OtherContactResponse)null);

               // Act
               var result = await _service.GetContactById(contactId);

               // Assert
               Assert.IsNull(result);
          }

          #endregion

          #region GetContactsByFamilyId Tests

          [TestMethod]
          public async Task GetContactsByFamilyId_WithValidId_ReturnsContacts()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               var expectedContacts = new List<OtherContactResponse> { new OtherContactResponse { } };
               _mockHttpService.Setup(x => x.Get<IList<OtherContactResponse>>(It.IsAny<string>())).ReturnsAsync(expectedContacts);

               // Act
               var result = await _service.GetContactsByFamilyId(familyId);

               // Assert
               Assert.IsNotNull(result);
               Assert.AreEqual(1, result.Count);
               _mockHttpService.Verify(x => x.Get<IList<OtherContactResponse>>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetContactsByFamilyId_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetContactsByFamilyId(Guid.Empty);
          }

          [TestMethod]
          public async Task GetContactsByFamilyId_WithValidId_ReturnsEmptyList_WhenNotFound()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Get<IList<OtherContactResponse>>(It.IsAny<string>())).ReturnsAsync((IList<OtherContactResponse>)null);

               // Act
               var result = await _service.GetContactsByFamilyId(familyId);

               // Assert
               Assert.IsNotNull(result);
               Assert.AreEqual(0, result.Count);
          }

          #endregion

          #region SaveContactAsync Tests

          [TestMethod]
          public async Task SaveContactAsync_WithValidData_ReturnsContact()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               var contact = new AddOtherContact { };
               var expectedContact = new OtherContactResponse { };
               _mockHttpService.Setup(x => x.Post<OtherContactResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedContact);

               // Act
               var result = await _service.SaveContactAsync(familyId, contact);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Post<OtherContactResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task SaveContactAsync_WithEmptyFamilyId_ThrowsArgumentException()
          {
               // Arrange
               var contact = new AddOtherContact { };

               // Act
               await _service.SaveContactAsync(Guid.Empty, contact);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task SaveContactAsync_WithNullContact_ThrowsArgumentNullException()
          {
               // Arrange
               var familyId = Guid.NewGuid();

               // Act
               await _service.SaveContactAsync(familyId, null);
          }

          [TestMethod]
          [ExpectedException(typeof(InvalidOperationException))]
          public async Task SaveContactAsync_WithNullResponse_ThrowsInvalidOperationException()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               var contact = new AddOtherContact { };
               _mockHttpService.Setup(x => x.Post<OtherContactResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync((OtherContactResponse)null);

               // Act
               await _service.SaveContactAsync(familyId, contact);
          }

          #endregion

          #region UpdateContactAsync Tests

          [TestMethod]
          public async Task UpdateContactAsync_WithValidData_ReturnsContact()
          {
               // Arrange
               var contact = new OtherContactResponse { };
               var expectedContact = new OtherContactResponse { };
               _mockHttpService.Setup(x => x.Post<OtherContactResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedContact);

               // Act
               var result = await _service.UpdateContactAsync(contact);

               // Assert
               Assert.IsNotNull(result);
               // Verify POST is used (backend uses POST, not PUT)
               _mockHttpService.Verify(x => x.Post<OtherContactResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task UpdateContactAsync_WithNullContact_ThrowsArgumentNullException()
          {
               // Act
               await _service.UpdateContactAsync(null);
          }

          [TestMethod]
          [ExpectedException(typeof(InvalidOperationException))]
          public async Task UpdateContactAsync_WithNullResponse_ThrowsInvalidOperationException()
          {
               // Arrange
               var contact = new OtherContactResponse { };
               _mockHttpService.Setup(x => x.Post<OtherContactResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync((OtherContactResponse)null);

               // Act
               await _service.UpdateContactAsync(contact);
          }

          #endregion

          #region DeleteContactById Tests

          [TestMethod]
          public async Task DeleteContactById_WithValidId_ReturnsBool()
          {
               // Arrange
               var contactId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

               // Act
               var result = await _service.DeleteContactById(contactId);

               // Assert
               Assert.IsTrue(result);
               // Verify POST is used (backend uses POST, not DELETE)
               _mockHttpService.Verify(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
               _mockHttpService.Verify(x => x.Delete<bool>(It.IsAny<string>()), Times.Never);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task DeleteContactById_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.DeleteContactById(Guid.Empty);
          }

          #endregion

          #region DeleteContactByFamilyId Tests

          [TestMethod]
          public async Task DeleteContactByFamilyId_WithValidId_ReturnsBool()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

               // Act
               var result = await _service.DeleteContactByFamilyId(familyId);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task DeleteContactByFamilyId_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.DeleteContactByFamilyId(Guid.Empty);
          }

          #endregion

          #region HasContactAddedForFamily Tests

          [TestMethod]
          public async Task HasContactAddedForFamily_WithValidId_ReturnsBool()
          {
               // Arrange
               var familyId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

               // Act
               var result = await _service.HasContactAddedForFamily(familyId);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Post<bool>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task HasContactAddedForFamily_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.HasContactAddedForFamily(Guid.Empty);
          }

          #endregion
     }
}
