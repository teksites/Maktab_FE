using Maktab.Domain.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktab.Domain.Tests.Services
{
     /// <summary>
     /// Comprehensive unit tests for CourseService
     /// Covers error handling, validation, and HTTP verb usage (PUT/DELETE RESTful)
     /// </summary>
     [TestClass]
     public class CourseServiceTests
     {
          private Mock<IHttpService> _mockHttpService;
          private Mock<ILocalStorageService> _mockLocalStorageService;
          private Mock<ILogger<CourseService>> _mockLogger;
          private CourseService _service;

          [TestInitialize]
          public void Setup()
          {
               _mockHttpService = new Mock<IHttpService>();
               _mockLocalStorageService = new Mock<ILocalStorageService>();
               _mockLogger = new Mock<ILogger<CourseService>>();
               _service = new CourseService(_mockHttpService.Object, _mockLocalStorageService.Object, _mockLogger.Object);
          }

          #region GetCourseByIdAsync Tests

          [TestMethod]
          public async Task GetCourseByIdAsync_WithValidId_ReturnsCourse()
          {
               // Arrange
               var courseId = Guid.NewGuid();
               var expectedCourse = new CourseResponseDetailed { };
               _mockHttpService.Setup(x => x.Get<CourseResponseDetailed>(It.IsAny<string>())).ReturnsAsync(expectedCourse);

               // Act
               var result = await _service.GetCourseByIdAsync(courseId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<CourseResponseDetailed>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetCourseByIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetCourseByIdAsync(Guid.Empty);
          }

          #endregion

          #region AddCourseAsync Tests

          [TestMethod]
          public async Task AddCourseAsync_WithValidData_ReturnsCourse()
          {
               // Arrange
               var course = new AddCourse { };
               var expectedCourse = new CourseResponseDetailed { };
               _mockHttpService.Setup(x => x.Post<CourseResponseDetailed>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedCourse);

               // Act
               var result = await _service.AddCourseAsync(course);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Post<CourseResponseDetailed>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task AddCourseAsync_WithNullCourse_ThrowsArgumentNullException()
          {
               // Act
               await _service.AddCourseAsync(null);
          }

          #endregion

          #region UpdateCourseAsync Tests

          [TestMethod]
          public async Task UpdateCourseAsync_WithValidData_ReturnsCourse()
          {
               // Arrange
               var courseId = Guid.NewGuid();
               var course = new CourseResponseDetailed { };
               var expectedCourse = new CourseResponseDetailed { };
               _mockHttpService.Setup(x => x.Put<CourseResponseDetailed>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedCourse);

               // Act
               var result = await _service.UpdateCourseAsync(courseId, course);

               // Assert
               Assert.IsNotNull(result);
               // Verify PUT is used (RESTful)
               _mockHttpService.Verify(x => x.Put<CourseResponseDetailed>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task UpdateCourseAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Arrange
               var course = new CourseResponseDetailed { };

               // Act
               await _service.UpdateCourseAsync(Guid.Empty, course);
          }

          #endregion

          #region RemoveCourseAsync Tests

          [TestMethod]
          public async Task RemoveCourseAsync_WithValidId_ReturnsBool()
          {
               // Arrange
               var courseId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Delete<bool>(It.IsAny<string>())).ReturnsAsync(true);

               // Act
               var result = await _service.RemoveCourseAsync(courseId);

               // Assert
               Assert.IsTrue(result);
               // Verify DELETE is used (RESTful)
               _mockHttpService.Verify(x => x.Delete<bool>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task RemoveCourseAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.RemoveCourseAsync(Guid.Empty);
          }

          #endregion

          #region GetCourseGroupsByIdAsync Tests

          [TestMethod]
          public async Task GetCourseGroupsByIdAsync_WithValidId_ReturnsGroup()
          {
               // Arrange
               var groupId = Guid.NewGuid();
               var expectedGroup = new CourseEnrollmentGroupResponse { };
               _mockHttpService.Setup(x => x.Get<CourseEnrollmentGroupResponse>(It.IsAny<string>())).ReturnsAsync(expectedGroup);

               // Act
               var result = await _service.GetCourseGroupsByIdAsync(groupId);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Get<CourseEnrollmentGroupResponse>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task GetCourseGroupsByIdAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.GetCourseGroupsByIdAsync(Guid.Empty);
          }

          #endregion

          #region AddCourseGroupAsync Tests

          [TestMethod]
          public async Task AddCourseGroupAsync_WithValidData_ReturnsGroup()
          {
               // Arrange
               var group = new AddCourseEnrollmentGroup { };
               var expectedGroup = new CourseEnrollmentGroupResponse { };
               _mockHttpService.Setup(x => x.Post<CourseEnrollmentGroupResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedGroup);

               // Act
               var result = await _service.AddCourseGroupAsync(group);

               // Assert
               Assert.IsNotNull(result);
               _mockHttpService.Verify(x => x.Post<CourseEnrollmentGroupResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentNullException))]
          public async Task AddCourseGroupAsync_WithNullGroup_ThrowsArgumentNullException()
          {
               // Act
               await _service.AddCourseGroupAsync(null);
          }

          #endregion

          #region UpdateCourseGroupAsync Tests

          [TestMethod]
          public async Task UpdateCourseGroupAsync_WithValidData_ReturnsGroup()
          {
               // Arrange
               var groupId = Guid.NewGuid();
               var group = new CourseEnrollmentGroupResponse { };
               var expectedGroup = new CourseEnrollmentGroupResponse { };
               _mockHttpService.Setup(x => x.Put<CourseEnrollmentGroupResponse>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(expectedGroup);

               // Act
               var result = await _service.UpdateCourseGroupAsync(groupId, group);

               // Assert
               Assert.IsNotNull(result);
               // Verify PUT is used (RESTful)
               _mockHttpService.Verify(x => x.Put<CourseEnrollmentGroupResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task UpdateCourseGroupAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Arrange
               var group = new CourseEnrollmentGroupResponse { };

               // Act
               await _service.UpdateCourseGroupAsync(Guid.Empty, group);
          }

          #endregion

          #region RemoveCourseGroupAsync Tests

          [TestMethod]
          public async Task RemoveCourseGroupAsync_WithValidId_ReturnsBool()
          {
               // Arrange
               var groupId = Guid.NewGuid();
               _mockHttpService.Setup(x => x.Delete<bool>(It.IsAny<string>())).ReturnsAsync(true);

               // Act
               var result = await _service.RemoveCourseGroupAsync(groupId);

               // Assert
               Assert.IsTrue(result);
               // Verify DELETE is used (RESTful)
               _mockHttpService.Verify(x => x.Delete<bool>(It.IsAny<string>()), Times.Once);
          }

          [TestMethod]
          [ExpectedException(typeof(ArgumentException))]
          public async Task RemoveCourseGroupAsync_WithEmptyId_ThrowsArgumentException()
          {
               // Act
               await _service.RemoveCourseGroupAsync(Guid.Empty);
          }

          #endregion
     }
}
