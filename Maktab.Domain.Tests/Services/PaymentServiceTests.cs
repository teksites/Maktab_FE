using Maktab.Domain.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktab.Domain.Tests.Services
{
     /// <summary>
     /// Comprehensive unit tests for PaymentService
     /// Covers error handling, validation, and RESTful HTTP verb usage
     /// </summary>
     [TestClass]
     public class PaymentServiceTests
     {
          private Mock<IHttpService> _mockHttpService;
          private Mock<ILocalStorageService> _mockLocalStorageService;
          private Mock<ILogger<PaymentService>> _mockLogger;
          private PaymentService _service;

          [TestInitialize]
          public void Setup()
          {
               _mockHttpService = new Mock<IHttpService>();
               _mockLocalStorageService = new Mock<ILocalStorageService>();
               _mockLogger = new Mock<ILogger<PaymentService>>();
               _service = new PaymentService(_mockHttpService.Object, _mockLocalStorageService.Object, _mockLogger.Object);
          }

          #region GetPaymentByIdAsync Tests

          [TestMethod]
          public async Task GetPaymentByIdAsync_WithValidId_ReturnsPayment()
          {
               // Arrange
               var paymentId = Guid.NewGuid();
               var expectedPayment = new CoursePaymentResponse { };
               _mockHttpService.Setup(x => x.Get<CoursePaymentResponse>(It.IsAny<string>())).ReturnsAsync(expectedPayment);

               // Act
               var result = await _service.GetPaymentByIdAsync(paymentId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<CoursePaymentResponse>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetPaymentByIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetPaymentByIdAsync(Guid.Empty);
          }

          #endregion

          #region AddtPaymentAsync Tests

          [TestMethod]
          public async Task AddtPaymentAsync_WithValidData_ReturnsResponse()
          {
               // Arrange
               var transaction = new AddStudentCourseTransaction { };
               var expectedResponse = new StudentCourseTransactionResponse { };
               _mockHttpService.Setup(x => x.Post<StudentCourseTransactionResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedResponse);

               // Act
               var result = await _service.AddtPaymentAsync(transaction);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Post<StudentCourseTransactionResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task AddtPaymentAsync_WithNullTransaction_ThrowsArgumentNullException()
          {
               // Act
               await _service.AddtPaymentAsync(null);
          }

          #endregion

          #region UpdatePaymentByIdAsync Tests

          [TestMethod]
          public async Task UpdatePaymentByIdAsync_WithValidData_ReturnsPayment()
          {
               // Arrange
               var paymentId = Guid.NewGuid();
               var payment = new CoursePaymentResponse { };
               var expectedPayment = new CoursePaymentResponse { };
               _mockHttpService.Setup(x => x.Put<CoursePaymentResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedPayment);

               // Act
               var result = await _service.UpdatePaymentByIdAsync(paymentId, payment);

               // Assert
               Assert.IsNotNull(result);
               // Verify PUT is used (RESTful)
               _mockHttpService.Verify(x => x.Put<CoursePaymentResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task UpdatePaymentByIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Arrange
               var payment = new CoursePaymentResponse { };

               // Act
               await _service.UpdatePaymentByIdAsync(Guid.Empty, payment);
          }

          #endregion

          #region RemovePaymentByIdAsync Tests

          [TestMethod]
          public async Task RemovePaymentByIdAsync_WithValidId_ReturnsBool()
          {
               // Arrange
               var paymentId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Delete<bool>(It.IsAny<string>())).ReturnsAsync(true);

               // Act
               var result = await _service.RemovePaymentByIdAsync(paymentId);

               // Assert
               Assert.IsTrue(result);
               // Verify DELETE is used (RESTful)
               _mockHttpService.Verify(x => x.Delete<bool>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task RemovePaymentByIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.RemovePaymentByIdAsync(Guid.Empty);
          }

          #endregion

          #region GetPaymentByCourseIdAsync Tests

          [TestMethod]
          public async Task GetPaymentByCourseIdAsync_WithValidId_ReturnsPayments()
          {
               // Arrange
               var courseId = Guid.NewGuid();
               var expectedPayments = new List<CoursePaymentResponse> { new CoursePaymentResponse { } };
               _mockHttpService.Setup(x => x.Get<IEnumerable<CoursePaymentResponse>>(It.IsAny<string>())).ReturnsAsync(expectedPayments);

               // Act
               var result = await _service.GetPaymentByCourseIdAsync(courseId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<IEnumerable<CoursePaymentResponse>>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetPaymentByCourseIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetPaymentByCourseIdAsync(Guid.Empty);
          }

          #endregion

          #region GetPaymentByStudentTransactionsIdIdAsync Tests

          [TestMethod]
          public async Task GetPaymentByStudentTransactionsIdIdAsync_WithValidId_ReturnsPayments()
          {
               // Arrange
               var transactionId = Guid.NewGuid();
               var expectedPayments = new List<CoursePaymentResponse> { new CoursePaymentResponse { } };
               _mockHttpService.Setup(x => x.Get<IEnumerable<CoursePaymentResponse>>(It.IsAny<string>())).ReturnsAsync(expectedPayments);

               // Act
               var result = await _service.GetPaymentByStudentTransactionsIdIdAsync(transactionId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<IEnumerable<CoursePaymentResponse>>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetPaymentByStudentTransactionsIdIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetPaymentByStudentTransactionsIdIdAsync(Guid.Empty);
          }

          #endregion
     }
}
