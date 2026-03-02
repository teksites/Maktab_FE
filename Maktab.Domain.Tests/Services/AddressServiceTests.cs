using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Maktab.Core.Interfaces.Services;
using Maktab.Domain.Services;
using MaktabDataContracts.Requests.Addresses;
using MaktabDataContracts.Responses.Addresses;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Maktab.Domain.Tests.Services
{
     /// <summary>
     /// Unit tests for AddressService with error handling and validation testing.
     /// </summary>
     [TestClass]
     public class AddressServiceTests
     {
          private Mock<IHttpService> _mockHttpService;
          private Mock<ILocalStorageService> _mockLocalStorage;
          private Mock<ILogger<AddressService>> _mockLogger;
          private AddressService _service;

          [TestInitialize]
          public void Setup()
          {
               _mockHttpService = new Mock<IHttpService>();
               _mockLocalStorage = new Mock<ILocalStorageService>();
               _mockLogger = new Mock<ILogger<AddressService>>();

               _service = new AddressService(
                    _mockHttpService.Object,
                    _mockLocalStorage.Object,
                    _mockLogger.Object);
          }

          #region GetAddressById Tests

          [TestMethod]
          public async Task GetAddressById_WithValidId_ReturnsAddress()
          {
               // Arrange
               var addressId = Guid.NewGuid();
               var expectedAddress = new AddressResponse { /* populate with properties */ };

               _mockHttpService
                    .Setup(x => x.Get<AddressResponse>(It.IsAny<string>()))
                    .ReturnsAsync(expectedAddress);

               // Act
               var result = await _service.GetAddressById(addressId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<AddressResponse>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetAddressById_WithEmptyId_ThrowsException()
          {
               // Arrange
               var emptyId = Guid.Empty;

               // Act
               await _service.GetAddressById(emptyId);

               // Assert - Exception expected
          }

          [TestMethod]
          public async Task GetAddressById_WithNotFound_ReturnsNull()
          {
               // Arrange
               var addressId = Guid.NewGuid();
               _mockHttpService
                    .Setup(x => x.Get<AddressResponse>(It.IsAny<string>()))
                    .ReturnsAsync((AddressResponse)null);

               // Act
               var result = await _service.GetAddressById(addressId);

               // Assert
               Assert.IsNull(result);
          }

          #endregion

          #region SaveAddressAsync Tests

          [TestMethod]
          public async Task SaveAddressAsync_WithValidAddress_ReturnsSavedAddress()
          {
               // Arrange
               var addressToSave = new AddAddress { /* populate with properties */ };
               var savedAddress = new AddressResponse { /* populate with properties */ };

               _mockHttpService
                    .Setup(x => x.Post<AddressResponse>(It.IsAny<string>(), It.IsAny<AddAddress>()))
                    .ReturnsAsync(savedAddress);

               // Act
               var result = await _service.SaveAddressAsync(addressToSave);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(
                    x => x.Post<AddressResponse>(It.IsAny<string>(), addressToSave),
                    Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task SaveAddressAsync_WithNullAddress_ThrowsException()
          {
               // Act
               await _service.SaveAddressAsync(null);

               // Assert - Exception expected
          }

          [TestMethod]
          [ExpectedException(typeof(InvalidOperationException))]
          public async Task SaveAddressAsync_WithNullServerResponse_ThrowsException()
          {
               // Arrange
               var addressToSave = new AddAddress { /* populate with properties */ };

               _mockHttpService
                    .Setup(x => x.Post<AddressResponse>(It.IsAny<string>(), It.IsAny<AddAddress>()))
                    .ReturnsAsync((AddressResponse)null);

               // Act
               await _service.SaveAddressAsync(addressToSave);

               // Assert - Exception expected
          }

          #endregion

          #region UpdateAddressAsync Tests

          [TestMethod]
          public async Task UpdateAddressAsync_WithValidAddress_ReturnsUpdatedAddress()
          {
               // Arrange
               var addressToUpdate = new AddressResponse { /* populate with properties */ };

               _mockHttpService
                    .Setup(x => x.Put<AddressResponse>(It.IsAny<string>(), It.IsAny<AddressResponse>()))
                    .ReturnsAsync(addressToUpdate);

               // Act
               var result = await _service.UpdateAddressAsync(addressToUpdate);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Put<AddressResponse>(It.IsAny<string>(), addressToUpdate), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task UpdateAddressAsync_WithNullAddress_ThrowsException()
          {
               // Act
               await _service.UpdateAddressAsync(null);

               // Assert - Exception expected
          }

          [TestMethod]
          [ExpectedException(typeof(InvalidOperationException))]
          public async Task UpdateAddressAsync_WithNullServerResponse_ThrowsException()
          {
               // Arrange
               var addressToUpdate = new AddressResponse { /* populate with properties */ };

               _mockHttpService
                    .Setup(x => x.Put<AddressResponse>(It.IsAny<string>(), It.IsAny<AddressResponse>()))
                    .ReturnsAsync((AddressResponse)null);

               // Act
               await _service.UpdateAddressAsync(addressToUpdate);

               // Assert - Exception expected
          }

          #endregion

          #region DeleteAddressById Tests

          [TestMethod]
          public async Task DeleteAddressById_WithValidId_ReturnsTrue()
          {
               // Arrange
               var addressId = Guid.NewGuid();
               _mockHttpService
                    .Setup(x => x.Delete<bool>(It.IsAny<string>()))
                    .ReturnsAsync(true);

               // Act
               var result = await _service.DeleteAddressById(addressId);

               // Assert
               Assert.IsTrue(result);
               _mockHttpService.Verify(x => x.Delete<bool>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task DeleteAddressById_WithEmptyId_ThrowsException()
          {
               // Act
               await _service.DeleteAddressById(Guid.Empty);

               // Assert - Exception expected
          }

          #endregion

          #region GetAddressByConnectedId Tests

          [TestMethod]
          public async Task GetAddressByConnectedId_WithValidId_ReturnsAddress()
          {
               // Arrange
               var connectedId = Guid.NewGuid();
               var expectedAddress = new AddressResponse { /* populate with properties */ };

               _mockHttpService
                    .Setup(x => x.Get<AddressResponse>(It.IsAny<string>()))
                    .ReturnsAsync(expectedAddress);

               // Act
               var result = await _service.GetAddressByConnectedId(connectedId);

               // Assert
               Assert.IsNotNull(result);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetAddressByConnectedId_WithEmptyId_ThrowsException()
          {
               // Act
               await _service.GetAddressByConnectedId(Guid.Empty);

               // Assert - Exception expected
          }

          #endregion

          #region DeleteAddressByConnectedId Tests

          [TestMethod]
          public async Task DeleteAddressByConnectedId_WithValidId_ReturnsTrue()
          {
               // Arrange
               var connectedId = Guid.NewGuid();
               _mockHttpService
                    .Setup(x => x.Delete<bool>(It.IsAny<string>()))
                    .ReturnsAsync(true);

               // Act
               var result = await _service.DeleteAddressByConnectedId(connectedId);

               // Assert
               Assert.IsTrue(result);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task DeleteAddressByConnectedId_WithEmptyId_ThrowsException()
          {
               // Act
               await _service.DeleteAddressByConnectedId(Guid.Empty);

               // Assert - Exception expected
          }

          #endregion

          #region Integration Tests

          [TestMethod]
          public async Task FullWorkflow_GetUpdateDelete_Succeeds()
          {
               // Arrange
               var addressId = Guid.NewGuid();
               var address = new AddressResponse { /* populate with properties */ };
               var updatedAddress = new AddressResponse { /* populate with properties */ };

               _mockHttpService
                    .Setup(x => x.Get<AddressResponse>(It.IsAny<string>()))
                    .ReturnsAsync(address);

               _mockHttpService
                    .Setup(x => x.Put<AddressResponse>(It.IsAny<string>(), It.IsAny<AddressResponse>()))
                    .ReturnsAsync(updatedAddress);

               _mockHttpService
                    .Setup(x => x.Delete<bool>(It.IsAny<string>()))
                    .ReturnsAsync(true);

               // Act
               var retrieved = await _service.GetAddressById(addressId);
               var updated = await _service.UpdateAddressAsync(retrieved);
               var deleted = await _service.DeleteAddressById(addressId);

               // Assert
               Assert.IsNotNull(retrieved);
               Assert.IsNotNull(updated);
               Assert.IsTrue(deleted);
          }

          #endregion
     }
}
